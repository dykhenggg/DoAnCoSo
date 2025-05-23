import React, { useState, useEffect } from "react";
import "./Storage.css";
import axios from "axios";
import { toast } from "react-toastify";

const Storage = () => {
  const [storage, setStorage] = useState([]);
  const [showAddModal, setShowAddModal] = useState(false);
  const [showEditModal, setShowEditModal] = useState(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [selectedItem, setSelectedItem] = useState(null);
  const [newItem, setNewItem] = useState({
    tenNguyenLieu: "",
    soLuong: 0,
    donVi: "",
    giaNhap: 0,
    nhaCungCap: "",
  });

  // Khởi tạo selectedItem khi mở modal chỉnh sửa
  const handleEdit = (item) => {
    setSelectedItem({
      ...item,
      nhaCungCap: item.nhaCungCap || "",
    });
    setShowEditModal(true);
  };

  // Reset form khi đóng modal
  const handleCloseModal = () => {
    setShowAddModal(false);
    setShowEditModal(false);
    setShowDeleteModal(false);
    setSelectedItem(null);
    setNewItem({
      tenNguyenLieu: "",
      soLuong: 0,
      donVi: "",
      giaNhap: 0,
      nhaCungCap: "",
    });
  };

  // Cập nhật state khi nhập liệu
  const handleInputChange = (e) => {
    const { name, value } = e.target;
    if (showEditModal) {
      setSelectedItem((prev) => ({
        ...prev,
        [name]: value,
      }));
    } else {
      setNewItem((prev) => ({
        ...prev,
        [name]: value,
      }));
    }
  };

  // Fetch storage items
  const fetchStorage = async () => {
    try {
      const response = await axios.get("http://localhost:5078/api/Kho");
      if (response.data) {
        setStorage(response.data);
      }
    } catch (error) {
      if (error.response) {
        toast.error(`Lỗi server: ${error.response.data}`);
      } else if (error.request) {
        toast.error("Không thể kết nối đến server");
      } else {
        toast.error(`Lỗi: ${error.message}`);
      }
    }
  };

  useEffect(() => {
    fetchStorage();
  }, []);

  // Add new item
  const handleAddItem = async (e) => {
    e.preventDefault();
    try {
      if (!newItem.tenNguyenLieu.trim()) {
        toast.error("Vui lòng nhập tên nguyên liệu");
        return;
      }

      const response = await axios.post(
        "http://localhost:5078/api/Kho",
        newItem
      );
      if (response.status === 201) {
        toast.success("Thêm nguyên liệu thành công");
        await fetchStorage();
        setShowAddModal(false);
        setNewItem({
          tenNguyenLieu: "",
          soLuong: 0,
          donVi: "",
          giaNhap: 0,
          nhaCungCap: "",
        });
      }
    } catch (error) {
      if (error.response) {
        toast.error(`Lỗi: ${error.response.data}`);
      } else if (error.request) {
        toast.error("Không thể kết nối đến server");
      } else {
        toast.error(`Lỗi: ${error.message}`);
      }
    }
  };

  // Edit item
  const handleEditItem = async (e) => {
    e.preventDefault();
    try {
      await axios.put(
        `http://localhost:5078/api/Kho/${selectedItem.maNguyenLieu}`,
        {
          maNguyenLieu: selectedItem.maNguyenLieu,
          tenNguyenLieu: selectedItem.tenNguyenLieu,
          soLuong: selectedItem.soLuong,
          donVi: selectedItem.donVi,
          giaNhap: selectedItem.giaNhap,
          nhaCungCap: selectedItem.nhaCungCap,
        }
      );
      toast.success("Cập nhật nguyên liệu thành công");
      fetchStorage();
      setShowEditModal(false);
    } catch (error) {
      if (error.response) {
        toast.error(`Lỗi: ${error.response.data}`);
      } else if (error.request) {
        toast.error("Không thể kết nối đến server");
      } else {
        toast.error(`Lỗi: ${error.message}`);
      }
    }
  };

  // Delete item
  const handleDeleteItem = async () => {
    try {
      await axios.delete(
        `http://localhost:5078/api/Kho/${selectedItem.maNguyenLieu}`
      );
      toast.success("Xóa nguyên liệu thành công");
      fetchStorage();
      setShowDeleteModal(false);
    } catch (error) {
      toast.error("Lỗi khi xóa nguyên liệu");
    }
  };

  return (
    <div className="storage-container">
      <div className="storage-header">
        <div className="header-left">
          <h2>Quản lý kho</h2>
          <span className="total-count">{storage.length} nguyên liệu</span>
        </div>
        <button className="add-button" onClick={() => setShowAddModal(true)}>
          <i className="fas fa-plus"></i> Thêm nguyên liệu
        </button>
      </div>

      <div className="storage-grid">
        {storage.map((item) => (
          <div key={item.maNguyenLieu} className="storage-card">
            <div className="storage-header">
              <h3>{item.tenNguyenLieu}</h3>
              <div className="storage-actions">
                <button
                  className="edit-button"
                  onClick={() => handleEdit(item)}
                >
                  <i className="fas fa-edit"></i>
                </button>
                <button
                  className="delete-button"
                  onClick={() => {
                    setSelectedItem(item);
                    setShowDeleteModal(true);
                  }}
                >
                  <i className="fas fa-trash"></i>
                </button>
              </div>
            </div>
            <div className="storage-details">
              <p><strong>Số lượng:</strong> {item.soLuong} {item.donVi}</p>
              <p><strong>Giá nhập:</strong> {item.giaNhap.toLocaleString('vi-VN')} VNĐ</p>
              <p><strong>Nhà cung cấp:</strong> {item.nhaCungCap}</p>
            </div>
          </div>
        ))}
      </div>

      {/* Add Modal */}
      {showAddModal && (
        <div className="modal">
          <div className="modal-content">
            <h3>Thêm nguyên liệu mới</h3>
            <form onSubmit={handleAddItem}>
              <div className="form-group">
                <label>Tên nguyên liệu:</label>
                <input
                  type="text"
                  name="tenNguyenLieu"
                  value={newItem.tenNguyenLieu}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Số lượng:</label>
                <input
                  type="number"
                  name="soLuong"
                  value={newItem.soLuong}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Đơn vị:</label>
                <input
                  type="text"
                  name="donVi"
                  value={newItem.donVi}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Giá nhập:</label>
                <input
                  type="number"
                  name="giaNhap"
                  value={newItem.giaNhap}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Nhà cung cấp:</label>
                <input
                  type="text"
                  name="nhaCungCap"
                  value={newItem.nhaCungCap}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="modal-actions">
                <button type="submit" className="save-button">Lưu</button>
                <button type="button" className="cancel-button" onClick={handleCloseModal}>
                  Hủy
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Edit Modal */}
      {showEditModal && selectedItem && (
        <div className="modal">
          <div className="modal-content">
            <h3>Chỉnh sửa nguyên liệu</h3>
            <form onSubmit={handleEditItem}>
              <div className="form-group">
                <label>Tên nguyên liệu:</label>
                <input
                  type="text"
                  name="tenNguyenLieu"
                  value={selectedItem.tenNguyenLieu}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Số lượng:</label>
                <input
                  type="number"
                  name="soLuong"
                  value={selectedItem.soLuong}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Đơn vị:</label>
                <input
                  type="text"
                  name="donVi"
                  value={selectedItem.donVi}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Giá nhập:</label>
                <input
                  type="number"
                  name="giaNhap"
                  value={selectedItem.giaNhap}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Nhà cung cấp:</label>
                <input
                  type="text"
                  name="nhaCungCap"
                  value={selectedItem.nhaCungCap}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="modal-actions">
                <button type="submit" className="save-button">Lưu</button>
                <button type="button" className="cancel-button" onClick={handleCloseModal}>
                  Hủy
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Delete Modal */}
      {showDeleteModal && selectedItem && (
        <div className="modal">
          <div className="modal-content">
            <h3>Xác nhận xóa</h3>
            <p>Bạn có chắc chắn muốn xóa nguyên liệu "{selectedItem.tenNguyenLieu}"?</p>
            <div className="modal-actions">
              <button className="delete-button" onClick={handleDeleteItem}>
                Xóa
              </button>
              <button className="cancel-button" onClick={handleCloseModal}>
                Hủy
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default Storage;
