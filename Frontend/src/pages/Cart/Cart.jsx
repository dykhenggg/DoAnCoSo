import React, { useContext } from 'react'
import './Cart.css'
import { StoreContext } from '../../Context/StoreContext'
import { food_list } from '../../assets/assets'
import { useNavigate } from 'react-router-dom'

const Cart = ({ setShowLogin }) => {
  const { cartItems, foodlist, removeFromCart, getTotalCartAmount, isLoggedIn } = useContext(StoreContext)
  const navigate = useNavigate();

  if (!isLoggedIn) {
    setShowLogin(true);
    return null;
  }

  return (
    <div className='cart'>
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
          if (cartItems[item._id] > 0) {
            return (
              <div>
                <div className="cart-items-title cart-items-item">
                  <img src={item.image} alt="" />
                  <p>{item.name}</p>
                  <p>${item.price}</p>
                  <p>{cartItems[item._id]}</p>
                  <p>${item.price * cartItems[item._id]}</p>
                  <p onClick={() => removeFromCart(item._id)} className='cross'>x</p>
                </div>
                <hr />
              </div>
            )
          }
        })}
      </div>
      <div className="cart-bottom">
        <div className="cart-total">
          <h2>Cart Total</h2>
          <div>
            <div className="cart-total-detail">
              <p>Subtotal</p>
              <p>${getTotalCartAmount()}</p>
            </div>
            <hr />
            <div className="cart-total-detail">
              <p>Delivery Free</p>
              <p>${getTotalCartAmount()===0?0:getTotalCartAmount()*0.1}</p>
            </div>
            <hr />
            <div className="cart-total-detail">
              <p>Total</p>
              <p>${getTotalCartAmount()===0?0:getTotalCartAmount()+(getTotalCartAmount()*0.1)}</p>
            </div>
          </div>
          <button onClick={()=> navigate('/order')}>PROCEED TO CHECKOUT</button>
        </div>
        <div className="cart-promocode">
          <div>
            <p>Add your promocode here</p>
            <div className="cart-promocode-input">
              <input type="text" placeholder='promocode' />
              <button>Apply</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default Cart
