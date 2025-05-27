import React, { useState, useEffect } from "react";
import "./Promotions.css";
import axios from "axios";
import { toast } from "react-toastify";

const Promotions = () => {
  const [promotions, setPromotions] = useState([]);
  const [showAddModal, setShowAddModal] = useState(false);
  const [showEditModal, setShowEditModal] = useState(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [selectedPromotion, setSelectedPromotion] = useState(null);
  const [searchTerm, setSearchTerm] = useState("");
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage] = useState(10);

  const [newPromotion, setNewPromotion] = useState({
    tenKhuyenMai: "",
    moTa: "",
    ngayBatDau: "",
    ngayKetThuc: "",
    phanTramGiam: 0,
    dieuKien: "",
    trangThai: true,
  });

  // Lọc khuyến mãi theo từ khóa tìm kiếm
  const filteredPromotions = promotions.filter((promotion) =>
    promotion.tenKhuyenMai.toLowerCase().includes(searchTerm.toLowerCase())
  );

  // Phân trang
  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentItems = filteredPromotions.slice(
    indexOfFirstItem,
    indexOfLastItem
  );
  const totalPages = Math.ceil(filteredPromotions.length / itemsPerPage);

  // Fetch danh sách khuyến mãi
  const fetchPromotions = async () => {
    try {
      const response = await axios.get("http://localhost:5078/api/KhuyenMai");
      if (response.data) {
        setPromotions(response.data);
      }
    } catch (error) {
      toast.error("Lỗi khi tải danh sách khuyến mãi");
    }
  };

  useEffect(() => {
    fetchPromotions();
  }, []);

  // Thêm khuyến mãi mới
  const handleAddPromotion = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(
        "http://localhost:5078/api/KhuyenMai",
        newPromotion
      );
      if (response.status === 201) {
        toast.success("Thêm khuyến mãi thành công");
        fetchPromotions();
        setShowAddModal(false);
        setNewPromotion({
          tenKhuyenMai: "",
          moTa: "",
          ngayBatDau: "",
          ngayKetThuc: "",
          phanTramGiam: 0,
          dieuKien: "",
          trangThai: true,
        });
      }
    } catch (error) {
      toast.error("Lỗi khi thêm khuyến mãi");
    }
  };

  // Cập nhật khuyến mãi
  const handleEditPromotion = async (e) => {
    e.preventDefault();
    try {
      await axios.put(
        `http://localhost:5078/api/KhuyenMai/${selectedPromotion.maKhuyenMai}`,
        selectedPromotion
      );
      toast.success("Cập nhật khuyến mãi thành công");
      fetchPromotions();
      setShowEditModal(false);
    } catch (error) {
      toast.error("Lỗi khi cập nhật khuyến mãi");
    }
  };

  // Xóa khuyến mãi
  const handleDeletePromotion = async () => {
    try {
      await axios.delete(
        `http://localhost:5078/api/KhuyenMai/${selectedPromotion.maKhuyenMai}`
      );
      toast.success("Xóa khuyến mãi thành công");
      fetchPromotions();
      setShowDeleteModal(false);
    } catch (error) {
      toast.error("Lỗi khi xóa khuyến mãi");
    }
  };

  // Xử lý thay đổi input
  const handleInputChange = (e) => {
    const { name, value } = e.target;
    if (showEditModal) {
      setSelectedPromotion((prev) => ({
        ...prev,
        [name]: value,
      }));
    } else {
      setNewPromotion((prev) => ({
        ...prev,
        [name]: value,
      }));
    }
  };

  return (
    <div className="promotions-container">
      <div className="promotions-header">
        <div className="header-left">
          <h2>Quản lý khuyến mãi</h2>
          <span className="total-count">{promotions.length} khuyến mãi</span>
        </div>
        <div className="header-right">
          <div className="search-box">
            <input
              type="text"
              placeholder="Tìm kiếm khuyến mãi..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
          <button className="add-button" onClick={() => setShowAddModal(true)}>
            <i className="fas fa-plus"></i> Thêm khuyến mãi
          </button>
        </div>
      </div>

      <div className="promotions-table">
        <table>
          <thead>
            <tr>
              <th>Tên khuyến mãi</th>
              <th>Mô tả</th>
              <th>Ngày bắt đầu</th>
              <th>Ngày kết thúc</th>
              <th>Phần trăm giảm</th>
              <th>Điều kiện</th>
              <th>Trạng thái</th>
              <th>Thao tác</th>
            </tr>
          </thead>
          <tbody>
            {currentItems.map((promotion) => (
              <tr key={promotion.maKhuyenMai}>
                <td>{promotion.tenKhuyenMai}</td>
                <td>{promotion.moTa}</td>
                <td>{new Date(promotion.ngayBatDau).toLocaleDateString()}</td>
                <td>{new Date(promotion.ngayKetThuc).toLocaleDateString()}</td>
                <td>{promotion.phanTramGiam}%</td>
                <td>{promotion.dieuKien}</td>
                <td>
                  <span
                    className={`status ${
                      promotion.trangThai ? "active" : "inactive"
                    }`}
                  >
                    {promotion.trangThai ? "Đang áp dụng" : "Đã kết thúc"}
                  </span>
                </td>
                <td>
                  <button
                    className="edit-button"
                    onClick={() => {
                      setSelectedPromotion(promotion);
                      setShowEditModal(true);
                    }}
                  >
                    <i className="fas fa-edit"></i>
                  </button>
                  <button
                    className="delete-button"
                    onClick={() => {
                      setSelectedPromotion(promotion);
                      setShowDeleteModal(true);
                    }}
                  >
                    <i className="fas fa-trash"></i>
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <div className="pagination">
        <button
          onClick={() => setCurrentPage(currentPage - 1)}
          disabled={currentPage === 1}
        >
          <i className="fas fa-chevron-left"></i> Trước
        </button>
        <span>
          {currentPage} / {totalPages}
        </span>
        <button
          onClick={() => setCurrentPage(currentPage + 1)}
          disabled={currentPage === totalPages}
        >
          Sau <i className="fas fa-chevron-right"></i>
        </button>
      </div>

      {/* Modal thêm khuyến mãi */}
      {showAddModal && (
        <div className="modal">
          <div className="modal-content">
            <h3>Thêm khuyến mãi mới</h3>
            <form onSubmit={handleAddPromotion}>
              <div className="form-group">
                <label>Tên khuyến mãi:</label>
                <input
                  type="text"
                  name="tenKhuyenMai"
                  value={newPromotion.tenKhuyenMai}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Mô tả:</label>
                <textarea
                  name="moTa"
                  value={newPromotion.moTa}
                  onChange={handleInputChange}
                ></textarea>
              </div>
              <div className="form-group">
                <label>Ngày bắt đầu:</label>
                <input
                  type="date"
                  name="ngayBatDau"
                  value={newPromotion.ngayBatDau}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Ngày kết thúc:</label>
                <input
                  type="date"
                  name="ngayKetThuc"
                  value={newPromotion.ngayKetThuc}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Phần trăm giảm:</label>
                <input
                  type="number"
                  name="phanTramGiam"
                  value={newPromotion.phanTramGiam}
                  onChange={handleInputChange}
                  min="0"
                  max="100"
                  required
                />
              </div>
              <div className="form-group">
                <label>Điều kiện:</label>
                <input
                  type="text"
                  name="dieuKien"
                  value={newPromotion.dieuKien}
                  onChange={handleInputChange}
                />
              </div>
              <div className="form-group">
                <label>Trạng thái:</label>
                <select
                  name="trangThai"
                  value={newPromotion.trangThai}
                  onChange={handleInputChange}
                >
                  <option value={true}>Đang áp dụng</option>
                  <option value={false}>Đã kết thúc</option>
                </select>
              </div>
              <div className="modal-actions">
                <button type="submit" className="save-button">
                  Lưu
                </button>
                <button
                  type="button"
                  className="cancel-button"
                  onClick={() => setShowAddModal(false)}
                >
                  Hủy
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Modal chỉnh sửa khuyến mãi */}
      {showEditModal && selectedPromotion && (
        <div className="modal">
          <div className="modal-content">
            <h3>Chỉnh sửa khuyến mãi</h3>
            <form onSubmit={handleEditPromotion}>
              <div className="form-group">
                <label>Tên khuyến mãi:</label>
                <input
                  type="text"
                  name="tenKhuyenMai"
                  value={selectedPromotion.tenKhuyenMai}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Mô tả:</label>
                <textarea
                  name="moTa"
                  value={selectedPromotion.moTa}
                  onChange={handleInputChange}
                ></textarea>
              </div>
              <div className="form-group">
                <label>Ngày bắt đầu:</label>
                <input
                  type="date"
                  name="ngayBatDau"
                  value={selectedPromotion.ngayBatDau.split("T")[0]}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Ngày kết thúc:</label>
                <input
                  type="date"
                  name="ngayKetThuc"
                  value={selectedPromotion.ngayKetThuc.split("T")[0]}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Phần trăm giảm:</label>
                <input
                  type="number"
                  name="phanTramGiam"
                  value={selectedPromotion.phanTramGiam}
                  onChange={handleInputChange}
                  min="0"
                  max="100"
                  required
                />
              </div>
              <div className="form-group">
                <label>Điều kiện:</label>
                <input
                  type="text"
                  name="dieuKien"
                  value={selectedPromotion.dieuKien}
                  onChange={handleInputChange}
                />
              </div>
              <div className="form-group">
                <label>Trạng thái:</label>
                <select
                  name="trangThai"
                  value={selectedPromotion.trangThai}
                  onChange={handleInputChange}
                >
                  <option value={true}>Đang áp dụng</option>
                  <option value={false}>Đã kết thúc</option>
                </select>
              </div>
              <div className="modal-actions">
                <button type="submit" className="save-button">
                  Lưu
                </button>
                <button
                  type="button"
                  className="cancel-button"
                  onClick={() => setShowEditModal(false)}
                >
                  Hủy
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Modal xóa khuyến mãi */}
      {showDeleteModal && selectedPromotion && (
        <div className="modal">
          <div className="modal-content">
            <h3>Xác nhận xóa</h3>
            <p>
              Bạn có chắc chắn muốn xóa khuyến mãi "
              {selectedPromotion.tenKhuyenMai}"?
            </p>
            <div className="modal-actions">
              <button className="delete-button" onClick={handleDeletePromotion}>
                Xóa
              </button>
              <button
                className="cancel-button"
                onClick={() => setShowDeleteModal(false)}
              >
                Hủy
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default Promotions;
