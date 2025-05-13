import React, { useState, useEffect } from "react";
import "./Menu.css";

const Menu = () => {
  const [menuItems, setMenuItems] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchMenuItems();
  }, []);

  const fetchMenuItems = async () => {
    try {
      const response = await fetch("http://localhost:5078/api/ThucDon");
      if (response.ok) {
        const data = await response.json();
        setMenuItems(data);
      }
    } catch (error) {
      console.error("Error fetching menu:", error);
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <div>Đang tải...</div>;

  return (
    <div className="menu">
      <h2>Danh sách món ăn</h2>
      <div className="menu-grid">
        {menuItems.map((item) => (
          <div key={item.maMon} className="menu-item">
            <img
              src={`http://localhost:5078/images/${item.hinhAnh}`}
              alt={item.tenMon}
              onError={(e) => {
                e.target.onerror = null;
                e.target.src = "default-food-image.jpg";
              }}
            />
            <h3>{item.tenMon}</h3>
            <p>{item.loaiMon}</p>
            <p className="price">{item.gia.toLocaleString("vi-VN")} VNĐ</p>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Menu;
