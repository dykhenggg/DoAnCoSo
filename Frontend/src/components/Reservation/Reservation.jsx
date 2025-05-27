import React, { useState, useEffect } from "react";
import axios from "axios";
import "./Reservation.css";

const ReservationConfirm = ({ show, onClose }) => {
  if (!show) return null;
  return (
    <div
      className="reservation-popup-backdrop"
      onClick={(e) => {
        if (e.target.className === "reservation-popup-backdrop") onClose();
      }}
    >
      <div className="reservation-popup-container">
        <button className="reservation-popup-close" onClick={onClose}>
          ×
        </button>
        <div className="reservation-popup-content">
          <h2>Đã nhận đặt bàn</h2>
          <p>
            Chúng tôi đã nhận thông tin đặt bàn của bạn.
            <br />
            Chúng tôi sẽ kiểm tra và thông báo cho bạn qua email hoặc số điện
            thoại đã cung cấp.
          </p>
          <button
            className="reservation-popup-confirm-button"
            onClick={onClose}
          >
            OK
          </button>
        </div>
      </div>
    </div>
  );
};

const Reservation = () => {
  const [tables, setTables] = useState([]);
  const [availableTables, setAvailableTables] = useState([]);
  const [form, setForm] = useState({
    name: "",
    phone: "",
    email: "",
    address: "",
    day: "",
    hourStart: "",
    hourEnd: "",
    people: 1,
    tableId: "",
  });
  const [showConfirm, setShowConfirm] = useState(false);
  const [error, setError] = useState("");

  // Lấy danh sách tất cả các bàn khi component mount
  useEffect(() => {
    const fetchTables = async () => {
      try {
        const response = await axios.get("http://localhost:5078/api/Ban");
        // Chỉ lấy các bàn đang hoạt động (TrangThai = true)
        const activeTables = response.data.filter((table) => table.trangThai);
        if (activeTables.length === 0) {
          setError("Hiện tại không có bàn nào đang hoạt động");
          return;
        }
        setTables(activeTables);
        setError(""); // Xóa thông báo lỗi nếu thành công
      } catch (err) {
        console.error(
          "Lỗi khi tải danh sách bàn:",
          err.response?.data || err.message
        );
        setError("Không thể tải danh sách bàn. Vui lòng thử lại sau.");
        setTables([]); // Reset danh sách bàn khi có lỗi
      }
    };
    fetchTables();
  }, []);

  // Lấy danh sách bàn trống khi thay đổi thông tin đặt bàn
  useEffect(() => {
    const fetchAvailableTables = async () => {
      if (!form.day || !form.hourStart || !form.hourEnd || !form.people) return;

      // Trong useEffect fetchAvailableTables
      try {
        // Validate date is not in the past
        const today = new Date();
        const reservationDate = new Date(form.day);
        if (reservationDate < today) {
          setError("Ngày đặt bàn không thể là ngày trong quá khứ");
          return;
        }

        // Convert to UTC time for API request
        const startDateTime = new Date(`${form.day}T${form.hourStart}:00`);
        const endDateTime = new Date(`${form.day}T${form.hourEnd}:00`);

        // Format dates in ISO format and handle timezone
        const startTimeUTC = startDateTime.toISOString();
        const endTimeUTC = endDateTime.toISOString();

        // Validate time difference (minimum 30 minutes, maximum 4 hours)
        const timeDiff = (endDateTime - startDateTime) / (1000 * 60); // in minutes

        if (timeDiff < 30 || timeDiff > 240) {
          setError("Thời gian đặt bàn phải từ 30 phút đến 4 giờ");
          return;
        }

        const response = await axios.get(
          `http://localhost:5078/api/Ban/available?soNguoi=${form.people}&thoiGianBatDau=${startTimeUTC}&thoiGianKetThuc=${endTimeUTC}`
        );
        setAvailableTables(response.data);

        if (
          form.tableId &&
          !response.data.find((t) => t.maBan === parseInt(form.tableId))
        ) {
          setForm((prev) => ({ ...prev, tableId: "" }));
        }
      } catch (err) {
        console.error("Error details:", err.response?.data || err.message);
        setError("Không thể kiểm tra bàn trống. Vui lòng thử lại sau.");
      }
    };

    fetchAvailableTables();
  }, [form.day, form.hourStart, form.hourEnd, form.people]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    try {
      // 1. Kiểm tra số điện thoại đã tồn tại
      const searchResponse = await axios.get(
        `http://localhost:5078/api/KhachHang/search?keyword=${form.phone}`
      );
      let maKH;

      if (searchResponse.data.length > 0) {
        // Nếu khách hàng đã tồn tại, lấy mã khách hàng
        maKH = searchResponse.data[0].maKhachHang;
      } else {
        // 2. Nếu chưa tồn tại, tạo khách hàng mới
        const customerResponse = await axios.post(
          "http://localhost:5078/api/KhachHang",
          {
            hoTen: form.name,
            soDienThoai: form.phone,
            email: form.email,
          }
        );
        maKH = customerResponse.data.maKhachHang;
      }

      // 3. Đặt bàn với MaKH
      const reservationData = {
        maBan: parseInt(form.tableId),
        maKH: maKH,
        thoiGianBatDau: `${form.day}T${form.hourStart}:00`,
        thoiGianKetThuc: `${form.day}T${form.hourEnd}:00`,
        soNguoi: parseInt(form.people),
        ghiChu: form.address,
      };

      await axios.post("http://localhost:5078/api/DatBan", reservationData);
      setShowConfirm(true);
      setForm({
        name: "",
        phone: "",
        email: "",
        address: "",
        day: "",
        hourStart: "",
        hourEnd: "",
        people: 1,
        tableId: "",
      });
    } catch (err) {
      setError(
        err.response?.data || "Có lỗi xảy ra khi đặt bàn. Vui lòng thử lại."
      );
    }
  };

  return (
    <div className="reservation-container">
      <h2>Đặt bàn</h2>
      {error && <div className="error-message">{error}</div>}
      <form
        className="reservation-form"
        onSubmit={handleSubmit}
        autoComplete="off"
      >
        <div className="reservation-form-row">
          <label>
            Họ và tên
            <input
              type="text"
              name="name"
              value={form.name}
              onChange={handleChange}
              placeholder="Họ tên đầy đủ"
              required
            />
          </label>
          <label>
            Số điện thoại
            <input
              type="tel"
              name="phone"
              value={form.phone}
              onChange={handleChange}
              placeholder="0123456789"
              pattern="[0-9]{10,12}"
              required
            />
          </label>
        </div>
        <label>
          Email
          <input
            type="email"
            name="email"
            value={form.email}
            onChange={handleChange}
            placeholder="email@example.com"
          />
        </label>
        <label>
          Địa chỉ
          <input
            type="text"
            name="address"
            value={form.address}
            onChange={handleChange}
            placeholder="Số nhà, đường, phường, quận, thành phố"
            required
          />
        </label>
        <label>
          Chọn bàn
          <select
            name="tableId"
            value={form.tableId}
            onChange={handleChange}
            required
          >
            <option value="">Chọn bàn</option>
            {availableTables.length > 0 ? (
              availableTables.map((table) => (
                <option key={table.maBan} value={table.maBan}>
                  {table.tenBan} - {table.sucChua} chỗ ngồi
                </option>
              ))
            ) : (
              <option value="" disabled>
                {form.day && form.hourStart && form.hourEnd
                  ? "Không có bàn trống trong thời gian này"
                  : "Vui lòng chọn thời gian đặt bàn"}
              </option>
            )}
          </select>
        </label>
        <div className="reservation-form-row">
          <label>
            Ngày
            <input
              type="date"
              name="day"
              value={form.day}
              min={new Date().toISOString().split("T")[0]}
              onChange={handleChange}
              required
            />
          </label>
          <label>
            Giờ vào
            <input
              type="time"
              name="hourStart"
              value={form.hourStart}
              onChange={handleChange}
              required
            />
          </label>
          <label>
            Giờ ra
            <input
              type="time"
              name="hourEnd"
              value={form.hourEnd}
              onChange={handleChange}
              required
            />
          </label>
          <label>
            Số người
            <input
              type="number"
              name="people"
              min="1"
              max="20"
              value={form.people}
              onChange={handleChange}
              required
            />
          </label>
        </div>

        <button type="submit" className="reservation-submit-btn">
          Đặt chỗ
        </button>
      </form>
      <ReservationConfirm
        show={showConfirm}
        onClose={() => setShowConfirm(false)}
      />
    </div>
  );
};

export default Reservation;
