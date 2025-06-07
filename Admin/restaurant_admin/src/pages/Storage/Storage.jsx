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
  const [showHistoryModal, setShowHistoryModal] = useState(false);
  const [transactionHistory, setTransactionHistory] = useState([]);
  const [historySearchTerm, setHistorySearchTerm] = useState("");
  const [historyCurrentPage, setHistoryCurrentPage] = useState(1);
  const [historyItemsPerPage] = useState(10);
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
    loaiGiaoDich: "nhap",
    soLuong: 0,
    ghiChu: "",
  });

  const [showInventoryModal, setShowInventoryModal] = useState(false);
  const [inventoryItems, setInventoryItems] = useState([]);
  const [inventoryDate, setInventoryDate] = useState(new Date().toISOString().split('T')[0]);
  const [inventoryNote, setInventoryNote] = useState("");
  const [showInventoryHistoryModal, setShowInventoryHistoryModal] = useState(false);
  const [inventoryHistory, setInventoryHistory] = useState([]);
  const [inventoryHistorySearchTerm, setInventoryHistorySearchTerm] = useState("");
  const [inventoryHistoryCurrentPage, setInventoryHistoryCurrentPage] = useState(1);
  const [inventoryHistoryItemsPerPage] = useState(10);
  const [showInventoryDetailsModal, setShowInventoryDetailsModal] = useState(false);
  const [selectedInventoryDetails, setSelectedInventoryDetails] = useState(null);

  // Lọc và sắp xếp dữ liệu
  const filteredItems = storage.filter((item) =>
    item.tenNguyenLieu.toLowerCase().includes(searchTerm.toLowerCase().trim())
  );

  const sortedItems = sortField
    ? [...filteredItems].sort((a, b) => {
        if (sortDirection === "asc") {
          // Handle potential null or undefined values gracefully
          const aValue = a[sortField] ?? '';
          const bValue = b[sortField] ?? '';

          // Custom sort for nested supplier name
          if (sortField === 'nhaCungCap.tenNCC') {
            const aSupplierName = a.nhaCungCap?.tenNCC ?? '';
            const bSupplierName = b.nhaCungCap?.tenNCC ?? '';
            return aSupplierName.localeCompare(bSupplierName);
          }

          // Handle numeric fields
          if (typeof aValue === 'number' && typeof bValue === 'number') {
            return aValue - bValue;
          }

          // Default string comparison for others
          return aValue.toString().localeCompare(bValue.toString());
        } else {
          // Handle potential null or undefined values gracefully
          const aValue = a[sortField] ?? '';
          const bValue = b[sortField] ?? '';

          // Custom sort for nested supplier name
          if (sortField === 'nhaCungCap.tenNCC') {
            const aSupplierName = a.nhaCungCap?.tenNCC ?? '';
            const bSupplierName = b.nhaCungCap?.tenNCC ?? '';
            return bSupplierName.localeCompare(aSupplierName);
          }

          // Handle numeric fields
          if (typeof aValue === 'number' && typeof bValue === 'number') {
            return bValue - aValue;
          }

          // Default string comparison for others
          return bValue.toString().localeCompare(aValue.toString());
        }
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
      // Validate dữ liệu
      if (!newTransaction.maNguyenLieu) {
        toast.error("Vui lòng chọn nguyên liệu");
        return;
      }
      if (newTransaction.soLuong <= 0) {
        toast.error("Số lượng phải lớn hơn 0");
        return;
      }

      const response = await axios.put(
        "http://localhost:5078/api/Kho/giaodich",
        newTransaction,
        {
          headers: {
            'Content-Type': 'application/json'
          }
        }
      );

      if (response.status === 200) {
        toast.success(response.data.message || "Giao dịch thành công");
        await fetchStorage(); // Refresh danh sách
        setShowTransactionModal(false);
        // Reset form
        setNewTransaction({
          maNguyenLieu: "",
          loaiGiaoDich: "nhap",
          soLuong: 0,
          ghiChu: "",
        });
      }
    } catch (error) {
      console.error("Transaction error:", error);
      if (error.response) {
        const errorMessage = typeof error.response.data === 'object' 
          ? JSON.stringify(error.response.data)
          : error.response.data;
        toast.error(`Lỗi server: ${errorMessage}`);
      } else if (error.request) {
        toast.error("Không thể kết nối đến server");
      } else {
        toast.error(`Lỗi: ${error.message}`);
      }
    }
  };

  // Reset form giao dịch
  const handleCloseTransactionModal = () => {
    setShowTransactionModal(false);
    setNewTransaction({
      maNguyenLieu: "",
      loaiGiaoDich: "nhap",
      soLuong: 0,
      ghiChu: "",
    });
  };

  // Kiểm kê kho
  const handleInventoryCheck = async () => {
    try {
      const response = await axios.get("http://localhost:5078/api/KiemKeKho/kiemke");
      if (response.status === 200) {
        const items = response.data.map(item => ({
          ...item,
          actualQuantity: item.soLuongHienTai,
          difference: 0,
          note: ""
        }));
        setInventoryItems(items);
        setShowInventoryModal(true);
      }
    } catch (error) {
      console.error("Inventory check error:", error);
      if (error.response) {
        const errorMessage = typeof error.response.data === 'object' 
          ? JSON.stringify(error.response.data)
          : error.response.data;
        toast.error(`Lỗi server: ${errorMessage}`);
      } else if (error.request) {
        toast.error("Không thể kết nối đến server");
      } else {
        toast.error(`Lỗi: ${error.message}`);
      }
    }
  };

  // Save inventory check results
  const handleSaveInventoryCheck = async () => {
    try {
      // Tạo ngày giờ UTC
      const now = new Date();
      const utcDate = new Date(Date.UTC(
        now.getUTCFullYear(),
        now.getUTCMonth(),
        now.getUTCDate(),
        now.getUTCHours(),
        now.getUTCMinutes(),
        now.getUTCSeconds()
      ));

      const inventoryData = {
        ngayKiemKe: utcDate.toISOString(), // Gửi dạng UTC
        nguoiKiemKe: "Admin", // TODO: Lấy từ thông tin người dùng đang đăng nhập
        ghiChu: inventoryNote,
        chiTietKiemKe: inventoryItems.map(item => ({
          maNguyenLieu: item.maNguyenLieu,
          soLuongThucTe: parseFloat(item.actualQuantity) || 0,
          chenhLech: parseFloat(item.difference) || 0,
          ghiChu: item.note || ""
        }))
      };

      console.log("Sending inventory data:", inventoryData); // Log dữ liệu gửi đi

      const response = await axios.post(
        "http://localhost:5078/api/KiemKeKho/luu",
        inventoryData,
        {
          headers: {
            'Content-Type': 'application/json'
          }
        }
      );

      if (response.status === 200) {
        toast.success("Lưu kết quả kiểm kê thành công");
        setShowInventoryModal(false);
        await fetchStorage(); // Refresh storage data
      }
    } catch (error) {
      console.error("Save inventory error:", error);
      if (error.response) {
        const errorMessage = typeof error.response.data === 'object' 
          ? JSON.stringify(error.response.data)
          : error.response.data;
        toast.error(`Lỗi server: ${errorMessage}`);
      } else if (error.request) {
        toast.error("Không thể kết nối đến server");
      } else {
        toast.error(`Lỗi: ${error.message}`);
      }
    }
  };

  // Update inventory item quantity
  const handleInventoryItemChange = (maNguyenLieu, field, value) => {
    setInventoryItems(prevItems => 
      prevItems.map(item => {
        if (item.maNguyenLieu === maNguyenLieu) {
          const updatedItem = { ...item, [field]: value };
          if (field === 'actualQuantity') {
            updatedItem.difference = value - item.soLuongHienTai;
          }
          return updatedItem;
        }
        return item;
      })
    );
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
        const errorMessage = typeof error.response.data === 'object' 
          ? JSON.stringify(error.response.data)
          : error.response.data;
        toast.error(`Lỗi server: ${errorMessage}`);
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

  // Fetch transaction history
  const fetchTransactionHistory = async () => {
    try {
      console.log("Fetching transaction history...");
      const response = await axios.get("http://localhost:5078/api/Kho/lichsu");
      console.log("Transaction history response:", response.data);
      if (response.data) {
        setTransactionHistory(response.data);
      }
    } catch (error) {
      console.error("Error fetching transaction history:", error);
      if (error.response) {
        const errorMessage = typeof error.response.data === 'object' 
          ? JSON.stringify(error.response.data)
          : error.response.data;
        toast.error(`Lỗi server: ${errorMessage}`);
      } else if (error.request) {
        toast.error("Không thể kết nối đến server");
      } else {
        toast.error(`Lỗi: ${error.message}`);
      }
    }
  };

  // Filter transaction history
  const filteredHistory = transactionHistory.filter((item) =>
    item.kho?.tenNguyenLieu?.toLowerCase().includes(historySearchTerm.toLowerCase().trim()) ||
    item.loai?.toLowerCase().includes(historySearchTerm.toLowerCase().trim())
  );

  // Paginate transaction history
  const indexOfLastHistoryItem = historyCurrentPage * historyItemsPerPage;
  const indexOfFirstHistoryItem = indexOfLastHistoryItem - historyItemsPerPage;
  const currentHistoryItems = filteredHistory.slice(indexOfFirstHistoryItem, indexOfLastHistoryItem);
  const totalHistoryPages = Math.ceil(filteredHistory.length / historyItemsPerPage) || 1;

  // Reset history page when search term changes
  useEffect(() => {
    setHistoryCurrentPage(1);
  }, [historySearchTerm]);

  // Format date
  const formatDate = (dateString) => {
    if (!dateString) return 'Không xác định';
    try {
      const date = new Date(dateString);
      return date.toLocaleString('vi-VN', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit'
      });
    } catch (error) {
      console.error('Error formatting date:', error);
      return 'Định dạng không hợp lệ';
    }
  };

  // Fetch inventory history
  const fetchInventoryHistory = async () => {
    try {
      const response = await axios.get("http://localhost:5078/api/KiemKeKho/lichsu");
      if (response.data) {
        setInventoryHistory(response.data);
      }
    } catch (error) {
      console.error("Error fetching inventory history:", error);
      if (error.response) {
        const errorMessage = typeof error.response.data === 'object' 
          ? JSON.stringify(error.response.data)
          : error.response.data;
        toast.error(`Lỗi server: ${errorMessage}`);
      } else if (error.request) {
        toast.error("Không thể kết nối đến server");
      } else {
        toast.error(`Lỗi: ${error.message}`);
      }
    }
  };

  // Filter inventory history
  const filteredInventoryHistory = inventoryHistory.filter((item) =>
    item.nguoiKiemKe?.toLowerCase().includes(inventoryHistorySearchTerm.toLowerCase().trim()) ||
    item.chiTietKiemKe?.some(ct => 
      ct.tenNguyenLieu?.toLowerCase().includes(inventoryHistorySearchTerm.toLowerCase().trim())
    )
  );

  // Paginate inventory history
  const indexOfLastInventoryHistoryItem = inventoryHistoryCurrentPage * inventoryHistoryItemsPerPage;
  const indexOfFirstInventoryHistoryItem = indexOfLastInventoryHistoryItem - inventoryHistoryItemsPerPage;
  const currentInventoryHistoryItems = filteredInventoryHistory.slice(
    indexOfFirstInventoryHistoryItem,
    indexOfLastInventoryHistoryItem
  );
  const totalInventoryHistoryPages = Math.ceil(filteredInventoryHistory.length / inventoryHistoryItemsPerPage) || 1;

  // Reset inventory history page when search term changes
  useEffect(() => {
    setInventoryHistoryCurrentPage(1);
  }, [inventoryHistorySearchTerm]);

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
        const errorMessage = typeof error.response.data === 'object' 
          ? JSON.stringify(error.response.data)
          : error.response.data;
        toast.error(`Lỗi server: ${errorMessage}`);
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

      const payload = {
        ...selectedItem,
        trangThai: "Inactive"
      };

      const response = await axios.put(
        `http://localhost:5078/api/Kho/${parseInt(selectedItem.maNguyenLieu)}`,
        payload
      );

      if (response.status === 200) {
        toast.success("Đã chuyển trạng thái nguyên liệu thành ngừng hoạt động");
        await fetchStorage();
        setShowDeleteModal(false);
        setSelectedItem(null);
      }
    } catch (error) {
      console.error("Delete error:", error);
      if (error.response) {
        const errorMessage = typeof error.response.data === 'object' 
          ? JSON.stringify(error.response.data)
          : error.response.data;
        toast.error(`Lỗi server: ${errorMessage}`);
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
        trangThai: selectedItem.trangThai
      };

      console.log("Sending payload:", payload); // Debug log

      const response = await axios.put(
        `http://localhost:5078/api/Kho/${selectedItem.maNguyenLieu}`,
        payload
      );

      if (response.status === 200) {
        toast.success("Cập nhật nguyên liệu thành công");
        await fetchStorage();
        setShowEditModal(false);
        setSelectedItem(null);
      }
    } catch (error) {
      console.error("Edit error:", error);
      if (error.response) {
        const errorMessage = typeof error.response.data === 'object' 
          ? JSON.stringify(error.response.data)
          : error.response.data;
        toast.error(`Lỗi server: ${errorMessage}`);
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
            className="inventory-button"
            onClick={handleInventoryCheck}
          >
            <i className="fas fa-clipboard-check"></i> Kiểm kê kho
          </button>
          <button
            className="inventory-history-button"
            onClick={() => {
              setShowInventoryHistoryModal(true);
              fetchInventoryHistory();
            }}
          >
            <i className="fas fa-history"></i> Lịch sử kiểm kê
          </button>
          <button
            className="history-button"
            onClick={() => {
              setShowHistoryModal(true);
              fetchTransactionHistory();
            }}
          >
            <i className="fas fa-history"></i> Lịch sử giao dịch
          </button>
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
              <th onClick={() => handleSort("trangThai")}>Trạng thái</th>
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
                  <span
                    className={`status-badge ${
                      item.trangThai === "Active" ? "status-active" : "status-inactive"
                    }`}
                  >
                    {item.trangThai === "Active" ? "Đang hoạt động" : "Ngừng hoạt động"}
                  </span>
                </td>
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

      {/* Transaction Modal */}
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
                  required
                >
                  <option value="">Chọn nguyên liệu</option>
                  {storage
                    .filter(item => item.trangThai === "Active")
                    .map((item) => (
                    <option key={item.maNguyenLieu} value={item.maNguyenLieu}>
                        {item.tenNguyenLieu} (Hiện có: {item.soLuongHienTai} {item.donVi})
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
                      soLuong: parseFloat(e.target.value) || 0,
                    })
                  }
                  min="0.01"
                  step="0.01"
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
                  onClick={handleCloseTransactionModal}
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
              <div className="form-group">
                <label>Trạng thái:</label>
                <select
                  name="trangThai"
                  value={selectedItem.trangThai}
                  onChange={handleInputChange}
                  required
                >
                  <option value="Active">Đang hoạt động</option>
                  <option value="Inactive">Ngừng hoạt động</option>
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
              <div className="form-group">
                <label>Trạng thái:</label>
                <select
                  name="trangThai"
                  value={newItem.trangThai}
                  onChange={handleInputChange}
                  required
                >
                  <option value="Active">Đang hoạt động</option>
                  <option value="Inactive">Ngừng hoạt động</option>
                </select>
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

      {/* History Modal */}
      {showHistoryModal && (
        <div className="modal">
          <div className="modal-content history-modal">
            <h3>Lịch sử giao dịch kho</h3>
            <div className="search-box">
              <i className="fas fa-search search-icon"></i>
              <input
                type="text"
                placeholder="Tìm kiếm theo tên nguyên liệu..."
                value={historySearchTerm}
                onChange={(e) => setHistorySearchTerm(e.target.value)}
              />
              {historySearchTerm && (
                <button
                  className="clear-search"
                  onClick={() => setHistorySearchTerm("")}
                >
                  <i className="fas fa-times"></i>
                </button>
              )}
            </div>
            <div className="history-table">
              <table>
                <thead>
                  <tr>
                    <th>Thời gian</th>
                    <th>Nguyên liệu</th>
                    <th>Loại giao dịch</th>
                    <th>Số lượng</th>
                    <th>Ghi chú</th>
                  </tr>
                </thead>
                <tbody>
                  {currentHistoryItems.length > 0 ? (
                    currentHistoryItems.map((item) => (
                      <tr key={item.maGiaoDich}>
                        <td>{formatDate(item.ngayGio)}</td>
                        <td>{item.kho?.tenNguyenLieu || 'Không xác định'}</td>
                        <td>
                          <span className={`transaction-type ${item.loai.toLowerCase()}`}>
                            {item.loai.toLowerCase() === "nhap" ? "Nhập kho" : "Xuất kho"}
                          </span>
                        </td>
                        <td>{item.soLuong}</td>
                        <td>{item.lyDo || "-"}</td>
                      </tr>
                    ))
                  ) : (
                    <tr>
                      <td colSpan="5" style={{ textAlign: 'center' }}>
                        Không có dữ liệu giao dịch
                      </td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
            <div className="pagination">
              <button
                onClick={() => setHistoryCurrentPage(historyCurrentPage - 1)}
                disabled={historyCurrentPage === 1}
              >
                <i className="fas fa-chevron-left"></i> Trước
              </button>
              <span>
                {historyCurrentPage} / {totalHistoryPages}
              </span>
              <button
                onClick={() => setHistoryCurrentPage(historyCurrentPage + 1)}
                disabled={historyCurrentPage === totalHistoryPages}
              >
                Sau <i className="fas fa-chevron-right"></i>
              </button>
            </div>
            <div className="modal-actions">
              <button
                className="close-button"
                onClick={() => setShowHistoryModal(false)}
              >
                Đóng
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Inventory Check Modal */}
      {showInventoryModal && (
        <div className="modal-overlay">
          <div className="modal-content inventory-modal">
            <h2>Kiểm Kê Kho</h2>
            <div className="inventory-header">
              <div className="inventory-date">
                <label>Ngày kiểm kê:</label>
                <input
                  type="text"
                  value={new Date().toLocaleString('vi-VN', {
                    year: 'numeric',
                    month: '2-digit',
                    day: '2-digit',
                    hour: '2-digit',
                    minute: '2-digit'
                  })}
                  disabled
                />
              </div>
              <div className="inventory-note">
                <label>Ghi chú:</label>
                <textarea
                  value={inventoryNote}
                  onChange={(e) => setInventoryNote(e.target.value)}
                  placeholder="Nhập ghi chú kiểm kê..."
                />
              </div>
            </div>
            <div className="inventory-table">
              <table>
                <thead>
                  <tr>
                    <th>Nguyên liệu</th>
                    <th>Số lượng hệ thống</th>
                    <th>Số lượng thực tế</th>
                    <th>Chênh lệch</th>
                    <th>Ghi chú</th>
                  </tr>
                </thead>
                <tbody>
                  {inventoryItems.map((item) => (
                    <tr key={item.maNguyenLieu}>
                      <td>{item.tenNguyenLieu}</td>
                      <td>{item.soLuongHienTai}</td>
                      <td>
                        <input
                          type="number"
                          value={item.actualQuantity}
                          onChange={(e) => handleInventoryItemChange(
                            item.maNguyenLieu,
                            'actualQuantity',
                            parseFloat(e.target.value) || 0
                          )}
                        />
                      </td>
                      <td className={item.difference !== 0 ? 'highlight' : ''}>
                        {item.difference}
                      </td>
                      <td>
                        <input
                          type="text"
                          value={item.note}
                          onChange={(e) => handleInventoryItemChange(
                            item.maNguyenLieu,
                            'note',
                            e.target.value
                          )}
                          placeholder="Ghi chú..."
                        />
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
            <div className="modal-actions">
              <button
                className="btn-save"
                onClick={handleSaveInventoryCheck}
              >
                Lưu kết quả
              </button>
              <button
                className="btn-cancel"
                onClick={() => setShowInventoryModal(false)}
              >
                Hủy
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Inventory History Modal */}
      {showInventoryHistoryModal && (
        <div className="modal-overlay">
          <div className="modal-content inventory-history-modal">
            <h2>Lịch Sử Kiểm Kê Kho</h2>
            <div className="search-box">
              <i className="fas fa-search search-icon"></i>
              <input
                type="text"
                placeholder="Tìm kiếm theo người kiểm kê hoặc tên nguyên liệu..."
                value={inventoryHistorySearchTerm}
                onChange={(e) => setInventoryHistorySearchTerm(e.target.value)}
              />
              {inventoryHistorySearchTerm && (
                <button
                  className="clear-search"
                  onClick={() => setInventoryHistorySearchTerm("")}
                >
                  <i className="fas fa-times"></i>
                </button>
              )}
            </div>
            <div className="inventory-history-table">
              <table>
                <thead>
                  <tr>
                    <th>Ngày kiểm kê</th>
                    <th>Người kiểm kê</th>
                    <th>Ghi chú</th>
                    <th>Chi tiết</th>
                  </tr>
                </thead>
                <tbody>
                  {currentInventoryHistoryItems.length > 0 ? (
                    currentInventoryHistoryItems.map((item) => (
                      <tr key={item.maKiemKe}>
                        <td>{formatDate(item.ngayKiemKe)}</td>
                        <td>{item.nguoiKiemKe}</td>
                        <td>{item.ghiChu || "-"}</td>
                        <td>
                          <button
                            className="view-details-button"
                            onClick={() => {
                              setSelectedInventoryDetails(item.chiTietKiemKe);
                              setShowInventoryDetailsModal(true);
                            }}
                          >
                            <i className="fas fa-eye"></i> Xem chi tiết
                          </button>
                        </td>
                      </tr>
                    ))
                  ) : (
                    <tr>
                      <td colSpan="4" style={{ textAlign: 'center' }}>
                        Không có dữ liệu kiểm kê
                      </td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
            <div className="pagination">
              <button
                onClick={() => setInventoryHistoryCurrentPage(inventoryHistoryCurrentPage - 1)}
                disabled={inventoryHistoryCurrentPage === 1}
              >
                <i className="fas fa-chevron-left"></i> Trước
              </button>
              <span>
                {inventoryHistoryCurrentPage} / {totalInventoryHistoryPages}
              </span>
              <button
                onClick={() => setInventoryHistoryCurrentPage(inventoryHistoryCurrentPage + 1)}
                disabled={inventoryHistoryCurrentPage === totalInventoryHistoryPages}
              >
                Sau <i className="fas fa-chevron-right"></i>
              </button>
            </div>
            <div className="modal-actions">
              <button
                className="close-button"
                onClick={() => setShowInventoryHistoryModal(false)}
              >
                Đóng
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Inventory Details Modal */}
      {showInventoryDetailsModal && selectedInventoryDetails && (
        <div className="modal-overlay">
          <div className="modal-content inventory-details-modal">
            <h2>Chi Tiết Kiểm Kê</h2>
            <div className="inventory-details-table">
              <table>
                <thead>
                  <tr>
                    <th>Nguyên liệu</th>
                    <th>Số lượng thực tế</th>
                    <th>Chênh lệch</th>
                  </tr>
                </thead>
                <tbody>
                  {selectedInventoryDetails.map((item, index) => (
                    <tr key={index}>
                      <td>{item.tenNguyenLieu}</td>
                      <td>{item.soLuongThucTe}</td>
                      <td className={item.chenhLech !== 0 ? 'highlight' : ''}>
                        {item.chenhLech}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
            <div className="modal-actions">
              <button
                className="close-button"
                onClick={() => {
                  setShowInventoryDetailsModal(false);
                  setSelectedInventoryDetails(null);
                }}
              >
                Đóng
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default Storage;
