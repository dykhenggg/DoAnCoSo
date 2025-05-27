import React, { useState, useEffect } from "react";
import "./Suppliers.css";
import axios from "axios";
import { toast } from "react-toastify";

const Suppliers = () => {
  const [suppliers, setSuppliers] = useState([]);
  const [showAddModal, setShowAddModal] = useState(false);
  const [showEditModal, setShowEditModal] = useState(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [selectedSupplier, setSelectedSupplier] = useState(null);
  const [searchTerm, setSearchTerm] = useState("");
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage] = useState(10);

  const [newSupplier, setNewSupplier] = useState({
    tenNhaCungCap: "",
    diaChi: "",
    soDienThoai: "",
    email: "",
    moTa: "",
  });

  // Lọc nhà cung cấp theo từ khóa tìm kiếm
  const filteredSuppliers = suppliers.filter(
    (supplier) =>
      supplier.tenNhaCungCap.toLowerCase().includes(searchTerm.toLowerCase()) ||
      supplier.soDienThoai.includes(searchTerm)
  );

  // Phân trang
  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentItems = filteredSuppliers.slice(
    indexOfFirstItem,
    indexOfLastItem
  );
  const totalPages = Math.ceil(filteredSuppliers.length / itemsPerPage);

  // Fetch danh sách nhà cung cấp
  const fetchSuppliers = async () => {
    try {
      const response = await axios.get("http://localhost:5078/api/NhaCungCap");
      if (response.data) {
        setSuppliers(response.data);
      }
    } catch (error) {
      toast.error("Lỗi khi tải danh sách nhà cung cấp");
    }
  };

  useEffect(() => {
    fetchSuppliers();
  }, []);

  // Thêm nhà cung cấp mới
  const handleAddSupplier = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(
        "http://localhost:5078/api/NhaCungCap",
        newSupplier
      );
      if (response.status === 201) {
        toast.success("Thêm nhà cung cấp thành công");
        fetchSuppliers();
        setShowAddModal(false);
        setNewSupplier({
          tenNhaCungCap: "",
          diaChi: "",
          soDienThoai: "",
          email: "",
          moTa: "",
        });
      }
    } catch (error) {
      toast.error("Lỗi khi thêm nhà cung cấp");
    }
  };

  // Cập nhật nhà cung cấp
  const handleEditSupplier = async (e) => {
    e.preventDefault();
    try {
      await axios.put(
        `http://localhost:5078/api/NhaCungCap/${selectedSupplier.maNhaCungCap}`,
        selectedSupplier
      );
      toast.success("Cập nhật nhà cung cấp thành công");
      fetchSuppliers();
      setShowEditModal(false);
    } catch (error) {
      toast.error("Lỗi khi cập nhật nhà cung cấp");
    }
  };

  // Xóa nhà cung cấp
  const handleDeleteSupplier = async () => {
    try {
      await axios.delete(
        `http://localhost:5078/api/NhaCungCap/${selectedSupplier.maNhaCungCap}`
      );
      toast.success("Xóa nhà cung cấp thành công");
      fetchSuppliers();
      setShowDeleteModal(false);
    } catch (error) {
      toast.error("Lỗi khi xóa nhà cung cấp");
    }
  };

  // Xử lý thay đổi input
  const handleInputChange = (e) => {
    const { name, value } = e.target;
    if (showEditModal) {
      setSelectedSupplier((prev) => ({
        ...prev,
        [name]: value,
      }));
    } else {
      setNewSupplier((prev) => ({
        ...prev,
        [name]: value,
      }));
    }
  };

  return (
    <div className="suppliers-container">
      <div className="suppliers-header">
        <div className="header-left">
          <h2>Quản lý nhà cung cấp</h2>
          <span className="total-count">{suppliers.length} nhà cung cấp</span>
        </div>
        <div className="header-right">
          <div className="search-box">
            <input
              type="text"
              placeholder="Tìm kiếm nhà cung cấp..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
          <button className="add-button" onClick={() => setShowAddModal(true)}>
            <i className="fas fa-plus"></i> Thêm nhà cung cấp
          </button>
        </div>
      </div>

      <div className="suppliers-table">
        <table>
          <thead>
            <tr>
              <th>Tên nhà cung cấp</th>
              <th>Địa chỉ</th>
              <th>Số điện thoại</th>
              <th>Email</th>
              <th>Mô tả</th>
              <th>Thao tác</th>
            </tr>
          </thead>
          <tbody>
            {currentItems.map((supplier) => (
              <tr key={supplier.maNhaCungCap}>
                <td>{supplier.tenNhaCungCap}</td>
                <td>{supplier.diaChi}</td>
                <td>{supplier.soDienThoai}</td>
                <td>{supplier.email}</td>
                <td>{supplier.moTa}</td>
                <td>
                  <button
                    className="edit-button"
                    onClick={() => {
                      setSelectedSupplier(supplier);
                      setShowEditModal(true);
                    }}
                  >
                    <i className="fas fa-edit"></i>
                  </button>
                  <button
                    className="delete-button"
                    onClick={() => {
                      setSelectedSupplier(supplier);
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

      {/* Modal thêm nhà cung cấp */}
      {showAddModal && (
        <div className="modal">
          <div className="modal-content">
            <h3>Thêm nhà cung cấp mới</h3>
            <form onSubmit={handleAddSupplier}>
              <div className="form-group">
                <label>Tên nhà cung cấp:</label>
                <input
                  type="text"
                  name="tenNhaCungCap"
                  value={newSupplier.tenNhaCungCap}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Địa chỉ:</label>
                <input
                  type="text"
                  name="diaChi"
                  value={newSupplier.diaChi}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Số điện thoại:</label>
                <input
                  type="tel"
                  name="soDienThoai"
                  value={newSupplier.soDienThoai}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Email:</label>
                <input
                  type="email"
                  name="email"
                  value={newSupplier.email}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Mô tả:</label>
                <textarea
                  name="moTa"
                  value={newSupplier.moTa}
                  onChange={handleInputChange}
                ></textarea>
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

      {/* Modal chỉnh sửa nhà cung cấp */}
      {showEditModal && selectedSupplier && (
        <div className="modal">
          <div className="modal-content">
            <h3>Chỉnh sửa nhà cung cấp</h3>
            <form onSubmit={handleEditSupplier}>
              <div className="form-group">
                <label>Tên nhà cung cấp:</label>
                <input
                  type="text"
                  name="tenNhaCungCap"
                  value={selectedSupplier.tenNhaCungCap}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Địa chỉ:</label>
                <input
                  type="text"
                  name="diaChi"
                  value={selectedSupplier.diaChi}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Số điện thoại:</label>
                <input
                  type="tel"
                  name="soDienThoai"
                  value={selectedSupplier.soDienThoai}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Email:</label>
                <input
                  type="email"
                  name="email"
                  value={selectedSupplier.email}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Mô tả:</label>
                <textarea
                  name="moTa"
                  value={selectedSupplier.moTa}
                  onChange={handleInputChange}
                ></textarea>
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

      {/* Modal xóa nhà cung cấp */}
      {showDeleteModal && selectedSupplier && (
        <div className="modal">
          <div className="modal-content">
            <h3>Xác nhận xóa</h3>
            <p>
              Bạn có chắc chắn muốn xóa nhà cung cấp "
              {selectedSupplier.tenNhaCungCap}"?
            </p>
            <div className="modal-actions">
              <button className="delete-button" onClick={handleDeleteSupplier}>
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

export default Suppliers;
