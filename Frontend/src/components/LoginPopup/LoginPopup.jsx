import React, { useState, useEffect, useContext } from 'react'
import { useNavigate } from 'react-router-dom';
import './LoginPopup.css'
import { assets } from '../../assets/assets'
import { StoreContext } from '../../Context/StoreContext';

const LoginPopup = ({ setShowLogin }) => {
  const [currState, setCurrState] = useState("Login")
  const navigate = useNavigate();
  const { login } = useContext(StoreContext);

  // Lock body scroll when component mounts
  useEffect(() => {
    document.body.style.overflow = 'hidden';
    return () => {
      document.body.style.overflow = 'unset';
    };
  }, []);

  // Handle click outside to close
  const handleBackdropClick = (e) => {
    if (e.target.className === 'login-popup') {
      setShowLogin(false);
    }
  };

  // Handle escape key to close
  useEffect(() => {
    const handleEscape = (e) => {
      if (e.key === 'Escape') {
        setShowLogin(false);
      }
    };
    window.addEventListener('keydown', handleEscape);
    return () => window.removeEventListener('keydown', handleEscape);
  }, [setShowLogin]);

  // Handle form submit
  const handleSubmit = (e) => {
    e.preventDefault();
    // For demo, just use dummy user info
    login({
      name: currState === 'Sign Up' ? e.target[0].value : 'User',
      email: e.target[1].value,
      avatar: assets.profile_icon
    });
    setShowLogin(false);
    navigate('/');
  };

  return (
    <div className='login-popup' onClick={handleBackdropClick}>
      <form className='login-popup-container' onSubmit={handleSubmit}>
        <div className="login-popup-title">
          <h2>{currState}</h2>
          <img 
            onClick={() => setShowLogin(false)} 
            src={assets.cross_icon} 
            alt="Close" 
            className="close-button"
          />
        </div>
        <div className="login-popup-inputs">
          {currState === "Sign Up" ? <input type="text" placeholder='Your Name' required /> : <></>}
          <input type="email" placeholder='Email' required />
          <input type="text" placeholder='ID' required />
          <input type="password" placeholder='Password' required/>
        </div>
        <button type="submit">
          {currState === "Sign Up" ? "Create account" : "Login"}
        </button>
        <p className="login-popup-switch">
          {currState === "Login" ? (
            <>Don't have an account? <span onClick={() => setCurrState("Sign Up")}>Sign Up</span></>
          ) : (
            <>Already have an account? <span onClick={() => setCurrState("Login")}>Login</span></>
          )}
        </p>
      </form>
    </div>
  )
}

export default LoginPopup
