import React, { useState } from "react";
import "./Add.css";
import { assets } from "../../assets/assets";
import { toast } from "react-toastify"; // Thêm toast để hiển thị thông báo
import { useNavigate } from "react-router-dom";
console.log("sada");
const Add = () => {
  const url = "http://localhost:5078"; // Use HTTP instead of HTTPS
  const [image, setImage] = useState(null);
  const [data, setData] = useState({
    name: "",
    price: "",
    category: "Salad", // Thông tin món ăn
  });
  const navigate = useNavigate();

  const onChangeHandler = (event) => {
    const name = event.target.name;
    const value = event.target.value;
    setData((data) => ({ ...data, [name]: value }));
  };

  const onSubmitHandler = async (event) => {
    event.preventDefault();
    const formData = new FormData();
    formData.append("TenMon", data.name);
    formData.append("Gia", Number(data.price));
    formData.append("LoaiMon", data.category);
    formData.append("HinhAnh", image);

    try {
      const response = await fetch(`${url}/api/ThucDon`, {
        method: "POST",
        body: formData,
        headers: {
          Accept: "application/json",
        },
      });

      if (response.ok) {
        setData({
          name: "",
          price: "",
          category: "Salad",
        });
        setImage(null);
        toast.success("Thêm món ăn thành công!");
        navigate("/menu"); // Navigate to menu after success
      } else {
        const error = await response.text();
        toast.error(error || "Thêm món ăn thất bại!");
      }
    } catch (error) {
      console.error("Error:", error);
      toast.error("Không thể kết nối đến server!");
    }
  };

  return (
    <div className="add">
      <form className="flex-col" onSubmit={onSubmitHandler}>
        <div className="add-image-upload flex-col">
          <p>Upload Image</p>
          <label htmlFor="food-image">
            <img
              className="inserted-image"
              src={image ? URL.createObjectURL(image) : assets.upload_area}
              alt="Upload Area"
            />
          </label>
          <input
            onChange={(e) => setImage(e.target.files[0])}
            type="file"
            id="food-image"
            hidden
            required
          />
        </div>
        <div className="add-product-name flex-col">
          <p>Product Name</p>
          <input
            onChange={onChangeHandler}
            value={data.name}
            type="text"
            name="name"
            placeholder="Enter Product Name"
            required
          />
        </div>
        <div className="add-category-price">
          <div className="add-category flex-col">
            <p>Product category</p>
            <select onChange={onChangeHandler} name="category">
              <option value="Salad">Salad</option>
              <option value="Rolls">Rolls</option>
              <option value="Deserts">Deserts</option>
              <option value="Sandwich">Sandwich</option>
              <option value="Cake">Cake</option>
              <option value="Pure Veg">Pure</option>
              <option value="Pasta">Pasta</option>
              <option value="Noodles">Noodles</option>
            </select>
          </div>
          <div className="add-price flex-col">
            <p>Product Price</p>
            <input
              onChange={onChangeHandler}
              value={data.price}
              type="number"
              name="price"
              placeholder="Enter Product Price"
              required
            />
          </div>
        </div>
        <button type="submit" className="add-btn">
          ADD
        </button>
      </form>
    </div>
  );
};

export default Add;
