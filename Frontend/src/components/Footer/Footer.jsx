import React from 'react';
import './Footer.css';
import logo from '../../assets/logo.png'; // Replace with the correct path to your logo

const Footer = () => {
  return (
    <div className="footer">
      <div className="footer-section">
        <img src={logo} alt="Logo" className="footer-logo" />
        <h3>Contact Us</h3>
        <p>Phone: <span>+123 456 789</span></p>
        <p>Email: <span>contact@restaurant.com</span></p>
      </div>
      <div className="footer-section">
        <h3>Follow Us</h3>
        <a href="https://facebook.com" target="_blank" rel="noopener noreferrer">Facebook</a>
        <a href="https://instagram.com" target="_blank" rel="noopener noreferrer">Instagram</a>
        <a href="https://youtube.com" target="_blank" rel="noopener noreferrer">YouTube</a>
      </div>
      <div className="footer-section">
        <h3>Find Us</h3>
        <iframe
          title="Google Map"
          src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3153.835434509374!2d144.9630579153169!3d-37.81410797975171!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x6ad642af0f11fd81%3A0xf577d8b7e1e2b1c!2sFederation%20Square!5e0!3m2!1sen!2sau!4v1681234567890!5m2!1sen!2sau"
          width="100%"
          height="200"
          style={{ border: 0 }}
          allowFullScreen=""
          loading="lazy"
        ></iframe>
      </div>
    </div>
  );
};

export default Footer;
