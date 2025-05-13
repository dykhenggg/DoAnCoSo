import React from "react";
import "./Dashboard.css";

const Dashboard = () => {
  return (
    <div className="dashboard">
      <h2>Dashboard</h2>
      <div className="dashboard-stats">
        <div className="stat-card">
          <h3>Tổng đơn hàng</h3>
          <p>0</p>
        </div>
        <div className="stat-card">
          <h3>Doanh thu</h3>
          <p>0 VNĐ</p>
        </div>
        <div className="stat-card">
          <h3>Khách hàng</h3>
          <p>0</p>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
