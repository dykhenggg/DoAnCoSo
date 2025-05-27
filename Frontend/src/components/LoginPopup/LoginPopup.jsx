import React, { useState, useEffect, useContext } from 'react'
import { useNavigate } from 'react-router-dom';
import './LoginPopup.css'
import { assets } from '../../assets/assets'
import { StoreContext } from '../../Context/StoreContext';
import axios from 'axios';

const LoginPopup = ({ setShowLogin }) => {
  const [currState, setCurrState] = useState("Login")
  const [error, setError] = useState("")
  const navigate = useNavigate();
  const { login } = useContext(StoreContext);

  // Lock body scroll when component mounts
  useEffect(() => {
    document.body.style.overflow = 'hidden';
    return () => {
      document.body.style.overflow = 'unset';
    };
  }, []);

  // Handle click outside to close
  const handleBackdropClick = (e) => {
    if (e.target.className === 'login-popup') {
      setShowLogin(false);
    }
  };

  // Handle escape key to close
  useEffect(() => {
    const handleEscape = (e) => {
      if (e.key === 'Escape') {
        setShowLogin(false);
      }
    };
    window.addEventListener('keydown', handleEscape);
    return () => window.removeEventListener('keydown', handleEscape);
  }, [setShowLogin]);

  // Handle form submit
  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    try {
      if (currState === "Login") {
        const response = await axios.post("http://localhost:5078/api/TaiKhoan/login", {
          tenDangNhap: e.target[1].value,
          matKhau: e.target[2].value
        });

        if (response.data.token) {
          // Store token in localStorage
          localStorage.setItem('token', response.data.token);
          // Store user info
          login(response.data.taiKhoan);
          setShowLogin(false);
          navigate('/');
        }
      } else {
        // Handle registration
        const response = await axios.post("http://localhost:5078/api/TaiKhoan/register", {
          hoTen: e.target[0].value,
          tenDangNhap: e.target[1].value,
          email: e.target[2].value,
          matKhau: e.target[3].value,
          sdt: e.target[4].value,
          maVaiTro: 2 // Default role for regular users
        });

        if (response.data) {
          setCurrState("Login");
          setError("Đăng ký thành công! Vui lòng đăng nhập.");
        }
      }
    } catch (err) {
      setError(err.response?.data || "Có lỗi xảy ra. Vui lòng thử lại.");
    }
  };

  return (
    <div className='login-popup' onClick={handleBackdropClick}>
      <form className='login-popup-container' onSubmit={handleSubmit}>
        <div className="login-popup-title">
          <h2>{currState}</h2>
          <img 
            onClick={() => setShowLogin(false)} 
            src={assets.cross_icon} 
            alt="Close" 
            className="close-button"
          />
        </div>
        {error && <div className="error-message">{error}</div>}
        <div className="login-popup-inputs">
          {currState === "Sign Up" ? (
            <>
              <input type="text" placeholder='Họ và tên' required />
              <input type="text" placeholder='Tên đăng nhập' required />
              <input type="email" placeholder='Email' required />
              <input type="password" placeholder='Mật khẩu' required />
              <input type="tel" placeholder='Số điện thoại' required />
            </>
          ) : (
            <>
              <input type="text" placeholder='Tên đăng nhập' required />
              <input type="password" placeholder='Mật khẩu' required />
            </>
          )}
        </div>
        <button type="submit">
          {currState === "Sign Up" ? "Đăng ký" : "Đăng nhập"}
        </button>
        <p className="login-popup-switch">
          {currState === "Login" ? (
            <>Chưa có tài khoản? <span onClick={() => setCurrState("Sign Up")}>Đăng ký</span></>
          ) : (
            <>Đã có tài khoản? <span onClick={() => setCurrState("Login")}>Đăng nhập</span></>
          )}
        </p>
      </form>
    </div>
  )
}

export default LoginPopup
