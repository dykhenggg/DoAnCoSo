import React, { useState, useEffect } from "react";
import "./Shifts.css";
import axios from "axios";
import { toast } from "react-toastify";

const Shifts = () => {
  const [shifts, setShifts] = useState([]);
  const [employees, setEmployees] = useState([]);
  const [showAddModal, setShowAddModal] = useState(false);
  const [showEditModal, setShowEditModal] = useState(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [selectedShift, setSelectedShift] = useState(null);
  const [newShift, setNewShift] = useState({
    maNhanVien: "",
    gioBatDau: "",
    gioKetThuc: "",
  });

  // Fetch shifts
  const fetchShifts = async () => {
    try {
      const response = await axios.get("http://localhost:5078/api/CaLamViec");
      setShifts(response.data);
    } catch (error) {
      toast.error("Lỗi khi tải danh sách ca làm việc");
    }
  };

  // Fetch employees
  const fetchEmployees = async () => {
    try {
      const response = await axios.get("http://localhost:5078/api/NhanVien");
      setEmployees(response.data);
    } catch (error) {
      toast.error("Lỗi khi tải danh sách nhân viên");
    }
  };

  useEffect(() => {
    fetchShifts();
    fetchEmployees();
  }, []);

  // Add new shift
  const handleAddShift = async (e) => {
    e.preventDefault();
    try {
      await axios.post("http://localhost:5078/api/CaLamViec", newShift);
      toast.success("Thêm ca làm việc thành công");
      fetchShifts();
      setShowAddModal(false);
      setNewShift({
        maNhanVien: "",
        gioBatDau: "",
        gioKetThuc: "",
      });
    } catch (error) {
      toast.error("Lỗi khi thêm ca làm việc");
    }
  };

  // Delete shift
  const handleDeleteShift = async () => {
    try {
      await axios.delete(
        `http://localhost:5078/api/CaLamViec/${selectedShift.maCa}`
      );
      toast.success("Xóa ca làm việc thành công");
      fetchShifts();
      setShowDeleteModal(false);
    } catch (error) {
      toast.error("Lỗi khi xóa ca làm việc");
    }
  };

  return (
    <div className="shifts-container">
      <h2>Quản lý ca làm việc</h2>

      <div className="shifts-table-container">
        <table className="shifts-table">
          <thead>
            <tr>
              <th>Nhân viên</th>
              <th>Giờ bắt đầu</th>
              <th>Giờ kết thúc</th>
              <th>Thao tác</th>
            </tr>
          </thead>
          <tbody>
            {shifts.map((shift) => (
              <tr key={shift.maCa}>
                <td>{shift.nhanVien?.hoTen}</td>
                <td>{shift.gioBatDau}</td>
                <td>{shift.gioKetThuc}</td>
                <td>
                  <button
                    className="edit-button"
                    onClick={() => {
                      setSelectedShift(shift);
                      setShowEditModal(true);
                    }}
                  >
                    Sửa
                  </button>
                  <button
                    className="delete-button"
                    onClick={() => {
                      setSelectedShift(shift);
                      setShowDeleteModal(true);
                    }}
                  >
                    Xóa
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <button className="add-button" onClick={() => setShowAddModal(true)}>
        + Thêm ca làm việc
      </button>

      {/* Add Modal */}
      {showAddModal && (
        <div className="modal">
          <div className="modal-content">
            <h3>Thêm ca làm việc mới</h3>
            <form onSubmit={handleAddShift}>
              <div className="form-group">
                <label>Nhân viên:</label>
                <select
                  value={newShift.maNhanVien}
                  onChange={(e) =>
                    setNewShift({ ...newShift, maNhanVien: e.target.value })
                  }
                  required
                >
                  <option value="">Chọn nhân viên</option>
                  {employees.map((employee) => (
                    <option key={employee.maNV} value={employee.maNV}>
                      {employee.hoTen}
                    </option>
                  ))}
                </select>
              </div>
              <div className="form-group">
                <label>Giờ bắt đầu:</label>
                <input
                  type="time"
                  value={newShift.gioBatDau}
                  onChange={(e) =>
                    setNewShift({ ...newShift, gioBatDau: e.target.value })
                  }
                  required
                />
              </div>
              <div className="form-group">
                <label>Giờ kết thúc:</label>
                <input
                  type="time"
                  value={newShift.gioKetThuc}
                  onChange={(e) =>
                    setNewShift({ ...newShift, gioKetThuc: e.target.value })
                  }
                  required
                />
              </div>
              <div className="modal-actions">
                <button type="submit">Thêm</button>
                <button type="button" onClick={() => setShowAddModal(false)}>
                  Hủy
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Delete Modal */}
      {showDeleteModal && (
        <div className="modal">
          <div className="modal-content">
            <h3>Xác nhận xóa</h3>
            <p>
              Bạn có chắc chắn muốn xóa ca làm việc của nhân viên{" "}
              {selectedShift.nhanVien?.hoTen} không?
            </p>
            <div className="modal-actions">
              <button onClick={handleDeleteShift}>Xóa</button>
              <button onClick={() => setShowDeleteModal(false)}>Hủy</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default Shifts;
