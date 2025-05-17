import { Link } from 'react-router-dom';
import { useAuth } from '../../Context/AuthContext';

const Header = () => {
  const { user, logout } = useAuth();

  return (
    <header className="bg-white shadow-md">
      <nav className="container mx-auto px-4 py-3">
        <div className="flex justify-between items-center">
          <Link to="/" className="text-2xl font-bold text-primary">
            Restaurant
          </Link>
          
          <div className="flex items-center space-x-6">
            <Link to="/menu" className="text-gray-600 hover:text-primary">
              Thực đơn
            </Link>
            <Link to="/reservation" className="text-gray-600 hover:text-primary">
              Đặt bàn
            </Link>
            {user ? (
              <>
                <Link to="/profile" className="text-gray-600 hover:text-primary">
                  Tài khoản
                </Link>
                <button
                  onClick={logout}
                  className="bg-primary text-white px-4 py-2 rounded hover:bg-primary-dark"
                >
                  Đăng xuất
                </button>
              </>
            ) : (
              <>
                <Link
                  to="/login"
                  className="text-gray-600 hover:text-primary"
                >
                  Đăng nhập
                </Link>
                <Link
                  to="/register"
                  className="bg-primary text-white px-4 py-2 rounded hover:bg-primary-dark"
                >
                  Đăng ký
                </Link>
              </>
            )}
          </div>
        </div>
      </nav>
    </header>
  );
};

export default Header; 