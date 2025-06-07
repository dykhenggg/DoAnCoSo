import React from "react";
import "./Home.css";
import Header from "../../components/Header/Header";
import AboutUs from "../../components/AboutUs/AboutUs";
import Footer from "../../components/Footer/Footer";
import Booking from "../../components/Booking/Booking";

const Home = () => {
  return (
    <div className="home">
      <Header />
      
      {/* Discount Banner */}
      <section className="discount-banner">
        <div className="container mx-auto px-4 py-8">
          <div className="bg-gradient-to-r from-blue-500 to-blue-600 rounded-lg shadow-lg p-6 text-center">
            <h2 className="text-3xl font-bold text-white mb-2">Ưu đãi đặc biệt!</h2>
            <p className="text-xl text-white mb-4">Giảm giá 20% khi đặt bàn kèm món ăn trước</p>
            <button 
              onClick={() => document.getElementById('reservation-section').scrollIntoView({ behavior: 'smooth' })}
              className="bg-white text-blue-600 px-8 py-3 rounded-full font-semibold hover:bg-blue-50 transition-colors"
            >
              Đặt bàn ngay
            </button>
          </div>
        </div>
      </section>
      
      {/* Booking Section */}
      <section id="reservation-section" className="py-16 bg-gradient-to-b from-gray-50 to-white">
        <div className="container mx-auto px-4">
          <div className="text-center mb-12">
            <h2 className="text-3xl font-bold text-gray-800 mb-4">Đặt bàn nhà hàng</h2>
            <p className="text-gray-600 max-w-2xl mx-auto">
              Đặt bàn trực tuyến để đảm bảo chỗ ngồi cho bạn và gia đình. Bạn có thể chọn đặt bàn đơn giản hoặc đặt trước món ăn yêu thích.
            </p>
          </div>
          <Booking />
        </div>
      </section>

      <AboutUs />
      <Footer />
    </div>
  );
};

export default Home;
