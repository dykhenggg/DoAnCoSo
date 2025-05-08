import { Link } from "react-router-dom";
import React from "react";

function HomePage() {
  const styles = {
    nav: {
      display: "flex",
      justifyContent: "space-between",
      alignItems: "center",
      padding: "20px 50px",
      backgroundColor: "#ffcc99",
    },
    logoSection: {
      flex: "0 0  auto",
    },
    menuSection: {
      flex: "1 1 auto",
      display: "flex",
      justifyContent: "flex-end",
    },
    navLinks: {
      listStyle: "none",
      display: "",
      gap: "30px",
      margin: 0,
      padding: 0,
      "@media (max-width: 768px)": {
        gap: "10px",
        marginTop: "15px",
      },
    },
    navItem: {
      textDecoration: "none",
      color: "#333",
      fontWeight: 500,
      padding: "8px 12px",
      borderRadius: "30px",
      transition: "all 0.3s ease",
    },
    navItemHover: {
      backgroundColor: "#ff914d",
      color: "white",
      transform: "translateY(-2px)",
    },
    logo: {
      height: "90px",
      width: "auto",
      "@media (max-width: 768px)": {
        height: "50px",
      },
    },
  };

  return (
    <>
      <header>
        <nav style={styles.nav}>
          <Link to="/">
            <img src="/src/assets/favicon.ico" alt="logo" style={styles.logo} />
          </Link>
          <div className="nav-links">
            <ul style={styles.navLinks}>
              <li>
                <HoverLink to="/" text="Home" />
              </li>
              <li>
                <HoverLink to="#menu" text="Menu" />
              </li>
              <li>
                <HoverLink to="#reservation" text="Reservation" />
              </li>
              <li>
                <HoverLink to="#gallery" text="Gallery" />
              </li>
              <li>
                <HoverLink to="/signin" text="Sign in" />
              </li>
            </ul>
          </div>
        </nav>
      </header>

      <div className="text-box">
        <h1>Welcome to</h1>
        <h2>NHA HANG</h2>
        <p>
          Enjoy the best FOOD in town, drink your favorite DRINK, spend time
          with your family in our COZY space, from SUNRISE to NIGHTFALL
        </p>
        <p>
          Take a look at our{" "}
          <Link to="#menu" className="btn">
            MENU
          </Link>
        </p>
      </div>

      <footer>
        <div className="footer-section">
          <h3>Contact</h3>
          <p>Email: contact@nhahang.com</p>
          <p>Phone: +123 456 789</p>
        </div>
        <div className="footer-section">
          <h3>Find Us</h3>
          <p>123 Restaurant St, Food City</p>
        </div>
        <div className="footer-section social-icons">
          <h3>Follow Us</h3>
          <a href="https://facebook.com" target="_blank">
            Facebook <i className="fi fi-brands-facebook"></i>
          </a>
          <br />
          <a href="https://instagram.com" target="_blank">
            Instagram <i className="fi fi-brands-instagram"></i>
          </a>
          <br />
          <a href="https://youtube.com" target="_blank">
            Youtube <i className="fi fi-brands-youtube"></i>
          </a>
        </div>
        <div className="footer-section">
          <h3>Opening Hours</h3>
          <p>Mon-Fri: 10:00 AM - 10:00 PM</p>
          <p>Sat-Sun: 11:00 AM - 11:00 PM</p>
        </div>
        <div className="footer-section">
          <h3>Other</h3>
          <p>Reservations available</p>
          <p>Gift cards</p>
        </div>
      </footer>
    </>
  );
}

// Component nhỏ để xử lý hiệu ứng hover bằng state
function HoverLink({ to, text }) {
  const [hovered, setHovered] = React.useState(false);

  const baseStyle = {
    textDecoration: "none",
    color: hovered ? "white" : "#333",
    fontWeight: 500,
    padding: "8px 12px",
    borderRadius: "6px",
    transition: "all 0.3s ease",
    backgroundColor: hovered ? "#ff914d" : "transparent",
    transform: hovered ? "translateY(-2px)" : "none",
  };

  return (
    <Link
      to={to}
      style={baseStyle}
      onMouseEnter={() => setHovered(true)}
      onMouseLeave={() => setHovered(false)}
    >
      {text}
    </Link>
  );
}

export default HomePage;
