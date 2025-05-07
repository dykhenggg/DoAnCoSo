import React from 'react'
import './Sidebar.css'
import { assets } from '../../assets/assets'
import {assets1} from '../../assets/assets1'
import { NavLink } from 'react-router-dom'

const Sidebar = () => {
  return (
    <div className="sidebar">
        <div className="sidebar-options">
            <NavLink to='/add' className="sidebar-option">
                <img src={assets.add_icon} alt="" />
                <p>Add Items</p>
            </NavLink>
            <NavLink to='/foodlist' className="sidebar-option">
                <img src={assets.order_icon} alt="" />
                <p>List Items</p>
            </NavLink>
            <NavLink to='/order' className="sidebar-option">
                <img src={assets.order_icon} alt="" />
                <p>Order</p>
            </NavLink>
            <NavLink to='/employees' className="sidebar-option">
                <img src={assets.order_icon} alt="" />
                <p>Employees</p>
            </NavLink>
            <NavLink to='/shifts' className="sidebar-option">
                <img src={assets.order_icon} alt="" />
                <p>Employees</p>
            </NavLink>
            <NavLink to='/storage' className="sidebar-option">
                <img src={assets.order_icon} alt="" />
                <p>Employees</p>
            </NavLink>
            <NavLink to='/logout' className="sidebar-option">
                <img src={assets1.logout_icon} alt="" />
                <p>Logout</p>
            </NavLink>

        </div>
    </div>
  )
}

export default Sidebar
