import React, { useState } from "react";
import "./Home.css";
import Header from "../../components/Header/Header";
import Menu from "../../components/Menu/Menu";
import FoodDisplay from "../../components/FoodDisplay/FoodDisplay";
import AboutUs from "../../components/AboutUs/AboutUs";
import Footer from "../../components/Footer/Footer";
import Reservation from "../../components/Reservation/Reservation";

const Home = () => {
    const [category, setCategory] = useState("All");

    return (
        <div className="home">
            <Header />
            <Menu category={category} setCategory={setCategory} />
            <FoodDisplay category={category} />
            <AboutUs />
            <div id="reservation-section">
                <Reservation />
            </div>
        </div>
    );
}

export default Home;