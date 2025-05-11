import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "./Login.css";
import { assets1 } from "../../assets/assets1";
import { ADMIN_ACCOUNT } from "../../constants/adminAccount";

const Login = ({ setShowLogin }) => {
  const [values, setValues] = useState({
    email: "",
    employeeId: "",
    password: "",
  });
  const [errors, setErrors] = useState({});
  const [isLoading, setIsLoading] = useState(false);

  let navigate = useNavigate();

  const validateForm = () => {
    const newErrors = {};
    if (!values.email) {
      newErrors.email = "Vui lòng nhập email";
    } else if (!/\S+@\S+\.\S+/.test(values.email)) {
      newErrors.email = "Email không hợp lệ";
    }
    if (!values.employeeId) {
      newErrors.employeeId = "Vui lòng nhập mã nhân viên";
    }
    if (!values.password) {
      newErrors.password = "Vui lòng nhập mật khẩu";
    }
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleLogin = async (event) => {
    event.preventDefault();
    if (!validateForm()) return;

    setIsLoading(true);
    try {
      // Kiểm tra thông tin đăng nhập với tài khoản admin
      if (
        values.email === ADMIN_ACCOUNT.email &&
        values.employeeId === ADMIN_ACCOUNT.employeeId &&
        values.password === ADMIN_ACCOUNT.password
      ) {
        // Giả lập token và thông tin user cho admin
        const adminData = {
          token: "admin-token-" + Date.now(),
          user: {
            ...ADMIN_ACCOUNT,
            id: 1,
            name: "Administrator",
          },
        };

        localStorage.setItem("token", adminData.token);
        localStorage.setItem("user", JSON.stringify(adminData.user));
        setShowLogin(false);
        navigate("/dashboard");
        return;
      }

      // Nếu không phải admin thì gọi API bình thường
      console.log("Attempting login with:", values);
      const apiUrl = `${import.meta.env.VITE_API_URL}/api/auth/login`;
      console.log("API URL:", apiUrl);

      const response = await fetch(apiUrl, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          email: values.email,
          password: values.password,
          employeeId: values.employeeId,
        }),
      });

      const data = await response.json();
      console.log("Response:", data);

      if (!response.ok) {
        throw new Error(data.message || "Đăng nhập thất bại");
      }

      if (data.token) {
        localStorage.setItem("token", data.token);
        localStorage.setItem("user", JSON.stringify(data.user));
        setShowLogin(false);
        navigate("/dashboard");
      } else {
        throw new Error("Token không hợp lệ");
      }
    } catch (error) {
      console.error("Login error:", error);
      setErrors({
        submit: error.message || "Đăng nhập thất bại. Vui lòng thử lại.",
      });
    } finally {
      setIsLoading(false);
    }
  };

  const handleChange = (event) => {
    const { name, value } = event.target;
    setValues({ ...values, [name]: value });
  };

  return (
    <div className="login-popup">
      <form className="login-popup-container" onSubmit={handleLogin}>
        <div className="login-popup-title">
          <h2>Đăng Nhập</h2>
        </div>
        <div className="login-popup-inputs">
          <input
            type="email"
            name="email"
            placeholder="Email"
            value={values.email}
            onChange={handleChange}
            className={errors.email ? "error" : ""}
          />
          {errors.email && (
            <span className="error-message">{errors.email}</span>
          )}

          <input
            type="text"
            name="employeeId"
            placeholder="Mã nhân viên"
            value={values.employeeId}
            onChange={handleChange}
            className={errors.employeeId ? "error" : ""}
          />
          {errors.employeeId && (
            <span className="error-message">{errors.employeeId}</span>
          )}

          <input
            type="password"
            name="password"
            placeholder="Mật khẩu"
            value={values.password}
            onChange={handleChange}
            className={errors.password ? "error" : ""}
          />
          {errors.password && (
            <span className="error-message">{errors.password}</span>
          )}
        </div>
        {errors.submit && <div className="error-message">{errors.submit}</div>}
        <button type="submit" disabled={isLoading}>
          {isLoading ? "Đang đăng nhập..." : "Đăng nhập"}
        </button>
      </form>
    </div>
  );
};

export default Login;
