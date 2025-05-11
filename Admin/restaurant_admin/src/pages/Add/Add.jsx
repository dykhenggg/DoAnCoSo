import React, { useState } from "react";
import "./Add.css";
import { assets } from "../../assets/assets";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const Add = () => {
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);
  const [image, setImage] = useState(null);
  const [data, setData] = useState({
    name: "",
    description: "",
    price: "",
    category: "Salad",
  }); //thong tin mon an

  const onChangeHandler = (event) => {
    const { name, value } = event.target;
    setData((prev) => ({ ...prev, [name]: value }));
  };

  const validateForm = () => {
    if (!data.name.trim()) {
      toast.error("Name is required");
      return false;
    }
    if (!data.price || data.price <= 0) {
      toast.error("Price must be greater than 0");
      return false;
    }
    if (!image) {
      toast.error("Image is required");
      return false;
    }
    return true;
  };

  const resetForm = () => {
    setData({
      name: "",
      description: "",
      price: "",
      category: "Salad",
    });
    setImage(null);
  };

  const onSubmitHandler = async (event) => {
    event.preventDefault();
    if (!validateForm()) return;

    setIsLoading(true);
    try {
      const formData = new FormData();
      formData.append("tenMon", data.name.trim());
      formData.append("gia", parseFloat(data.price).toString());
      formData.append("loaiMon", data.category);
      formData.append("hinhAnh", image);

      const response = await axios.post(
        "http://localhost:5078/api/ThucDon",
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );

      if (response.status === 201 || response.status === 200) {
        toast.success("Thêm món ăn thành công!");
        resetForm();
        navigate("/foodlist");
      }
    } catch (error) {
      console.error("API Error Details:", error.response?.data);
      toast.error(error.response?.data?.message || "Thêm món ăn thất bại");
    } finally {
      setIsLoading(false);
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
        <div className="add-product-description flex-col">
          <p>Product description</p>
          <textarea
            onChange={onChangeHandler}
            value={data.description}
            name="description"
            rows="6"
            placeholder="Enter Product Description"
            required
          ></textarea>
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
        <button type="submit" className="add-btn" disabled={isLoading}>
          {isLoading ? "ĐANG THÊM..." : "THÊM MÓN ĂN"}
        </button>
      </form>
    </div>
  );
};

export default Add;
