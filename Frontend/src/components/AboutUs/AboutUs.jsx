import React, { useEffect, useRef } from 'react';
import './AboutUs.css';
import { assets } from '../../assets/assets';

const AboutUs = () => {
    const aboutRef = useRef(null);

    useEffect(() => {
        const observer = new IntersectionObserver(
            (entries) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        entry.target.classList.add('fade-in');
                    }
                });
            },
            { 
                threshold: 0.1,
                rootMargin: '0px 0px -100px 0px'
            }
        );

        if (aboutRef.current) {
            observer.observe(aboutRef.current);
        }

        // Handle navbar click animation
        const handleNavClick = () => {
            if (aboutRef.current) {
                aboutRef.current.classList.add('fade-in');
            }
        };

        window.addEventListener('aboutUsClick', handleNavClick);

        return () => {
            if (aboutRef.current) {
                observer.unobserve(aboutRef.current);
            }
            window.removeEventListener('aboutUsClick', handleNavClick);
        };
    }, []);

    return (
        <div id="about-us" className="about-us-container" ref={aboutRef}>
            <div className="about-content">
                <div className="about-text">
                    <h2>Our Story</h2>
                    <p>
                        Founded in 2020, our restaurant began as a dream shared by a group of passionate food enthusiasts. 
                        What started as a small family kitchen has blossomed into a culinary haven where tradition meets innovation.
                    </p>
                    <p>
                        Our journey began when our head chef, inspired by generations of family recipes and global culinary adventures, 
                        decided to create a space where food tells stories. Each dish we serve carries the legacy of our heritage 
                        while embracing modern culinary techniques.
                    </p>
                    <p>
                        Our team of dedicated chefs and staff brings together decades of combined experience, 
                        each contributing their unique perspective to create an unforgettable dining experience. 
                        We believe in using only the finest, locally-sourced ingredients, supporting our community 
                        while ensuring the highest quality in every bite.
                    </p>
                    <p>
                        Today, we continue to grow and evolve, always staying true to our core values of 
                        exceptional food, warm hospitality, and creating memorable moments for our guests.
                    </p>
                </div>
                <div className="about-image">
                    <img src={assets.about_us_chef} alt="Our Restaurant" />
                    <img src={assets.about_us_table} alt="Our Restaurant" />
                    <img src={assets.about_us_kitchen} alt="Our Restaurant" />
                </div>
            </div>
        </div>
    );
};

export default AboutUs; 