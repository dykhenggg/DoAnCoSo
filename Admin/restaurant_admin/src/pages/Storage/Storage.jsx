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
    soLuong: 0,
    donVi: "",
    giaNhap: 0,
    nhaCungCap: "",
    dinhMuc: 0,
    ghiChu: "",
  });

  const [newTransaction, setNewTransaction] = useState({
    maNguyenLieu: "",
    loaiGiaoDich: "nhap", // "nhap" hoặc "xuat"
    soLuong: 0,
    ngayGiaoDich: new Date().toISOString().split("T")[0],
    ghiChu: "",
  });

  // Lọc và sắp xếp dữ liệu
  const filteredItems = storage.filter(
    (item) =>
      item.tenNguyenLieu.toLowerCase().includes(searchTerm.toLowerCase()) ||
      item.nhaCungCap.toLowerCase().includes(searchTerm.toLowerCase())
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
        <div className="header-right">
          <div className="search-box">
            <input
              type="text"
              placeholder="Tìm kiếm nguyên liệu..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
          <button
            className="transaction-button"
            onClick={() => setShowTransactionModal(true)}
          >
            <i className="fas fa-exchange-alt"></i> Giao dịch kho
          </button>
          <button className="inventory-button" onClick={handleInventoryCheck}>
            <i className="fas fa-clipboard-check"></i> Kiểm kê
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
              <th onClick={() => handleSort("soLuong")}>Số lượng</th>
              <th onClick={() => handleSort("donVi")}>Đơn vị</th>
              <th onClick={() => handleSort("dinhMuc")}>Định mức</th>
              <th onClick={() => handleSort("giaNhap")}>Giá nhập</th>
              <th onClick={() => handleSort("nhaCungCap")}>Nhà cung cấp</th>
              <th>Thao tác</th>
            </tr>
          </thead>
          <tbody>
            {currentItems.map((item) => (
              <tr key={item.maNguyenLieu}>
                <td>{item.tenNguyenLieu}</td>
                <td className={item.soLuong < item.dinhMuc ? "low-stock" : ""}>
                  {item.soLuong} {item.donVi}
                </td>
                <td>{item.donVi}</td>
                <td>{item.dinhMuc}</td>
                <td>{item.giaNhap.toLocaleString("vi-VN")} VNĐ</td>
                <td>{item.nhaCungCap}</td>
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
      {/* Delete Modal */}
      {showDeleteModal && selectedItem && (
        <div className="modal">
          <div className="modal-content">
            <h3>Xác nhận xóa</h3>
            <p>
              Bạn có chắc chắn muốn xóa nguyên liệu "
              {selectedItem.tenNguyenLieu}"?
            </p>
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
