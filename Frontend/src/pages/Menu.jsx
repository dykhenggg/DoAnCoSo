import { useState, useEffect } from 'react';
import api from '../config/axios';

const Menu = () => {
  const [dishes, setDishes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedCategory, setSelectedCategory] = useState('all');
  const [searchTerm, setSearchTerm] = useState('');

  useEffect(() => {
    const fetchDishes = async () => {
      try {
        const response = await api.get('/thucdon');
        setDishes(response.data);
        setLoading(false);
      } catch (err) {
        setError('Không thể tải thực đơn. Vui lòng thử lại sau.');
        setLoading(false);
      }
    };

    fetchDishes();
  }, []);

  const filteredDishes = dishes.filter(dish => {
    const matchesCategory = selectedCategory === 'all' || dish.loai === selectedCategory;
    const matchesSearch = dish.tenMon.toLowerCase().includes(searchTerm.toLowerCase());
    return matchesCategory && matchesSearch;
  });

  if (loading) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="flex justify-center items-center h-64">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary"></div>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="text-center text-red-600">
          <p>{error}</p>
        </div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold text-center mb-8">Thực đơn</h1>

      {/* Search and Filter */}
      <div className="mb-8">
        <div className="flex flex-col md:flex-row gap-4 justify-between items-center">
          <div className="w-full md:w-1/3">
            <input
              type="text"
              placeholder="Tìm món ăn..."
              className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-primary"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
          <div className="w-full md:w-1/3">
            <select
              className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-primary"
              value={selectedCategory}
              onChange={(e) => setSelectedCategory(e.target.value)}
            >
              <option value="all">Tất cả món</option>
              <option value="mon-khai-vi">Món khai vị</option>
              <option value="mon-chinh">Món chính</option>
              <option value="mon-trang-mieng">Món tráng miệng</option>
              <option value="do-uong">Đồ uống</option>
            </select>
          </div>
        </div>
      </div>

      {/* Dishes Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
        {filteredDishes.map((dish) => (
          <div key={dish.maMon} className="bg-white rounded-lg shadow-md overflow-hidden">
            <img
              src={dish.hinhAnh}
              alt={dish.tenMon}
              className="w-full h-48 object-cover"
            />
            <div className="p-4">
              <h3 className="text-xl font-bold mb-2">{dish.tenMon}</h3>
              <p className="text-gray-600 mb-4">{dish.moTa}</p>
              <div className="flex justify-between items-center">
                <span className="text-xl font-bold text-primary">
                  {dish.gia.toLocaleString('vi-VN')}đ
                </span>
                <button
                  className="bg-primary text-white px-4 py-2 rounded hover:bg-primary-dark"
                  onClick={() => {/* Add to cart logic */}}
                >
                  Thêm vào giỏ
                </button>
              </div>
            </div>
          </div>
        ))}
      </div>

      {filteredDishes.length === 0 && (
        <div className="text-center py-8">
          <p className="text-gray-600">Không tìm thấy món ăn phù hợp.</p>
        </div>
      )}
    </div>
  );
};

export default Menu; 