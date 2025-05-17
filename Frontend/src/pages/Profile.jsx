import { useState, useEffect } from 'react';
import { useAuth } from '../Context/AuthContext';
import api from '../config/axios';

const Profile = () => {
  const { user } = useAuth();
  const [formData, setFormData] = useState({
    hoTen: '',
    email: '',
    sdt: '',
    diaChi: ''
  });
  const [orders, setOrders] = useState([]);
  const [reservations, setReservations] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const response = await api.get(`/khachhang/${user.maKhachHang}`);
        setFormData({
          hoTen: response.data.hoTen,
          email: response.data.email,
          sdt: response.data.sdt,
          diaChi: response.data.diaChi
        });

        // Fetch orders
        const ordersResponse = await api.get(`/donhang/khachhang/${user.maKhachHang}`);
        setOrders(ordersResponse.data);

        // Fetch reservations
        const reservationsResponse = await api.get(`/datban/khachhang/${user.maKhachHang}`);
        setReservations(reservationsResponse.data);

        setLoading(false);
      } catch (err) {
        setError('Không thể tải thông tin người dùng');
        setLoading(false);
      }
    };

    fetchUserData();
  }, [user.maKhachHang]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    try {
      await api.put(`/khachhang/${user.maKhachHang}`, formData);
      setSuccess('Cập nhật thông tin thành công');
    } catch (err) {
      setError('Cập nhật thông tin thất bại');
    }
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

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold text-center mb-8">Thông tin cá nhân</h1>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
        {/* Profile Form */}
        <div className="bg-white p-6 rounded-lg shadow-md">
          <h2 className="text-xl font-bold mb-4">Cập nhật thông tin</h2>
          
          {error && (
            <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative mb-4">
              {error}
            </div>
          )}
          
          {success && (
            <div className="bg-green-100 border border-green-400 text-green-700 px-4 py-3 rounded relative mb-4">
              {success}
            </div>
          )}

          <form onSubmit={handleSubmit} className="space-y-4">
            <div>
              <label className="block text-gray-700 mb-2">Họ và tên</label>
              <input
                type="text"
                name="hoTen"
                value={formData.hoTen}
                onChange={handleChange}
                className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-primary"
                required
              />
            </div>

            <div>
              <label className="block text-gray-700 mb-2">Email</label>
              <input
                type="email"
                name="email"
                value={formData.email}
                onChange={handleChange}
                className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-primary"
                required
              />
            </div>

            <div>
              <label className="block text-gray-700 mb-2">Số điện thoại</label>
              <input
                type="tel"
                name="sdt"
                value={formData.sdt}
                onChange={handleChange}
                className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-primary"
                required
              />
            </div>

            <div>
              <label className="block text-gray-700 mb-2">Địa chỉ</label>
              <textarea
                name="diaChi"
                value={formData.diaChi}
                onChange={handleChange}
                className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-primary"
                rows="3"
                required
              ></textarea>
            </div>

            <button
              type="submit"
              className="w-full bg-primary text-white px-4 py-2 rounded-lg hover:bg-primary-dark"
            >
              Cập nhật
            </button>
          </form>
        </div>

        {/* Orders and Reservations */}
        <div className="space-y-8">
          {/* Orders */}
          <div className="bg-white p-6 rounded-lg shadow-md">
            <h2 className="text-xl font-bold mb-4">Lịch sử đơn hàng</h2>
            {orders.length > 0 ? (
              <div className="space-y-4">
                {orders.map(order => (
                  <div key={order.maDonHang} className="border rounded-lg p-4">
                    <div className="flex justify-between items-center mb-2">
                      <span className="font-bold">Đơn hàng #{order.maDonHang}</span>
                      <span className={`px-2 py-1 rounded text-sm ${
                        order.trangThai === 'Hoàn thành' ? 'bg-green-100 text-green-800' :
                        order.trangThai === 'Đang xử lý' ? 'bg-yellow-100 text-yellow-800' :
                        'bg-red-100 text-red-800'
                      }`}>
                        {order.trangThai}
                      </span>
                    </div>
                    <p className="text-gray-600">Ngày đặt: {new Date(order.ngayDat).toLocaleDateString()}</p>
                    <p className="text-gray-600">Tổng tiền: {order.tongTien.toLocaleString('vi-VN')}đ</p>
                  </div>
                ))}
              </div>
            ) : (
              <p className="text-gray-600">Chưa có đơn hàng nào</p>
            )}
          </div>

          {/* Reservations */}
          <div className="bg-white p-6 rounded-lg shadow-md">
            <h2 className="text-xl font-bold mb-4">Lịch sử đặt bàn</h2>
            {reservations.length > 0 ? (
              <div className="space-y-4">
                {reservations.map(reservation => (
                  <div key={reservation.maDatBan} className="border rounded-lg p-4">
                    <div className="flex justify-between items-center mb-2">
                      <span className="font-bold">Đặt bàn #{reservation.maDatBan}</span>
                      <span className={`px-2 py-1 rounded text-sm ${
                        reservation.trangThai === 'Đã xác nhận' ? 'bg-green-100 text-green-800' :
                        reservation.trangThai === 'Đang chờ' ? 'bg-yellow-100 text-yellow-800' :
                        'bg-red-100 text-red-800'
                      }`}>
                        {reservation.trangThai}
                      </span>
                    </div>
                    <p className="text-gray-600">Ngày đặt: {new Date(reservation.ngayDat).toLocaleDateString()}</p>
                    <p className="text-gray-600">Giờ đặt: {reservation.gioDat}</p>
                    <p className="text-gray-600">Số người: {reservation.soNguoi}</p>
                  </div>
                ))}
              </div>
            ) : (
              <p className="text-gray-600">Chưa có lịch sử đặt bàn</p>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default Profile; 