import React, { useState, useEffect } from "react";
import "./Tables.css";
import axios from "axios";
import { toast } from "react-toastify";

const Tables = () => {
  const [tables, setTables] = useState([]);
  const [showAddModal, setShowAddModal] = useState(false);
  const [showEditModal, setShowEditModal] = useState(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [selectedTable, setSelectedTable] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage] = useState(10);
  const [searchTerm, setSearchTerm] = useState("");
  const [newTable, setNewTable] = useState({
    tenBan: "",
    sucChua: "",
    trangThai: false, // Đặt mặc định là false (Trống)
  });

  // Fetch tables
  const fetchTables = async () => {
    try {
      const response = await axios.get("http://localhost:5078/api/Ban");
      setTables(response.data);
    } catch (error) {
      toast.error("Lỗi khi tải danh sách bàn");
    }
  };

  useEffect(() => {
    fetchTables();
  }, []);

  // Add new table
  const handleAdd = async () => {
    try {
      await axios.post("http://localhost:5078/api/Ban", newTable);
      toast.success("Thêm bàn thành công");
      fetchTables();
      setShowAddModal(false);
      setNewTable({ tenBan: "", sucChua: "", trangThai: false });
    } catch (error) {
      toast.error("Lỗi khi thêm bàn");
    }
  };

  // Edit table
  const handleEdit = async () => {
    try {
      await axios.put(
        `http://localhost:5078/api/Ban/${selectedTable.maBan}`,
        selectedTable
      );
      toast.success("Cập nhật bàn thành công");
      fetchTables();
      setShowEditModal(false);
    } catch (error) {
      toast.error("Lỗi khi cập nhật bàn");
    }
  };

  // Delete table
  const handleDelete = async () => {
    try {
      await axios.delete(
        `http://localhost:5078/api/Ban/${selectedTable.maBan}`
      );
      toast.success("Xóa bàn thành công");
      fetchTables(); // Tải lại danh sách bàn với mã bàn đã được cập nhật
      setShowDeleteModal(false);
    } catch (error) {
      toast.error("Lỗi khi xóa bàn");
    }
  };

  // Filter tables
  const filteredTables = tables.filter((table) =>
    table.tenBan.toLowerCase().includes(searchTerm.toLowerCase())
  );

  // Pagination
  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentItems = filteredTables.slice(indexOfFirstItem, indexOfLastItem);
  const totalPages = Math.ceil(filteredTables.length / itemsPerPage);

  return (
    <div className="tables-container">
      <div className="tables-header">
        <h2>Quản lý bàn ăn</h2>
        <div className="tables-actions">
          <input
            type="text"
            placeholder="Tìm kiếm..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="search-input"
          />
          <button className="add-button" onClick={() => setShowAddModal(true)}>
            Thêm bàn mới
          </button>
        </div>
      </div>

      <table className="tables-table">
        <thead>
          <tr>
            <th>Mã bàn</th>
            <th>Tên bàn</th>
            <th>Sức chứa</th>
            <th>Trạng thái</th>
            <th>Thao tác</th>
          </tr>
        </thead>
        <tbody>
          {currentItems.map((table) => (
            <tr key={table.maBan}>
              <td>{table.maBan}</td>
              <td>{table.tenBan}</td>
              <td>{table.sucChua}</td>
              <td>{table.trangThai ? "Đã đặt" : "Trống"}</td>
              <td>
                <button
                  className="edit-button"
                  onClick={() => {
                    setSelectedTable(table);
                    setShowEditModal(true);
                  }}
                >
                  Sửa
                </button>
                <button
                  className="delete-button"
                  onClick={() => {
                    setSelectedTable(table);
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

      <div className="pagination">
        <button
          onClick={() => setCurrentPage(currentPage - 1)}
          disabled={currentPage === 1}
          style={{ color: "#2196f3" }} // Màu xanh
        >
          ←
        </button>
        <span>{currentPage}</span>
        <button
          onClick={() => setCurrentPage(currentPage + 1)}
          disabled={currentPage === totalPages}
          style={{ color: "#2196f3" }} // Màu xanh
        >
          →
        </button>
      </div>

      {/* Add Modal */}
      {showAddModal && (
        <div className="modal">
          <div className="modal-content">
            <h3>Thêm bàn mới</h3>
            <input
              type="text"
              placeholder="Tên bàn"
              value={newTable.tenBan}
              onChange={(e) =>
                setNewTable({ ...newTable, tenBan: e.target.value })
              }
            />
            <input
              type="number"
              placeholder="Sức chứa"
              value={newTable.sucChua}
              onChange={(e) =>
                setNewTable({ ...newTable, sucChua: e.target.value })
              }
            />
            <div className="modal-actions">
              <button onClick={handleAdd}>Thêm</button>
              <button onClick={() => setShowAddModal(false)}>Hủy</button>
            </div>
          </div>
        </div>
      )}

      {/* Edit Modal */}
      {showEditModal && (
        <div className="modal">
          <div className="modal-content">
            <h3>Sửa thông tin bàn</h3>
            <input
              type="text"
              placeholder="Tên bàn"
              value={selectedTable.tenBan}
              onChange={(e) =>
                setSelectedTable({ ...selectedTable, tenBan: e.target.value })
              }
            />
            <input
              type="number"
              placeholder="Sức chứa"
              value={selectedTable.sucChua}
              onChange={(e) =>
                setSelectedTable({ ...selectedTable, sucChua: e.target.value })
              }
            />
            <div className="modal-actions">
              <button onClick={handleEdit}>Lưu</button>
              <button onClick={() => setShowEditModal(false)}>Hủy</button>
            </div>
          </div>
        </div>
      )}

      {/* Delete Modal */}
      {showDeleteModal && (
        <div className="modal">
          <div className="modal-content">
            <h3>Xác nhận xóa</h3>
            <p>Bạn có chắc chắn muốn xóa bàn này?</p>
            <div className="modal-actions">
              <button onClick={handleDelete}>Xóa</button>
              <button onClick={() => setShowDeleteModal(false)}>Hủy</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default Tables;
