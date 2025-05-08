import React, { useState } from 'react'
import './LoginPopup.css'
import { assets } from '../../assets/assets'

const LoginPopup = ({ setShowLogin }) => {

  
  const [currState, setCurrState] = useState("Login")

  return (
    <div className='login-popup'>
      <form className='login-popup-container' >
        <div className="login-popup-title">
          <h2>{currState}</h2>
          <img onClick={() => setShowLogin(false)} src={assets.cross_icon} alt="" />
        </div>
        <div className="login-popup-inputs">
          {currState === "Sign Up" ? <input type="text" placeholder='Your Name' required /> : <></>}
          <input type="email" placeholder='Email' required />
          <input type="text" placeholder='Employee ID' required />
          <input type="password" placeholder='Password' required/>
        </div>
        <button>
          {currState === "Sign Up" ? "Create account" : "Login"}
        </button>
        
      </form>
    </div>
  )
}

export default LoginPopup
