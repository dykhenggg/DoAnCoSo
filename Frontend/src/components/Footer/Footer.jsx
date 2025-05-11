import React from 'react';
import './Footer.css';
import { assets } from '../../assets/assets';
import { Link } from 'react-router-dom';

const Footer = () => {
  return (
    <footer className="footer">
      <div className="footer-content">
        <div className="footer-section brand">
          <img src={assets.logo} alt="Logo" className="footer-logo" />
          <p className="brand-description">
            Experience the finest culinary journey with our exceptional dishes and warm hospitality.
          </p>
          <div className="social-links">
            <a href="https://facebook.com" target="_blank" rel="noopener noreferrer">
              <i className="bx bxl-facebook"></i>
            </a>
            <a href="https://instagram.com" target="_blank" rel="noopener noreferrer">
              <i className="bx bxl-instagram"></i>
            </a>
            <a href="https://twitter.com" target="_blank" rel="noopener noreferrer">
              <i className="bx bxl-twitter"></i>
            </a>
          </div>
        </div>

        <div className="footer-section contact">
          <h3>Contact Us</h3>
          <div className="contact-info">
            <p><i className="bx bx-map"></i> 123 Restaurant Street, Food City, FC 12345</p>
            <p><i className="bx bx-phone"></i> +1 (555) 123-4567</p>
            <p><i className="bx bx-envelope"></i> info@restaurant.com</p>
          </div>
        </div>

        <div className="footer-section hours">
          <h3>Opening Hours</h3>
          <div className="hours-info">
            <p>Monday - Friday: 11:00 AM - 10:00 PM</p>
            <p>Saturday: 10:00 AM - 11:00 PM</p>
            <p>Sunday: 10:00 AM - 9:00 PM</p>
          </div>
        </div>
      </div>

      <div className="footer-bottom">
        <p>&copy; 2024 Restaurant Name. All rights reserved.</p>
        <div className="footer-bottom-links">
          <Link to="/privacy">Privacy Policy</Link>
          <Link to="/terms">Terms of Service</Link>
        </div>
      </div>
    </footer>
  );
};

export default Footer;
