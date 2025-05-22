import React, { useContext } from "react";
import "./Cart.css";
import { StoreContext } from "../../Context/StoreContext";
import { food_list } from "../../assets/assets";
import { useNavigate } from "react-router-dom";

const Cart = ({ setShowLogin }) => {
  const {
    cartItems,
    foodlist,
    removeFromCart,
    getTotalCartAmount,
    isLoggedIn,
  } = useContext(StoreContext);
  const navigate = useNavigate();

  if (!isLoggedIn) {
    setShowLogin(true);
    return null;
  }

  return (
    <div className="cart">
      <div className="cart-items">
        <div className="cart-items-title">
          <p>Items</p>
          <p>Title</p>
          <p>Price</p>
          <p>Quantity</p>
          <p>Total</p>
          <p>Remove</p>
        </div>
        <br />
        <hr />
        {food_list.map((item, index) => {
          if (cartItems[item.maMon] > 0) {
            return (
              <div key={item.maMon}>
                {" "}
                // Thay key={index} bằng key={item.maMon}
                <div className="cart-items-title cart-items-item">
                  <img
                    src={`http://localhost:5078/images/${item.hinhAnh}`}
                    alt=""
                  />{" "}
                  {/* Thay đổi từ image */}
                  <p>{item.tenMon}</p> {/* Thay đổi từ name thành tenMon */}
                  <p>{item.gia}VND</p> {/* Thay đổi từ price thành gia */}
                  <p>{cartItems[item.maMon]}</p>
                  <p>{item.gia * cartItems[item.maMon]}VND</p>
                  <p
                    onClick={() => removeFromCart(item.maMon)}
                    className="cross"
                  >
                    x
                  </p>
                </div>
                <hr />
              </div>
            );
          }
          return null;
        })}
      </div>
      <div className="cart-bottom">
        <div className="cart-total">
          <h2>Cart Total</h2>
          <div>
            <div className="cart-total-detail">
              <p>Subtotal</p>
              <p>{getTotalCartAmount()}VND</p>
            </div>
            <hr />
            <div className="cart-total-detail">
              <p>Delivery Free</p>
              <p>
                {getTotalCartAmount() === 0 ? 0 : getTotalCartAmount() * 0.1}VND
              </p>
            </div>
            <hr />
            <div className="cart-total-detail">
              <p>Total</p>
              <p>
                {getTotalCartAmount() === 0
                  ? 0
                  : getTotalCartAmount() + getTotalCartAmount() * 0.1}
                VND
              </p>
            </div>
          </div>
          <button onClick={() => navigate("/order")}>
            PROCEED TO CHECKOUT
          </button>
        </div>
        <div className="cart-promocode">
          <div>
            <p>Add your promocode here</p>
            <div className="cart-promocode-input">
              <input type="text" placeholder="promocode" />
              <button>Apply</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Cart;
