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
  const [searchTerm, setSearchTerm] = useState("");
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage] = useState(8);
  const [sortField, setSortField] = useState(null);
  const [sortDirection, setSortDirection] = useState("asc");
  const [suppliers, setSuppliers] = useState([]);
  const [transactions, setTransactions] = useState([]);
  const [showTransactionModal, setShowTransactionModal] = useState(false);
  const [newItem, setNewItem] = useState({
    tenNguyenLieu: "",
    donVi: "",
    soLuongHienTai: 0,
    soLuongToiThieu: 0,
    maNCC: 1, // Mặc định là 1, bạn cần thay đổi thành ID thực tế của nhà cung cấp
    trangThai: "Active",
  });

  const [newTransaction, setNewTransaction] = useState({
    maNguyenLieu: "",
    loaiGiaoDich: "nhap", // "nhap" hoặc "xuat"
    soLuong: 0,
    ngayGiaoDich: new Date().toISOString().split("T")[0],
    ghiChu: "",
  });

  // Lọc và sắp xếp dữ liệu
  const filteredItems = storage.filter((item) =>
    item.tenNguyenLieu.toLowerCase().includes(searchTerm.toLowerCase().trim())
  );

  const sortedItems = sortField
    ? [...filteredItems].sort((a, b) => {
        if (sortDirection === "asc") {
          return a[sortField] > b[sortField] ? 1 : -1;
        }
        return a[sortField] < b[sortField] ? 1 : -1;
      })
    : filteredItems;

  // Phân trang
  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentItems = sortedItems.slice(indexOfFirstItem, indexOfLastItem);
  const totalPages = Math.ceil(sortedItems.length / itemsPerPage);

  // Reset trang về 1 khi thay đổi searchTerm
  useEffect(() => {
    setCurrentPage(1);
  }, [searchTerm]);

  // Xử lý giao dịch kho
  const handleTransaction = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(
        "http://localhost:5078/api/Kho/giaodich",
        newTransaction
      );
      if (response.status === 200) {
        toast.success("Giao dịch thành công");
        fetchStorage();
        setShowTransactionModal(false);
      }
    } catch (error) {
      toast.error("Lỗi khi thực hiện giao dịch");
    }
  };

  // Kiểm kê kho
  const handleInventoryCheck = async () => {
    try {
      const response = await axios.get("http://localhost:5078/api/Kho/kiemke");
      if (response.status === 200) {
        toast.success("Kiểm kê kho thành công");
        setStorage(response.data);
      }
    } catch (error) {
      toast.error("Lỗi khi kiểm kê kho");
    }
  };

  // Handle edit button click
  const handleEdit = (item) => {
    setSelectedItem({
      maNguyenLieu: item.maNguyenLieu,
      tenNguyenLieu: item.tenNguyenLieu,
      donVi: item.donVi,
      soLuongHienTai: item.soLuongHienTai,
      soLuongToiThieu: item.soLuongToiThieu,
      maNCC: item.nhaCungCap?.maNCC || item.maNCC,
      trangThai: item.trangThai || "Active",
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
      donVi: "",
      soLuongHienTai: 0,
      soLuongToiThieu: 0,
      maNCC: 1,
      trangThai: "Active",
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

  // Fetch suppliers
  const fetchSuppliers = async () => {
    try {
      const response = await axios.get("http://localhost:5078/api/NhaCungCap");
      if (response.data) {
        // Chỉ lấy nhà cung cấp đang hoạt động
        const activeSuppliers = response.data.filter(
          (supplier) => supplier.trangThai === "Active"
        );
        setSuppliers(activeSuppliers);
      }
    } catch (error) {
      toast.error("Không thể lấy danh sách nhà cung cấp");
    }
  };

  useEffect(() => {
    fetchStorage();
    fetchSuppliers();
  }, []);

  // Add new item
  const handleAddItem = async (e) => {
    e.preventDefault();
    try {
      // Validate required fields
      if (!newItem.tenNguyenLieu.trim()) {
        toast.error("Vui lòng nhập tên nguyên liệu");
        return;
      }
      if (newItem.tenNguyenLieu.trim().length > 100) {
        toast.error("Tên nguyên liệu không được vượt quá 100 ký tự");
        return;
      }
      if (!newItem.donVi.trim()) {
        toast.error("Vui lòng nhập đơn vị");
        return;
      }
      if (newItem.donVi.trim().length > 20) {
        toast.error("Đơn vị không được vượt quá 20 ký tự");
        return;
      }

      // Tìm nhà cung cấp được chọn
      const selectedSupplier = suppliers.find(
        (s) => s.maNCC === parseInt(newItem.maNCC)
      );
      if (!selectedSupplier) {
        toast.error("Không tìm thấy nhà cung cấp");
        return;
      }

      const itemToSend = {
        tenNguyenLieu: newItem.tenNguyenLieu.trim(),
        donVi: newItem.donVi.trim(),
        soLuongHienTai: parseFloat(newItem.soLuongHienTai),
        soLuongToiThieu: parseFloat(newItem.soLuongToiThieu),
        maNCC: selectedSupplier.maNCC,
        trangThai: "Active",
        ngayNhap: new Date().toISOString(),
      };

      // Log dữ liệu gửi đi
      console.log("Data being sent:", itemToSend);

      // Kiểm tra giá trị số
      if (isNaN(itemToSend.soLuongHienTai) || itemToSend.soLuongHienTai < 0) {
        toast.error("Số lượng hiện tại không hợp lệ");
        return;
      }
      if (isNaN(itemToSend.soLuongToiThieu) || itemToSend.soLuongToiThieu < 0) {
        toast.error("Số lượng tối thiểu không hợp lệ");
        return;
      }
      if (isNaN(itemToSend.maNCC) || itemToSend.maNCC <= 0) {
        toast.error("Vui lòng chọn nhà cung cấp");
        return;
      }

      // Kiểm tra định dạng ngày
      if (
        !itemToSend.ngayNhap ||
        isNaN(new Date(itemToSend.ngayNhap).getTime())
      ) {
        toast.error("Ngày nhập không hợp lệ");
        return;
      }

      const response = await axios.post(
        "http://localhost:5078/api/Kho",
        itemToSend,
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );
      if (response.status === 201) {
        toast.success("Thêm nguyên liệu thành công");
        await fetchStorage();
        setShowAddModal(false);
        setNewItem({
          tenNguyenLieu: "",
          donVi: "",
          soLuongHienTai: 0,
          soLuongToiThieu: 0,
          maNCC: "",
          trangThai: "Active",
        });
      }
    } catch (error) {
      console.error("Full error object:", error);
      if (error.response) {
        // Log chi tiết lỗi từ server
        console.error("Error response data:", error.response.data);
        console.error("Error response status:", error.response.status);
        console.error("Error response headers:", error.response.headers);

        // Hiển thị thông báo lỗi chi tiết hơn
        const errorMessage =
          typeof error.response.data === "string"
            ? error.response.data
            : JSON.stringify(error.response.data);
        toast.error(`Lỗi server: ${errorMessage}`);
      } else if (error.request) {
        console.error("Error request:", error.request);
        toast.error("Không thể kết nối đến server");
      } else {
        console.error("Error message:", error.message);
        toast.error(`Lỗi: ${error.message}`);
      }
    }
  };

  // Edit item
  const handleEditItem = async (e) => {
    e.preventDefault();
    try {
      // Validate required fields
      if (!selectedItem.tenNguyenLieu?.trim()) {
        toast.error("Vui lòng nhập tên nguyên liệu");
        return;
      }
      if (!selectedItem.donVi?.trim()) {
        toast.error("Vui lòng nhập đơn vị");
        return;
      }
      if (selectedItem.soLuongHienTai < 0) {
        toast.error("Số lượng hiện tại không hợp lệ");
        return;
      }
      if (selectedItem.soLuongToiThieu < 0) {
        toast.error("Số lượng tối thiểu không hợp lệ");
        return;
      }

      // Convert data types before sending
      const payload = {
        maNguyenLieu: parseInt(selectedItem.maNguyenLieu),
        tenNguyenLieu: selectedItem.tenNguyenLieu.trim(),
        donVi: selectedItem.donVi.trim(),
        soLuongHienTai: parseFloat(selectedItem.soLuongHienTai),
        soLuongToiThieu: parseFloat(selectedItem.soLuongToiThieu),
        maNCC: parseInt(selectedItem.maNCC),
        trangThai: selectedItem.trangThai || "Active",
      };

      console.log("Sending payload:", payload); // Debug log

      const response = await axios.put(
        `http://localhost:5078/api/Kho/${selectedItem.maNguyenLieu}`,
        payload
      );

      if (response.status === 200) {
        toast.success("Cập nhật nguyên liệu thành công");
        await fetchStorage(); // Refresh the list
        setShowEditModal(false);
      }
    } catch (error) {
      console.error("Edit error:", error);
      if (error.response) {
        console.log("Error response data:", error.response.data); // Debug log
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
      if (!selectedItem?.maNguyenLieu) {
        toast.error("Không tìm thấy mã nguyên liệu");
        return;
      }

      console.log("Deleting item:", selectedItem); // Debug log

      const response = await axios.delete(
        `http://localhost:5078/api/Kho/${parseInt(selectedItem.maNguyenLieu)}`
      );

      if (response.status === 204) {
        toast.success("Xóa nguyên liệu thành công");
        await fetchStorage(); // Refresh the list
        setShowDeleteModal(false);
        setSelectedItem(null); // Reset selected item
      }
    } catch (error) {
      console.error("Delete error:", error);
      if (error.response) {
        console.log("Error response data:", error.response.data); // Debug log
        toast.error(`Lỗi: ${error.response.data}`);
      } else if (error.request) {
        toast.error("Không thể kết nối đến server");
      } else {
        toast.error(`Lỗi: ${error.message}`);
      }
    }
  };

  return (
    <div className="storage-container">
      <div className="storage-header">
        <div className="header-left">
          <h2>Quản lý kho</h2>
          <span className="total-count">{storage.length} nguyên liệu</span>
        </div>
        <div className="header-right">
          <div className="search-box">
            <i className="fas fa-search search-icon"></i>
            <input
              type="text"
              placeholder="Nhập tên nguyên liệu để tìm kiếm..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              aria-label="Tìm kiếm nguyên liệu"
            />
            {searchTerm && (
              <button
                className="clear-search"
                onClick={() => setSearchTerm("")}
                title="Xóa tìm kiếm"
                aria-label="Xóa tìm kiếm"
              >
                <i className="fas fa-times"></i>
              </button>
            )}
          </div>
          <button
            className="transaction-button"
            onClick={() => setShowTransactionModal(true)}
          >
            <i className="fas fa-exchange-alt"></i> Giao dịch kho
          </button>
          <button className="add-button" onClick={() => setShowAddModal(true)}>
            <i className="fas fa-plus"></i> Thêm nguyên liệu
          </button>
        </div>
      </div>

      <div className="storage-table">
        <table>
          <thead>
            <tr>
              <th onClick={() => handleSort("tenNguyenLieu")}>
                Tên nguyên liệu
              </th>
              <th onClick={() => handleSort("soLuongHienTai")}>Số lượng</th>
              <th onClick={() => handleSort("donVi")}>Đơn vị</th>
              <th onClick={() => handleSort("soLuongToiThieu")}>
                Số lượng tối thiểu
              </th>
              <th onClick={() => handleSort("nhaCungCap.tenNCC")}>
                Nhà cung cấp
              </th>
              <th>Thao tác</th>
            </tr>
          </thead>
          <tbody>
            {currentItems.map((item) => (
              <tr key={item.maNguyenLieu}>
                <td>{item.tenNguyenLieu}</td>
                <td
                  className={
                    item.soLuongHienTai < item.soLuongToiThieu
                      ? "low-stock"
                      : ""
                  }
                >
                  {item.soLuongHienTai} {item.donVi}
                </td>
                <td>{item.donVi}</td>
                <td>{item.soLuongToiThieu}</td>
                <td>{item.nhaCungCap?.tenNCC || "N/A"}</td>
                <td>
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

      {/* Giữ nguyên các modal hiện có và thêm modal giao dịch kho */}
      {showTransactionModal && (
        <div className="modal">
          <div className="modal-content">
            <h3>Giao dịch kho</h3>
            <form onSubmit={handleTransaction}>
              <div className="form-group">
                <label>Loại giao dịch:</label>
                <select
                  name="loaiGiaoDich"
                  value={newTransaction.loaiGiaoDich}
                  onChange={(e) =>
                    setNewTransaction({
                      ...newTransaction,
                      loaiGiaoDich: e.target.value,
                    })
                  }
                >
                  <option value="nhap">Nhập kho</option>
                  <option value="xuat">Xuất kho</option>
                </select>
              </div>
              <div className="form-group">
                <label>Nguyên liệu:</label>
                <select
                  name="maNguyenLieu"
                  value={newTransaction.maNguyenLieu}
                  onChange={(e) =>
                    setNewTransaction({
                      ...newTransaction,
                      maNguyenLieu: e.target.value,
                    })
                  }
                >
                  <option value="">Chọn nguyên liệu</option>
                  {storage.map((item) => (
                    <option key={item.maNguyenLieu} value={item.maNguyenLieu}>
                      {item.tenNguyenLieu}
                    </option>
                  ))}
                </select>
              </div>
              <div className="form-group">
                <label>Số lượng:</label>
                <input
                  type="number"
                  name="soLuong"
                  value={newTransaction.soLuong}
                  onChange={(e) =>
                    setNewTransaction({
                      ...newTransaction,
                      soLuong: parseFloat(e.target.value),
                    })
                  }
                  required
                />
              </div>
              <div className="form-group">
                <label>Ghi chú:</label>
                <textarea
                  name="ghiChu"
                  value={newTransaction.ghiChu}
                  onChange={(e) =>
                    setNewTransaction({
                      ...newTransaction,
                      ghiChu: e.target.value,
                    })
                  }
                ></textarea>
              </div>
              <div className="modal-actions">
                <button type="submit" className="save-button">
                  Thực hiện
                </button>
                <button
                  type="button"
                  className="cancel-button"
                  onClick={() => setShowTransactionModal(false)}
                >
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
                <label>Số lượng hiện tại:</label>
                <input
                  type="number"
                  name="soLuongHienTai"
                  value={selectedItem.soLuongHienTai}
                  onChange={handleInputChange}
                  min="0"
                  required
                />
              </div>
              <div className="form-group">
                <label>Số lượng tối thiểu:</label>
                <input
                  type="number"
                  name="soLuongToiThieu"
                  value={selectedItem.soLuongToiThieu}
                  onChange={handleInputChange}
                  min="0"
                  required
                />
              </div>
              <div className="form-group">
                <label>Nhà cung cấp:</label>
                <select
                  name="maNCC"
                  value={selectedItem.maNCC}
                  onChange={handleInputChange}
                  required
                >
                  {suppliers.map((supplier) => (
                    <option key={supplier.maNCC} value={supplier.maNCC}>
                      {supplier.tenNCC}
                    </option>
                  ))}
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

      {/* Delete Modal */}
      {showDeleteModal && selectedItem && (
        <div className="modal">
          <div className="modal-content">
            <h3>Xác nhận xóa</h3>
            <p>
              Bạn có chắc chắn muốn xóa nguyên liệu "
              <strong>{selectedItem.tenNguyenLieu}</strong>"?
            </p>
            <p className="warning-text">
              Lưu ý: Hành động này không thể hoàn tác!
            </p>
            <div className="modal-actions">
              <button
                type="button"
                className="cancel-button"
                onClick={handleCloseModal}
              >
                Hủy
              </button>
              <button
                type="button"
                className="delete-button"
                onClick={handleDeleteItem}
              >
                Xóa
              </button>
            </div>
          </div>
        </div>
      )}

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
                <label>Số lượng hiện tại:</label>
                <input
                  type="number"
                  name="soLuongHienTai"
                  value={newItem.soLuongHienTai}
                  onChange={handleInputChange}
                  min="0"
                  required
                />
              </div>
              <div className="form-group">
                <label>Số lượng tối thiểu:</label>
                <input
                  type="number"
                  name="soLuongToiThieu"
                  value={newItem.soLuongToiThieu}
                  onChange={handleInputChange}
                  min="0"
                  required
                />
              </div>
              <div className="form-group">
                <label>Nhà cung cấp:</label>
                <select
                  name="maNCC"
                  value={newItem.maNCC}
                  onChange={handleInputChange}
                  required
                >
                  <option value="">Chọn nhà cung cấp</option>
                  {suppliers.map((supplier) => (
                    <option key={supplier.maNCC} value={supplier.maNCC}>
                      {supplier.tenNCC}
                    </option>
                  ))}
                </select>
                {suppliers.length === 0 && (
                  <span className="error-message">
                    Không có nhà cung cấp đang hoạt động
                  </span>
                )}
              </div>
              <div className="modal-actions">
                <button type="submit" className="save-button">
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
    </div>
  );
};

export default Storage;
