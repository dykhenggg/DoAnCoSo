import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { toast } from 'react-toastify';
import './InventoryCheck.css';

const InventoryCheck = () => {
  const [inventory, setInventory] = useState([]);
  const [checkedItems, setCheckedItems] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage] = useState(10);

  // Fetch inventory data
  const fetchInventory = async () => {
    try {
      const response = await axios.get('http://localhost:5078/api/NguyenLieu');
      setInventory(response.data);
      // Initialize checked items with current quantities
      setCheckedItems(response.data.map(item => ({
        ...item,
        actualQuantity: item.soLuongTon
      })));
    } catch (error) {
      toast.error('Lỗi khi tải dữ liệu kho');
    }
  };

  useEffect(() => {
    fetchInventory();
  }, []);

  // Handle quantity change
  const handleQuantityChange = (id, value) => {
    setCheckedItems(prevItems =>
      prevItems.map(item =>
        item.maNguyenLieu === id
          ? { ...item, actualQuantity: value }
          : item
      )
    );
  };

  // Save inventory check results
  const handleSave = async () => {
    try {
      const updates = checkedItems.map(item => ({
        maNguyenLieu: item.maNguyenLieu,
        soLuongTon: item.actualQuantity
      }));

      await axios.put('http://localhost:5078/api/NguyenLieu/UpdateInventory', updates);
      toast.success('Cập nhật kiểm kho thành công');
      fetchInventory();
    } catch (error) {
      toast.error('Lỗi khi cập nhật kiểm kho');
    }
  };

  // Filter inventory items
  const filteredItems = checkedItems.filter(item =>
    item.tenNguyenLieu.toLowerCase().includes(searchTerm.toLowerCase())
  );

  // Pagination
  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentItems = filteredItems.slice(indexOfFirstItem, indexOfLastItem);
  const totalPages = Math.ceil(filteredItems.length / itemsPerPage);

  return (
    <div className="inventory-check">
      <div className="inventory-check-header">
        <h2>Kiểm Kho</h2>
        <div className="inventory-check-actions">
          <input
            type="text"
            placeholder="Tìm kiếm nguyên liệu..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="search-input"
          />
          <button className="save-button" onClick={handleSave}>
            Lưu kết quả kiểm kho
          </button>
        </div>
      </div>

      <table className="inventory-check-table">
        <thead>
          <tr>
            <th>Mã nguyên liệu</th>
            <th>Tên nguyên liệu</th>
            <th>Đơn vị tính</th>
            <th>Số lượng theo sổ</th>
            <th>Số lượng thực tế</th>
            <th>Chênh lệch</th>
          </tr>
        </thead>
        <tbody>
          {currentItems.map((item) => (
            <tr key={item.maNguyenLieu}>
              <td>{item.maNguyenLieu}</td>
              <td>{item.tenNguyenLieu}</td>
              <td>{item.donViTinh}</td>
              <td>{item.soLuongTon}</td>
              <td>
                <input
                  type="number"
                  value={item.actualQuantity}
                  onChange={(e) => handleQuantityChange(item.maNguyenLieu, e.target.value)}
                  min="0"
                  className="quantity-input"
                />
              </td>
              <td className={item.actualQuantity - item.soLuongTon < 0 ? 'negative' : 'positive'}>
                {item.actualQuantity - item.soLuongTon}
              </td>
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
    </div>
  );
};

export default InventoryCheck;