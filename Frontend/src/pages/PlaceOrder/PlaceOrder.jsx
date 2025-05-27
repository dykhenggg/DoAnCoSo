import React, { useContext, useState } from "react";
import "./PlaceOrder.css";
import { StoreContext } from "../../Context/StoreContext";
import PaymentPopup from "../../components/PaymentPopup/PaymentPopup";

const PlaceOrder = () => {
  const { getTotalCartAmount } = useContext(StoreContext);
  const [showPayment, setShowPayment] = useState(false);

  const handlePaymentClick = (e) => {
    e.preventDefault();
    setShowPayment(true);
  };

  return (
    <>
      <form className="place-order">
        <div className="place-order-left">
          <p className="title">Thông tin giao hàng</p>
          <div className="multi-fields">
            <input type="text" placeholder="Họ" />
            <input type="text" placeholder="Tên" />
          </div>
          <input type="email" placeholder="Địa chỉ email" />
          <input type="text" placeholder="Tên đường" />
          <div className="multi-fields">
            <input type="text" placeholder="Quận/Huyện" />
            <input type="text" placeholder="Phường/Xã" />
          </div>
          <input type="text" placeholder="Số điện thoại" />
        </div>
        <div className="place-order-right">
          <div className="cart-total">
            <h2>Tổng giỏ hàng</h2>
            <div>
              <div className="cart-total-detail">
                <p>Tạm tính</p>
                <p>{getTotalCartAmount().toLocaleString()}đ</p>
              </div>
              <hr />
              <div className="cart-total-detail">
                <p>Phí giao hàng</p>
                <p>
                  {(getTotalCartAmount() === 0
                    ? 0
                    : getTotalCartAmount() * 0.1
                  ).toLocaleString()}
                  đ
                </p>
              </div>
              <hr />
              <div className="cart-total-detail">
                <p>Tổng cộng</p>
                <p>
                  {(getTotalCartAmount() === 0
                    ? 0
                    : getTotalCartAmount() + getTotalCartAmount() * 0.1
                  ).toLocaleString()}
                  đ
                </p>
              </div>
            </div>
            <button onClick={handlePaymentClick}>TIẾN HÀNH THANH TOÁN</button>
          </div>
        </div>
      </form>
      <PaymentPopup show={showPayment} onClose={() => setShowPayment(false)} />
    </>
  );
};

export default PlaceOrder;
