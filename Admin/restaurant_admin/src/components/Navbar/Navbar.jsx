import React from 'react'
import './Navbar.css'
import {assets1} from '../../assets/assets1'
import {assets} from '../../assets/assets'

const Navbar = () => {
  return (
    <div className="navbar">
      <img className='logo' src={assets1.logo} alt="" />
      <img className='profile' src={assets.profile_image} alt="" />
    </div>
  )
}

export default Navbar
