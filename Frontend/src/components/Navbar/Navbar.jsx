import React, { useState, useContext, useEffect, useRef } from "react";  
import './Navbar.css'
import {assets} from "../../assets/assets"; 
import { Link } from "react-router-dom";
import { StoreContext } from "../../Context/StoreContext";
import { useAuth } from '../../Context/AuthContext';

const Navbar = ({setShowLogin}) => {
    const [menu, setMenu] = useState("Home");
    const [isFixed, setIsFixed] = useState(false);
    const [showDropdown, setShowDropdown] = useState(false);
    const dropdownRef = useRef(null);
    const {getTotalCartAmount, isLoggedIn, user, logout} = useAuth();

    useEffect(() => {
        const handleScroll = () => {
            const header = document.querySelector('.header');
            if (header) {
                const headerHeight = header.offsetHeight;
                const scrollPosition = window.scrollY;
                setIsFixed(scrollPosition > headerHeight - 80); 
            }
        };

        window.addEventListener('scroll', handleScroll);
        return () => window.removeEventListener('scroll', handleScroll);
    }, []);

    // Close dropdown when clicking outside
    useEffect(() => {
        const handleClickOutside = (event) => {
            if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
                setShowDropdown(false);
            }
        };

        document.addEventListener('mousedown', handleClickOutside);
        return () => document.removeEventListener('mousedown', handleClickOutside);
    }, []);

    const handleScroll = (elementId) => {
        const element = document.getElementById(elementId);
        if (element) {
            const navbarHeight = 80;
            const elementPosition = element.getBoundingClientRect().top + window.pageYOffset;
            window.scrollTo({
                top: elementPosition - navbarHeight,
                behavior: 'smooth'
            });
        }
    };

    const handleLogout = () => {
        logout();
        setShowDropdown(false);
    };

    return (
        <div className={`navbar ${isFixed ? 'navbar-fixed' : ''}`}>
            <Link to='/'><img src={assets.logo} alt="" className="logo"/></Link>
            <ul className="navbar-menu">
                <Link to='/'><li onClick={()=>setMenu("Home")} className={menu==="Home"?"active":""}>Home</li></Link>
                <li 
                    onClick={() => {
                        setMenu("Menu");
                        handleScroll('food-display');
                    }} 
                    className={menu==="Menu"?"active":""}
                >
                    Menu
                </li>
                <li onClick={()=>setMenu("Reservation")} className={menu==="Reservation"?"active":""}>Reservation</li>
                
                <li 
                    onClick={() => {
                        setMenu("About Us");
                        handleScroll('about-us');
                        window.dispatchEvent(new Event('aboutUsClick'));
                    }} 
                    className={menu==="About Us"?"active":""}
                >
                    About Us
                </li>
            </ul>
            <div className="navbar-right">
                <img src={assets.search_icon} alt="search"/>
                <div className="navbar-search-icon">
                    <Link to='/cart'><img src={assets.basket_icon} alt="cart" /></Link>
                    <div className="dot"></div>
                </div>
                {isLoggedIn ? (
                  <div className="navbar-avatar-container" ref={dropdownRef}>
                    <img 
                      src={user?.avatar || assets.profile_icon} 
                      alt="avatar" 
                      className="navbar-avatar" 
                      title={user?.name || 'User'} 
                      onClick={() => setShowDropdown(!showDropdown)}
                    />
                    {showDropdown && (
                      <div className="user-dropdown">
                        <div className="user-info">
                          <p className="user-name">{user?.name || 'User'}</p>
                          <p className="user-id">ID: {user?.id || 'N/A'}</p>
                        </div>
                        <hr />
                        <button onClick={handleLogout} className="logout-btn">Log Out</button>
                      </div>
                    )}
                  </div>
                ) : (
                  <button onClick={()=>setShowLogin(true)}>Sign in</button>
                )}
            </div>
        </div>
    );
};

export default Navbar;