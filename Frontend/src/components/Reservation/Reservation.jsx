import React, { useState } from 'react';
import './Reservation.css';

const ReservationConfirm = ({ show, onClose }) => {
  if (!show) return null;
  return (
    <div className="reservation-popup-backdrop" onClick={e => { if (e.target.className === 'reservation-popup-backdrop') onClose(); }}>
      <div className="reservation-popup-container">
        <button className="reservation-popup-close" onClick={onClose}>×</button>
        <div className="reservation-popup-content">
          <h2>Reservation Received</h2>
          <p>We got your reservation suggestion!<br/>We will check and inform you via your email or phone number.</p>
          <button className="reservation-popup-confirm-button" onClick={onClose}>OK</button>
        </div>
      </div>
    </div>
  );
};

const Reservation = () => {
  const [form, setForm] = useState({
    name: '',
    phone: '',
    address: '',
    day: '',
    hourStart: '',
    hourEnd: '',
    people: 1,
  });
  const [showConfirm, setShowConfirm] = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    setShowConfirm(true);
  };

  return (
    <div className="reservation-container">
      <h2>Book a Table</h2>
      <form className="reservation-form" onSubmit={handleSubmit} autoComplete="off">
        <div className="reservation-form-row">
          <label>
            Name
            <input
              type="text"
              name="name"
              value={form.name}
              onChange={handleChange}
              placeholder="Your full name"
              required
            />
          </label>
          <label>
            Phone Number
            <input
              type="tel"
              name="phone"
              value={form.phone}
              onChange={handleChange}
              placeholder="e.g. 0123456789"
              pattern="[0-9]{10,12}"
              required
            />
          </label>
        </div>
        <label>
          Address
          <input
            type="text"
            name="address"
            value={form.address}
            onChange={handleChange}
            placeholder="Street, District, Ward"
            required
          />
        </label>
        <div className="reservation-form-row">
          <label>
            Day
            <input
              type="date"
              name="day"
              value={form.day}
              onChange={handleChange}
              required
            />
          </label>
          <label>
            Hour Start
            <input
              type="time"
              name="hourStart"
              value={form.hourStart}
              onChange={handleChange}
              required
            />
          </label>
          <label>
            Hour End
            <input
              type="time"
              name="hourEnd"
              value={form.hourEnd}
              onChange={handleChange}
              required
            />
          </label>
          <label>
            Number of People
            <input
              type="number"
              name="people"
              min="1"
              value={form.people}
              onChange={handleChange}
              required
            />
          </label>
        </div>
        <button type="submit" className="reservation-submit-btn">Reserve</button>
      </form>
      <ReservationConfirm show={showConfirm} onClose={() => setShowConfirm(false)} />
    </div>
  );
};

export default Reservation;