import React, { useState } from "react";
import Navbar from "./components/Navbar/Navbar";
import Sidebar from "./components/Sidebar/Sidebar";
import { Route, Routes } from "react-router-dom";
import Add from "./pages/Add/Add";
import List from "./pages/List/List";
import Order from "./pages/Order/Order";
import Employees from "./pages/Employees/Employees";
import Shifts from "./pages/Shifts/Shifts";
import Storage from "./pages/Storage/Storage";
import Login from "./components/Login/Login";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

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
          <Route path="/" element={<List />} />
          <Route path="/dashboard" element={<List />} />
          <Route
            path="/add"
            element={
              showLogin ? <Login setShowLogin={setShowLogin} /> : <Add />
            }
          />
          <Route
            path="/foodlist"
            element={
              showLogin ? <Login setShowLogin={setShowLogin} /> : <List />
            }
          />
          <Route path="/order" element={<Order />} />
          <Route path="/employees" element={<Employees />} />
          <Route path="/shifts" element={<Shifts />} />
          <Route path="/storage" element={<Storage />} />
        </Routes>
      </div>
    </div>
  );
};

export default App;
