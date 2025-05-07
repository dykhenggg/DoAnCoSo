import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './Login.css';
import { assets1 } from '../../assets/assets1';

const Login = ({ setShowLogin }) => {
  const [values, setValues] = useState({
    email: "",
    employeeId: "",
    password: ""
  });

  let navigate = useNavigate();

  const handleLogin = (event) => {
    event.preventDefault(); 

    // Validation logic
    if (!values.email || !values.employeeId || !values.password) {
      alert("Please fill in all fields.");
      return;
    }

    // If validation passes, proceed with login
    console.log("Logging in with:", values);
    setShowLogin(false);
    navigate('/dashboard'); // Navigate to a dashboard or another page
  };

  const handleChange = (event) => {
    const { name, value } = event.target;
    setValues({ ...values, [name]: value });
  };

  return (
    <div className='login-popup'>
      <form className='login-popup-container' onSubmit={handleLogin}>
        <div className="login-popup-title">
          <h2>Login</h2>
        </div>
        <div className="login-popup-inputs">
          <input
            type="email"
            name="email"
            placeholder="Email"
            value={values.email}
            onChange={handleChange}
            required
          />
          <input
            type="text"
            name="employeeId"
            placeholder="Employee ID"
            value={values.employeeId}
            onChange={handleChange}
            required
          />
          <input
            type="password"
            name="password"
            placeholder="Password"
            value={values.password}
            onChange={handleChange}
            required
          />
        </div>
        <button type="submit">
          Login
        </button>
      </form>
    </div>
  );
};

export default Login;
