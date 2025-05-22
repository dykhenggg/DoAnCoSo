import React, { useState, useEffect } from "react";
import axios from "axios";
import { toast } from "react-toastify";
import "./Storage.css";

const Storage = () => {
  const [inventory, setInventory] = useState([]);
  const [showAddModal, setShowAddModal] = useState(false);
  const [showEditModal, setShowEditModal] = useState(false);
  const [selectedItem, setSelectedItem] = useState(null);
  const [newItem, setNewItem] = useState({
    tenNguyenLieu: "",
    soLuong: 0,
    donVi: "",
    moTa: ""
  });

  useEffect(() => {
    fetchInventory();
  }, []);

  const fetchInventory = async () => {
    try {
      const response = await axios.get("http://localhost:5078/api/Kho");
      setInventory(response.data);
    } catch (error) {
      toast.error("Lỗi khi tải dữ liệu kho");
    }
  };

  const handleAddItem = async (e) => {
    e.preventDefault();
    try {
      await axios.post("http://localhost:5078/api/Kho", newItem);
      toast.success("Thêm nguyên liệu thành công");
      fetchInventory();
      setShowAddModal(false);
      setNewItem({ tenNguyenLieu: "", soLuong: 0, donVi: "", moTa: "" });
    } catch (error) {
      toast.error("Lỗi khi thêm nguyên liệu");
    }
  };

  const handleEditItem = async (e) => {
    e.preventDefault();
    try {
      await axios.put(`http://localhost:5078/api/Kho/${selectedItem.id}`, selectedItem);
      toast.success("Cập nhật nguyên liệu thành công");
      fetchInventory();
      setShowEditModal(false);
    } catch (error) {
      toast.error("Lỗi khi cập nhật nguyên liệu");
    }
  };

  const handleDeleteItem = async (id) => {
    if (window.confirm("Bạn có chắc muốn xóa nguyên liệu này?")) {
      try {
        await axios.delete(`http://localhost:5078/api/Kho/${id}`);
        toast.success("Xóa nguyên liệu thành công");
        fetchInventory();
      } catch (error) {
        toast.error("Lỗi khi xóa nguyên liệu");
      }
    }
  };

  return (
    <div className="storage-container">
      <div className="storage-header">
        <h2>Quản lý kho</h2>
        <button className="btn-add" onClick={() => setShowAddModal(true)}>
          Thêm nguyên liệu
        </button>
      </div>

      <table className="storage-table">
        <thead>
          <tr>
            <th>Tên nguyên liệu</th>
            <th>Số lượng</th>
            <th>Đơn vị</th>
            <th>Mô tả</th>
            <th>Thao tác</th>
          </tr>
        </thead>
        <tbody>
          {inventory.map((item) => (
            <tr key={item.id}>
              <td>{item.tenNguyenLieu}</td>
              <td>{item.soLuong}</td>
              <td>{item.donVi}</td>
              <td>{item.moTa}</td>
              <td className="action-buttons">
                <button
                  className="btn-edit"
                  onClick={() => {
                    setSelectedItem(item);
                    setShowEditModal(true);
                  }}
                >
                  Sửa
                </button>
                <button
                  className="btn-delete"
                  onClick={() => handleDeleteItem(item.id)}
                >
                  Xóa
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {/* Modal thêm nguyên liệu */}
      {showAddModal && (
        <div className="modal-overlay">
          <div className="modal-content">
            <div className="modal-header">
              <h3>Thêm nguyên liệu mới</h3>
              <button className="btn-close" onClick={() => setShowAddModal(false)}>
                ×
              </button>
            </div>
            <form onSubmit={handleAddItem}>
              <div className="modal-body">
                <div className="form-group">
                  <label>Tên nguyên liệu:</label>
                  <input
                    type="text"
                    value={newItem.tenNguyenLieu}
                    onChange={(e) =>
                      setNewItem({ ...newItem, tenNguyenLieu: e.target.value })
                    }
                    required
                  />
                </div>
                <div className="form-group">
                  <label>Số lượng:</label>
                  <input
                    type="number"
                    value={newItem.soLuong}
                    onChange={(e) =>
                      setNewItem({ ...newItem, soLuong: parseInt(e.target.value) })
                    }
                    required
                  />
                </div>
                <div className="form-group">
                  <label>Đơn vị:</label>
                  <input
                    type="text"
                    value={newItem.donVi}
                    onChange={(e) =>
                      setNewItem({ ...newItem, donVi: e.target.value })
                    }
                    required
                  />
                </div>
                <div className="form-group">
                  <label>Mô tả:</label>
                  <input
                    type="text"
                    value={newItem.moTa}
                    onChange={(e) =>
                      setNewItem({ ...newItem, moTa: e.target.value })
                    }
                  />
                </div>
              </div>
              <div className="modal-footer">
                <button type="submit" className="btn-save">
                  Lưu
                </button>
                <button
                  type="button"
                  className="btn-cancel"
                  onClick={() => setShowAddModal(false)}
                >
                  Hủy
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Modal sửa nguyên liệu */}
      {showEditModal && (
        <div className="modal-overlay">
          <div className="modal-content">
            <div className="modal-header">
              <h3>Sửa thông tin nguyên liệu</h3>
              <button className="btn-close" onClick={() => setShowEditModal(false)}>
                ×
              </button>
            </div>
            <form onSubmit={handleEditItem}>
              <div className="modal-body">
                <div className="form-group">
                  <label>Tên nguyên liệu:</label>
                  <input
                    type="text"
                    value={selectedItem.tenNguyenLieu}
                    onChange={(e) =>
                      setSelectedItem({
                        ...selectedItem,
                        tenNguyenLieu: e.target.value,
                      })
                    }
                    required
                  />
                </div>
                <div className="form-group">
                  <label>Số lượng:</label>
                  <input
                    type="number"
                    value={selectedItem.soLuong}
                    onChange={(e) =>
                      setSelectedItem({
                        ...selectedItem,
                        soLuong: parseInt(e.target.value),
                      })
                    }
                    required
                  />
                </div>
                <div className="form-group">
                  <label>Đơn vị:</label>
                  <input
                    type="text"
                    value={selectedItem.donVi}
                    onChange={(e) =>
                      setSelectedItem({ ...selectedItem, donVi: e.target.value })
                    }
                    required
                  />
                </div>
                <div className="form-group">
                  <label>Mô tả:</label>
                  <input
                    type="text"
                    value={selectedItem.moTa}
                    onChange={(e) =>
                      setSelectedItem({ ...selectedItem, moTa: e.target.value })
                    }
                  />
                </div>
              </div>
              <div className="modal-footer">
                <button type="submit" className="btn-save">
                  Lưu
                </button>
                <button
                  type="button"
                  className="btn-cancel"
                  onClick={() => setShowEditModal(false)}
                >
                  Hủy
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default Storage;
