import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { toast } from 'react-toastify';
import './InventoryTransaction.css';

const InventoryTransaction = () => {
  const [transactions, setTransactions] = useState([]);
  const [inventory, setInventory] = useState([]);
  const [showAddModal, setShowAddModal] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage] = useState(10);
  const [searchTerm, setSearchTerm] = useState('');
  const [newTransaction, setNewTransaction] = useState({
    maNguyenLieu: '',
    loaiPhieu: 'Nhap', // 'Nhap' hoặc 'Xuat'
    soLuong: '',
    ngayNhapXuat: new Date().toISOString().split('T')[0],
    ghiChu: ''
  });

  // Fetch transactions
  const fetchTransactions = async () => {
    try {
      const response = await axios.get('http://localhost:5078/api/NhapXuatKho');
      setTransactions(response.data);
    } catch (error) {
      toast.error('Lỗi khi tải dữ liệu nhập xuất kho');
    }
  };

  // Fetch inventory items
  const fetchInventory = async () => {
    try {
      const response = await axios.get('http://localhost:5078/api/NguyenLieu');
      setInventory(response.data);
    } catch (error) {
      toast.error('Lỗi khi tải danh sách nguyên liệu');
    }
  };

  useEffect(() => {
    fetchTransactions();
    fetchInventory();
  }, []);

  // Handle add transaction
  const handleAdd = async () => {
    try {
      await axios.post('http://localhost:5078/api/NhapXuatKho', newTransaction);
      toast.success('Thêm phiếu nhập xuất thành công');
      fetchTransactions();
      setShowAddModal(false);
      setNewTransaction({
        maNguyenLieu: '',
        loaiPhieu: 'Nhap',
        soLuong: '',
        ngayNhapXuat: new Date().toISOString().split('T')[0],
        ghiChu: ''
      });
    } catch (error) {
      toast.error('Lỗi khi thêm phiếu nhập xuất');
    }
  };

  // Filter transactions
  const filteredTransactions = transactions.filter(transaction =>
    transaction.nguyenLieu?.tenNguyenLieu.toLowerCase().includes(searchTerm.toLowerCase()) ||
    transaction.loaiPhieu.toLowerCase().includes(searchTerm.toLowerCase())
  );

  // Pagination
  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentItems = filteredTransactions.slice(indexOfFirstItem, indexOfLastItem);
  const totalPages = Math.ceil(filteredTransactions.length / itemsPerPage);

  return (
    <div className="inventory-transaction">
      <div className="inventory-transaction-header">
        <h2>Quản lý nhập xuất kho</h2>
        <div className="inventory-transaction-actions">
          <input
            type="text"
            placeholder="Tìm kiếm..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="search-input"
          />
          <button className="add-button" onClick={() => setShowAddModal(true)}>
            Thêm phiếu nhập/xuất
          </button>
        </div>
      </div>

      <table className="inventory-transaction-table">
        <thead>
          <tr>
            <th>Mã phiếu</th>
            <th>Loại phiếu</th>
            <th>Nguyên liệu</th>
            <th>Số lượng</th>
            <th>Ngày nhập/xuất</th>
            <th>Ghi chú</th>
          </tr>
        </thead>
        <tbody>
          {currentItems.map((transaction) => (
            <tr key={transaction.maPhieu}>
              <td>{transaction.maPhieu}</td>
              <td className={transaction.loaiPhieu === 'Nhap' ? 'import' : 'export'}>
                {transaction.loaiPhieu === 'Nhap' ? 'Nhập kho' : 'Xuất kho'}
              </td>
              <td>{transaction.nguyenLieu?.tenNguyenLieu}</td>
              <td>{transaction.soLuong}</td>
              <td>{new Date(transaction.ngayNhapXuat).toLocaleDateString('vi-VN')}</td>
              <td>{transaction.ghiChu}</td>
            </tr>
          ))}
        </tbody>
      </table>

      <div className="pagination">
        <button
          onClick={() => setCurrentPage(currentPage - 1)}
          disabled={currentPage === 1}
        >
          Trước
        </button>
        <span>{currentPage}</span>
        <button
          onClick={() => setCurrentPage(currentPage + 1)}
          disabled={currentPage === totalPages}
        >
          Sau
        </button>
      </div>

      {/* Add Modal */}
      {showAddModal && (
        <div className="modal">
          <div className="modal-content">
            <h3>Thêm phiếu nhập/xuất</h3>
            <select
              value={newTransaction.maNguyenLieu}
              onChange={(e) =>
                setNewTransaction({
                  ...newTransaction,
                  maNguyenLieu: e.target.value
                })
              }
            >
              <option value="">Chọn nguyên liệu</option>
              {inventory.map((item) => (
                <option key={item.maNguyenLieu} value={item.maNguyenLieu}>
                  {item.tenNguyenLieu}
                </option>
              ))}
            </select>
            <select
              value={newTransaction.loaiPhieu}
              onChange={(e) =>
                setNewTransaction({
                  ...newTransaction,
                  loaiPhieu: e.target.value
                })
              }
            >
              <option value="Nhap">Nhập kho</option>
              <option value="Xuat">Xuất kho</option>
            </select>
            <input
              type="number"
              placeholder="Số lượng"
              value={newTransaction.soLuong}
              onChange={(e) =>
                setNewTransaction({
                  ...newTransaction,
                  soLuong: e.target.value
                })
              }
            />
            <input
              type="date"
              value={newTransaction.ngayNhapXuat}
              onChange={(e) =>
                setNewTransaction({
                  ...newTransaction,
                  ngayNhapXuat: e.target.value
                })
              }
            />
            <textarea
              placeholder="Ghi chú"
              value={newTransaction.ghiChu}
              onChange={(e) =>
                setNewTransaction({
                  ...newTransaction,
                  ghiChu: e.target.value
                })
              }
            />
            <div className="modal-actions">
              <button onClick={handleAdd}>Thêm</button>
              <button onClick={() => setShowAddModal(false)}>Hủy</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default InventoryTransaction; 