import { Link } from 'react-router-dom';

const Home = () => {
  return (
    <div>
      {/* Hero Section */}
      <section className="relative h-[600px] bg-cover bg-center" 
               style={{ backgroundImage: "url('/images/hero-bg.jpg')" }}>
        <div className="absolute inset-0 bg-black bg-opacity-50"></div>
        <div className="relative container mx-auto px-4 h-full flex items-center">
          <div className="text-white max-w-2xl">
            <h1 className="text-5xl font-bold mb-4">
              Khám phá hương vị ẩm thực tuyệt vời
            </h1>
            <p className="text-xl mb-8">
              Chúng tôi mang đến cho bạn những món ăn ngon nhất, 
              được chế biến bởi những đầu bếp tài năng.
            </p>
            <div className="space-x-4">
              <Link
                to="/menu"
                className="bg-primary text-white px-8 py-3 rounded-lg hover:bg-primary-dark"
              >
                Xem thực đơn
              </Link>
              <Link
                to="/reservation"
                className="bg-white text-primary px-8 py-3 rounded-lg hover:bg-gray-100"
              >
                Đặt bàn ngay
              </Link>
            </div>
          </div>
        </div>
      </section>

      {/* Features Section */}
      <section className="py-16 bg-gray-50">
        <div className="container mx-auto px-4">
          <h2 className="text-3xl font-bold text-center mb-12">
            Tại sao chọn chúng tôi?
          </h2>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
            <div className="text-center">
              <div className="text-4xl text-primary mb-4">
                <i className="fas fa-utensils"></i>
              </div>
              <h3 className="text-xl font-bold mb-2">Ẩm thực đẳng cấp</h3>
              <p className="text-gray-600">
                Thực đơn phong phú với các món ăn được chế biến từ nguyên liệu tươi ngon
              </p>
            </div>
            <div className="text-center">
              <div className="text-4xl text-primary mb-4">
                <i className="fas fa-concierge-bell"></i>
              </div>
              <h3 className="text-xl font-bold mb-2">Dịch vụ chuyên nghiệp</h3>
              <p className="text-gray-600">
                Đội ngũ nhân viên nhiệt tình, chu đáo và chuyên nghiệp
              </p>
            </div>
            <div className="text-center">
              <div className="text-4xl text-primary mb-4">
                <i className="fas fa-glass-cheers"></i>
              </div>
              <h3 className="text-xl font-bold mb-2">Không gian sang trọng</h3>
              <p className="text-gray-600">
                Thiết kế hiện đại, không gian thoáng đãng và ấm cúng
              </p>
            </div>
          </div>
        </div>
      </section>

      {/* Special Dishes Section */}
      <section className="py-16">
        <div className="container mx-auto px-4">
          <h2 className="text-3xl font-bold text-center mb-12">
            Món ăn đặc biệt
          </h2>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
            {/* Add special dishes here */}
          </div>
        </div>
      </section>

      {/* Testimonials Section */}
      <section className="py-16 bg-gray-50">
        <div className="container mx-auto px-4">
          <h2 className="text-3xl font-bold text-center mb-12">
            Khách hàng nói gì về chúng tôi?
          </h2>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
            {/* Add testimonials here */}
          </div>
        </div>
      </section>
    </div>
  );
};

export default Home; 