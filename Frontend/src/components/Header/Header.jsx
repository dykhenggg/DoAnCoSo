import React, { useState, useEffect } from "react";
import "./Header.css";

const Header = () => {
  const [currentSlide, setCurrentSlide] = useState(0);

  const slides = [
    {
      image: "/images/allen-rad-9G_oJBKwi1c-unsplash.jpg",
      title: "Gourmet Burgers",
      description:
        "Savor our handcrafted burgers made with premium ingredients",
      buttonText: "View Menu",
      buttonLink: "food-display",
    },
    {
      image: "/images/fastfood_background.jpg",
      title: "Fast & Fresh",
      description: "Quick service without compromising on quality",
      buttonText: "Order Now",
      buttonLink: "/order",
    },
    {
      image: "/images/header_img.png",
      title: "Healthy Choices",
      description: "Delicious and nutritious meals prepared with care",
      buttonText: "Explore Healthy Options",
      buttonLink: "/healthy-menu",
    },
  ];

  useEffect(() => {
    const timer = setInterval(() => {
      setCurrentSlide((prevSlide) => (prevSlide + 1) % slides.length);
    }, 7000); // Change slide every 5 seconds

    return () => clearInterval(timer);
  }, []);

  const handleScroll = (elementId) => {
    // Check if the link is for internal navigation
    if (!elementId.startsWith("/")) {
      const element = document.getElementById(elementId);
      if (element) {
        element.scrollIntoView({
          behavior: "smooth",
          block: "start",
        });
      }
    }
  };

  return (
    <div className="header">
      <div className="slideshow-container">
        {slides.map((slide, index) => (
          <div
            key={index}
            className={`slide ${index === currentSlide ? "active" : ""}`}
            style={{ backgroundImage: `url(${slide.image})` }}
          />
        ))}
      </div>
      <div className="header-contents">
        <div className="slide-content">
          <h2>{slides[currentSlide].title}</h2>
          <p>{slides[currentSlide].description}</p>
          <button
            className="cta-button"
            onClick={() => handleScroll(slides[currentSlide].buttonLink)}
          >
            {slides[currentSlide].buttonText}
          </button>
        </div>
        <div className="slide-indicators">
          {slides.map((_, index) => (
            <span
              key={index}
              className={`indicator ${index === currentSlide ? "active" : ""}`}
              onClick={() => setCurrentSlide(index)}
            />
          ))}
        </div>
      </div>
    </div>
  );
};

export default Header;
