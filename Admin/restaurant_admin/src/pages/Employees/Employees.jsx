import React, { useState, useEffect } from "react";
import "./Employees.css";
import axios from "axios";
import { toast } from "react-toastify";

const Employees = () => {
  const [employees, setEmployees] = useState([]);
  const [departments, setDepartments] = useState([]);
  const [showAddModal, setShowAddModal] = useState(false);
  const [showEditModal, setShowEditModal] = useState(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [selectedEmployee, setSelectedEmployee] = useState(null);
  const [searchTerm, setSearchTerm] = useState("");
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage] = useState(10);

  const [newEmployee, setNewEmployee] = useState({
    hoTen: "",
    email: "",
    SDT: "",
    diaChi: "",
    maBoPhan: "",
    chucVu: "",
    matKhau: "",
  });

  const handleAddEmployee = async (e) => {
    e.preventDefault();

    // Validation đầy đủ
    if (!newEmployee.hoTen.trim()) {
      toast.error("Vui lòng nhập họ tên");
      return;
    }
    if (!newEmployee.email.trim()) {
      toast.error("Vui lòng nhập email");
      return;
    }
    if (!newEmployee.soDienThoai.trim()) {
      toast.error("Vui lòng nhập số điện thoại");
      return;
    }
    if (!newEmployee.maBoPhan) {
      toast.error("Vui lòng chọn bộ phận");
      return;
    }
    if (!newEmployee.chucVu.trim()) {
      toast.error("Vui lòng nhập chức vụ");
      return;
    }

    // Validate email format
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(newEmployee.email.trim())) {
      toast.error("Email không hợp lệ");
      return;
    }

    // Validate phone number format (10 digits)
    const phoneRegex = /^\d{10}$/;
    if (!phoneRegex.test(newEmployee.soDienThoai.trim())) {
      toast.error("Số điện thoại phải có 10 chữ số");
      return;
    }

    try {
      const employeeData = {
        HoTen: newEmployee.hoTen.trim(),
        Email: newEmployee.email.trim().toLowerCase(),
        SDT: newEmployee.soDienThoai.trim(),
        DiaChi: newEmployee.diaChi.trim(),
        MaBoPhan: parseInt(newEmployee.maBoPhan),
        ChucVu: newEmployee.chucVu.trim(),
        MatKhau: newEmployee.matKhau,
      };

      const response = await axios.post(
        "http://localhost:5078/api/NhanVien",
        employeeData
      );

      if (response.status === 201) {
        toast.success("Thêm nhân viên thành công");
        await fetchEmployees();
        handleCloseModal();
      }
    } catch (error) {
      if (error.response?.data) {
        toast.error(error.response.data);
      } else {
        toast.error("Lỗi khi thêm nhân viên");
      }
    }
  };

  const handleEdit = (employee) => {
    setSelectedEmployee(employee);
    setShowEditModal(true);
  };

  const handleCloseModal = () => {
    setShowAddModal(false);
    setShowEditModal(false);
    setShowDeleteModal(false);
    setSelectedEmployee(null);
    setNewEmployee({
      hoTen: "",
      email: "",
      soDienThoai: "",
      diaChi: "",
      ngaySinh: "",
      gioiTinh: true,
      maBoPhan: "",
      chucVu: "",
      matKhau: "123456",
    });
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    if (showEditModal) {
      setSelectedEmployee((prev) => ({
        ...prev,
        [name]: value,
      }));
    } else {
      setNewEmployee((prev) => ({
        ...prev,
        [name]: value,
      }));
    }
  };

  const fetchEmployees = async () => {
    try {
      const response = await axios.get("http://localhost:5078/api/NhanVien");
      if (response.data) {
        setEmployees(response.data);
      }
    } catch (error) {
      toast.error("Lỗi khi tải danh sách nhân viên");
    }
  };

  const fetchDepartments = async () => {
    try {
      const response = await axios.get("http://localhost:5078/api/BoPhan");
      if (response.data) {
        setDepartments(response.data);
      }
    } catch (error) {
      toast.error("Lỗi khi tải danh sách bộ phận");
    }
  };

  useEffect(() => {
    fetchEmployees();
    fetchDepartments();
  }, []);

  const handleEditEmployee = async (e) => {
    e.preventDefault();
    try {
      await axios.put(
        `http://localhost:5078/api/NhanVien/${selectedEmployee.maNhanVien}`,
        selectedEmployee
      );
      toast.success("Cập nhật nhân viên thành công");
      fetchEmployees();
      handleCloseModal();
    } catch (error) {
      toast.error("Lỗi khi cập nhật nhân viên");
    }
  };

  const handleDeleteEmployee = async () => {
    try {
      await axios.delete(
        `http://localhost:5078/api/NhanVien/${selectedEmployee.maNhanVien}`
      );
      toast.success("Xóa nhân viên thành công");
      fetchEmployees();
      handleCloseModal();
    } catch (error) {
      toast.error("Lỗi khi xóa nhân viên");
    }
  };

  // Tìm kiếm và phân trang
  const filteredEmployees = employees.filter(
    (employee) =>
      employee.hoTen.toLowerCase().includes(searchTerm.toLowerCase()) ||
      employee.soDienThoai.includes(searchTerm) ||
      employee.email.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentItems = filteredEmployees.slice(
    indexOfFirstItem,
    indexOfLastItem
  );
  const totalPages = Math.ceil(filteredEmployees.length / itemsPerPage);

  const paginate = (pageNumber) => setCurrentPage(pageNumber);

  return (
    <div className="employees-container">
      <div className="employees-header">
        <div className="header-left">
          <h2>Quản lý nhân viên</h2>
          <span className="total-count">{employees.length} nhân viên</span>
        </div>
        <div className="header-right">
          <input
            type="text"
            placeholder="Tìm kiếm..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="search-input"
          />
          <button className="add-button" onClick={() => setShowAddModal(true)}>
            <i className="fas fa-plus"></i> Thêm nhân viên
          </button>
        </div>
      </div>

      <div className="employees-table">
        <table>
          <thead>
            <tr>
              <th>Họ tên</th>
              <th>Số điện thoại</th>
              <th>Email</th>
              <th>Bộ phận</th>
              <th>Chức vụ</th>
              <th>Thao tác</th>
            </tr>
          </thead>
          <tbody>
            {currentItems.map((employee) => (
              <tr key={employee.maNhanVien}>
                <td>{employee.hoTen}</td>
                <td>{employee.soDienThoai}</td>
                <td>{employee.email}</td>
                <td>
                  {
                    departments.find((d) => d.maBoPhan === employee.maBoPhan)
                      ?.tenBoPhan
                  }
                </td>
                <td>{employee.chucVu}</td>
                <td>
                  <button
                    className="edit-button"
                    onClick={() => handleEdit(employee)}
                  >
                    <i className="fas fa-edit"></i>
                  </button>
                  <button
                    className="delete-button"
                    onClick={() => {
                      setSelectedEmployee(employee);
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
        {Array.from({ length: totalPages }, (_, i) => (
          <button
            key={i + 1}
            onClick={() => paginate(i + 1)}
            className={currentPage === i + 1 ? "active" : ""}
          >
            {i + 1}
          </button>
        ))}
      </div>

      {/* Add Modal */}
      {showAddModal && (
        <div className="modal">
          <div className="modal-content">
            <h2>Thêm nhân viên mới</h2>
            <form onSubmit={handleAddEmployee}>
              <div className="form-group">
                <label>Họ tên:</label>
                <input
                  type="text"
                  name="hoTen"
                  value={newEmployee.hoTen}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Email:</label>
                <input
                  type="email"
                  name="email"
                  value={newEmployee.email}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Số điện thoại:</label>
                <input
                  type="tel"
                  name="soDienThoai"
                  value={newEmployee.soDienThoai}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Địa chỉ:</label>
                <input
                  type="text"
                  name="diaChi"
                  value={newEmployee.diaChi}
                  onChange={handleInputChange}
                />
              </div>
              <div className="form-group">
                <label>Bộ phận:</label>
                <select
                  name="maBoPhan"
                  value={newEmployee.maBoPhan}
                  onChange={handleInputChange}
                  required
                >
                  <option value="">Chọn bộ phận</option>
                  {departments.map((department) => (
                    <option
                      key={department.maBoPhan}
                      value={department.maBoPhan}
                    >
                      {department.tenBoPhan}
                    </option>
                  ))}
                </select>
              </div>
              <div className="form-group">
                <label>Chức vụ:</label>
                <input
                  type="text"
                  name="chucVu"
                  value={newEmployee.chucVu}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="modal-actions">
                <button type="submit" className="submit-button">
                  Thêm
                </button>
                <button
                  type="button"
                  className="cancel-button"
                  onClick={handleCloseModal}
                >
                  Hủy
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Edit Modal */}
      {showEditModal && (
        <div className="modal">
          <div className="modal-content">
            <h2>Chỉnh sửa nhân viên</h2>
            <form onSubmit={handleEditEmployee}>
              <div className="form-group">
                <label>Họ tên:</label>
                <input
                  type="text"
                  name="hoTen"
                  value={selectedEmployee.hoTen}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Số điện thoại:</label>
                <input
                  type="tel"
                  name="soDienThoai"
                  value={selectedEmployee.soDienThoai}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Email:</label>
                <input
                  type="email"
                  name="email"
                  value={selectedEmployee.email}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Địa chỉ:</label>
                <input
                  type="text"
                  name="diaChi"
                  value={selectedEmployee.diaChi}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Ngày sinh:</label>
                <input
                  type="date"
                  name="ngaySinh"
                  value={selectedEmployee.ngaySinh}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label>Giới tính:</label>
                <select
                  name="gioiTinh"
                  value={selectedEmployee.gioiTinh}
                  onChange={handleInputChange}
                  required
                >
                  <option value={true}>Nam</option>
                  <option value={false}>Nữ</option>
                </select>
              </div>
              <div className="form-group">
                <label>Bộ phận:</label>
                <select
                  name="maBoPhan"
                  value={selectedEmployee.maBoPhan}
                  onChange={handleInputChange}
                  required
                >
                  <option value="">Chọn bộ phận</option>
                  {departments.map((department) => (
                    <option
                      key={department.maBoPhan}
                      value={department.maBoPhan}
                    >
                      {department.tenBoPhan}
                    </option>
                  ))}
                </select>
              </div>
              <div className="form-group">
                <label>Chức vụ:</label>
                <input
                  type="text"
                  name="chucVu"
                  value={selectedEmployee.chucVu}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="modal-actions">
                <button type="submit" className="submit-button">
                  Lưu
                </button>
                <button
                  type="button"
                  className="cancel-button"
                  onClick={handleCloseModal}
                >
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
            <h2>Xác nhận xóa</h2>
            <p>Bạn có chắc chắn muốn xóa nhân viên này?</p>
            <div className="modal-actions">
              <button className="delete-button" onClick={handleDeleteEmployee}>
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

export default Employees;
