import React from "react";
import "./Sidebar.css";
import { assets } from "../../assets/assets";
import { assets1 } from "../../assets/assets1";
import { NavLink } from "react-router-dom";

const Sidebar = () => {
  const handleLogout = () => {
    window.location.reload();
  };

  return (
    <div className="sidebar">
      <div className="sidebar-options">
        <NavLink to="/" className="sidebar-option">
          <img src={assets.order_icon} alt="" />
          <p>Dashboard</p>
        </NavLink>
        <NavLink to="/categories" className="sidebar-option">
          <img src={assets.order_icon} alt="" />
          <p>Loại món</p>
        </NavLink>
        <NavLink to="/foods" className="sidebar-option">
          <img src={assets.order_icon} alt="" />
          <p>Món ăn</p>
        </NavLink>
        <NavLink to="/order" className="sidebar-option">
          <img src={assets.order_icon} alt="" />
          <p>Đặt bàn</p>
        </NavLink>
        <NavLink to="/departments" className="sidebar-option">
          <img src={assets.order_icon} alt="" />
          <p>Các bộ phận</p>
        </NavLink>
        <NavLink to="/employees" className="sidebar-option">
          <img src={assets.order_icon} alt="" />
          <p>Nhân viên</p>
        </NavLink>
        <NavLink to="/shifts" className="sidebar-option">
          <img src={assets.order_icon} alt="" />
          <p>Ca làm việc</p>
        </NavLink>
        <NavLink to="/storage" className="sidebar-option">
          <img src={assets.order_icon} alt="" />
          <p>Kho</p>
        </NavLink>
        <div className="sidebar-option" onClick={handleLogout}>
          <img src={assets1.logout_icon} alt="" />
          <p>Logout</p>
        </div>
      </div>
    </div>
  );
};

export default Sidebar;
