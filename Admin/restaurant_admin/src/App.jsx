import React, { useState } from "react";
import { Route, Routes } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

// Components
import Navbar from "./components/Navbar/Navbar";
import Sidebar from "./components/Sidebar/Sidebar";
import Login from "./components/Login/Login";

// Pages
import Dashboard from "./pages/Dashboard/Dashboard";
import Foods from "./pages/Foods/Foods";
import Categories from "./pages/Categories/Categories";
import Order from "./pages/Order/Order";
import Departments from "./pages/Departments/Departments";
import Shifts from "./pages/Shifts/Shifts";
import Storage from "./pages/Storage/Storage";
// import Employees from "./pages/Employees/Employees";

const App = () => {
  const [showLogin, setShowLogin] = useState(true);
  return (
    <div>
      <ToastContainer />
      {showLogin ? <Login setShowLogin={setShowLogin} /> : <></>}
      <Navbar />
      <hr />
      <div className="app-content">
        <Sidebar />
        <Routes>
          <Route path="/" element={<Dashboard />} />
          <Route path="/foods" element={<Foods />} />
          <Route path="/categories" element={<Categories />} />
          <Route path="/order" element={<Order />} />
          <Route path="/departments" element={<Departments />} />
          <Route path="/shifts" element={<Shifts />} />
          <Route path="/storage" element={<Storage />} />
          {/* <Route path="/employees" element={<Employees />} /> */}
        </Routes>
      </div>
    </div>
  );
};

export default App;
