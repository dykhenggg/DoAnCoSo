import { useState } from "react";
import { Route, Routes } from "react-router-dom";
import Home from "./pages/Home/Home";
import Cart from "./pages/Cart/Cart";
import Navbar from "./components/Navbar/Navbar";
import Footer from "./components/Footer/Footer";
import LoginPopup from "./components/LoginPopup/LoginPopup";
import PlaceOrder from "./pages/PlaceOrder/PlaceOrder";
import Reservation from "./components/Reservation/Reservation";

function App() {
  const [showLogin, setShowLogin] = useState(false);

  return (
    <>
      {showLogin ? <LoginPopup setShowLogin={setShowLogin} /> : <></>}
      <div className="app">
        <Navbar setShowLogin={setShowLogin} />
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/cart" element={<Cart setShowLogin={setShowLogin} />} />
          <Route path="/order" element={<PlaceOrder />} />
          <Route path="/reservation" element={<Reservation />} />
        </Routes>
      </div>
      <Footer />
    </>
  );
}

export default App;
