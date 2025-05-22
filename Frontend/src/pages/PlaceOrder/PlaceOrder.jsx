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
          <p className="title">Delivery Information</p>
          <div className="multi-fields">
            <input type="text" placeholder="First Name" />
            <input type="text" placeholder="Last Name" />
          </div>
          <input type="email" placeholder="Email address" />
          <input type="text" placeholder="Street" />
          <div className="multi-fields">
            <input type="text" placeholder="District" />
            <input type="text" placeholder="Ward" />
          </div>
          <input type="text" placeholder="Phone" />
        </div>
        <div className="place-order-right">
          <div className="cart-total">
            <h2>Cart Total</h2>
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
                    {getTotalCartAmount() === 0 ? 0 : getTotalCartAmount() * 0.1}
                    VND
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
            </div>
            <button onClick={handlePaymentClick}>PROCEED TO PAYMENT</button>
          </div>
        </div>
      </form>
      <PaymentPopup show={showPayment} onClose={() => setShowPayment(false)} />
    </>
  );
};

export default PlaceOrder;
