import { useState, useEffect } from 'react';
import api from '../config/axios';
import { useAuth } from '../Context/AuthContext';

const Reservation = () => {
  const { user } = useAuth();
  const [tables, setTables] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [formData, setFormData] = useState({
    ngayDat: '',
    gioDat: '',
    soNguoi: 2,
    ghiChu: '',
    maBan: ''
  });

  useEffect(() => {
    const fetchTables = async () => {
      try {
        const response = await api.get('/ban');
        setTables(response.data);
        setLoading(false);
      } catch (err) {
        setError('Không thể tải danh sách bàn. Vui lòng thử lại sau.');
        setLoading(false);
      }
    };

    fetchTables();
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!user) {
      alert('Vui lòng đăng nhập để đặt bàn');
      return;
    }

    try {
      await api.post('/datban', {
        ...formData,
        maKhachHang: user.maKhachHang
      });
      alert('Đặt bàn thành công!');
      setFormData({
        ngayDat: '',
        gioDat: '',
        soNguoi: 2,
        ghiChu: '',
        maBan: ''
      });
    } catch (err) {
      alert('Đặt bàn thất bại. Vui lòng thử lại sau.');
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

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
      <h1 className="text-3xl font-bold text-center mb-8">Đặt bàn</h1>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
        {/* Reservation Form */}
        <div className="bg-white p-6 rounded-lg shadow-md">
          <h2 className="text-xl font-bold mb-4">Thông tin đặt bàn</h2>
          <form onSubmit={handleSubmit} className="space-y-4">
            <div>
              <label className="block text-gray-700 mb-2">Ngày đặt</label>
              <input
                type="date"
                name="ngayDat"
                value={formData.ngayDat}
                onChange={handleChange}
                className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-primary"
                required
              />
            </div>

            <div>
              <label className="block text-gray-700 mb-2">Giờ đặt</label>
              <input
                type="time"
                name="gioDat"
                value={formData.gioDat}
                onChange={handleChange}
                className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-primary"
                required
              />
            </div>

            <div>
              <label className="block text-gray-700 mb-2">Số người</label>
              <input
                type="number"
                name="soNguoi"
                value={formData.soNguoi}
                onChange={handleChange}
                min="1"
                max="20"
                className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-primary"
                required
              />
            </div>

            <div>
              <label className="block text-gray-700 mb-2">Ghi chú</label>
              <textarea
                name="ghiChu"
                value={formData.ghiChu}
                onChange={handleChange}
                className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-primary"
                rows="3"
              ></textarea>
            </div>

            <button
              type="submit"
              className="w-full bg-primary text-white px-4 py-2 rounded-lg hover:bg-primary-dark"
            >
              Đặt bàn
            </button>
          </form>
        </div>

        {/* Available Tables */}
        <div>
          <h2 className="text-xl font-bold mb-4">Bàn có sẵn</h2>
          <div className="grid grid-cols-2 gap-4">
            {tables.map(table => (
              <div
                key={table.maBan}
                className={`p-4 rounded-lg border cursor-pointer ${
                  formData.maBan === table.maBan
                    ? 'border-primary bg-primary bg-opacity-10'
                    : 'border-gray-200 hover:border-primary'
                }`}
                onClick={() => setFormData(prev => ({ ...prev, maBan: table.maBan }))}
              >
                <h3 className="font-bold">Bàn {table.tenBan}</h3>
                <p className="text-gray-600">Sức chứa: {table.sucChua} người</p>
                <p className="text-gray-600">Vị trí: {table.viTri}</p>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
};

export default Reservation; 