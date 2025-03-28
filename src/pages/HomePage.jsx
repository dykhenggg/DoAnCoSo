import { Link } from "react-router-dom"

function HomePage() {
    return (
        <>
            <header>
                <nav>
                    <Link>
                        <img src="/images/5596751.jpg" alt="test pic" />
                        
                    </Link>
                    <div class="nav-links">
                        <ul>
                            <li><Link to="/">Home</Link></li>
                            <li><Link to="#menu">Menu</Link></li>
                            <li><Link to="#reservation">Reservation</Link></li>
                            <li><Link to="#gallery">Gallery</Link></li>
                            <li><Link to="/signin">Signin</Link></li>
                        </ul>
                    </div>
                </nav>

            </header>
            <div>
                <div class="text-box">
                    <h1>Welcome to</h1>
                    <h2>NHA HANG</h2>
                    <p>Enjoy the best FOOD in town, drink your favorite DRINK, spend time with your family in our COZY space, from SUNRISE to NIGHTFALL</p>
                    <p> Take a look at our <Link to="#menu" class="btn">MENU</Link></p>

                </div>

            </div>
            <footer>
                <div class="footer-section">
                    <h3>Contact</h3>
                    <p>Email: contact@nhahang.com</p>
                    <p>Phone: +123 456 789</p>
                </div>
                <div class="footer-section">
                    <h3>Find Us</h3>
                    <p>123 Restaurant St, Food City</p>
                </div>
                <div class="footer-section social-icons">
                    <h3>Follow Us</h3>
                    <Link to="https://facebook.com" target="_blank">Facebook <i class="fi fi-brands-facebook"></i></Link><br />
                    <Link to="https://instagram.com" target="_blank">Instagram <i class="fi fi-brands-instagram"></i></Link><br />
                    <Link to="https://youtube.com" target="_blank">Youtube <i class="fi fi-brands-youtube"></i></Link>
                </div>
                <div class="footer-section">
                    <h3>Opening Hours</h3>
                    <p>Mon-Fri: 10:00 AM - 10:00 PM</p>
                    <p>Sat-Sun: 11:00 AM - 11:00 PM</p>
                </div>
                <div class="footer-section">
                    <h3>Other</h3>
                    <p>Reservations available</p>
                    <p>Gift cards</p>
                </div>
            </footer>
        
            </>
        )
}

export default HomePage