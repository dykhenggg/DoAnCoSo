import React, { useState, useEffect } from "react";
import "./List.css";
import { toast } from "react-toastify";
import axios from "axios";

const List = () => {
  const [foodList, setFoodList] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    fetchFoodList();
  }, []);

  const fetchFoodList = async () => {
    try {
      const response = await axios.get("http://localhost:5078/api/ThucDon");
      setFoodList(response.data);
      setIsLoading(false);
    } catch (error) {
      toast.error("Không thể tải danh sách món ăn");
      setIsLoading(false);
    }
  };

  const handleDelete = async (id) => {
    if (window.confirm("Bạn có chắc chắn muốn xóa món ăn này?")) {
      try {
        await axios.delete(`http://localhost:5078/api/ThucDon/${id}`);
        toast.success("Xóa món ăn thành công");
        fetchFoodList();
      } catch (error) {
        toast.error("Không thể xóa món ăn");
      }
    }
  };

  if (isLoading) return <div>Loading...</div>;

  return (
    <div className="list add flex-col">
      <h2>Danh sách món ăn</h2>
      <div className="list-table">
        <div className="list-table-format title">
          <b>Ảnh</b>
          <b>Tên món</b>
          <b>Loại</b>
          <b>Giá</b>
          <b>Thao tác</b>
        </div>
        {foodList.map((food) => (
          <div key={food.maMon} className="list-table-format">
            <img
              src={`http://localhost:5078/images/${food.hinhAnh}`}
              alt={food.tenMon}
              style={{ width: "50px", height: "50px", objectFit: "cover" }}
            />
            <p>{food.tenMon}</p>
            <p>{food.loaiMon}</p>
            <p>{food.gia.toLocaleString("vi-VN")} VNĐ</p>
            <div>
              <button onClick={() => handleDelete(food.maMon)}>Xóa</button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default List;
