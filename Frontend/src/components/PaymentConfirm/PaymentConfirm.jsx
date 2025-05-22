import React from 'react';

const PaymentConfirm = ({ show, onClose }) => {
  if (!show) return null;
  return (
    <div className="payment-popup-backdrop" onClick={e => { if (e.target.className === 'payment-popup-backdrop') onClose(); }}>
      <div className="payment-popup-container">
        <button className="payment-popup-close" onClick={onClose}>×</button>
        <h2>Payment Confirmation</h2>
        <div style={{textAlign: 'center', margin: '20px 0'}}>
          <p>Thank you for using our service!</p>
          <p>We will process your order as soon as possible.<br/>Your order is being processed.</p>
        </div>
        <button className="payment-popup-confirm-button" onClick={onClose}>Close</button>
      </div>
    </div>
  );
};

export default PaymentConfirm;
