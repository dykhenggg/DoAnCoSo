import React from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import './EmployeeDetails.css';

const EmployeeDetails = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  // This would typically come from an API call
  const employee = {
    id: id,
    name: "John Smith",
    position: "Chef",
    email: "john.smith@restaurant.com",
    phone: "+1 (555) 123-4567",
    hireDate: "2023-01-15",
    department: "Kitchen",
    address: "123 Main St, City, State",
    emergencyContact: "Jane Smith (Spouse) - +1 (555) 987-6543"
  };

  return (
    <div className="employee-details-container">
      <div className="details-header">
        <h2>Employee Details</h2>
        <button className="btn-back" onClick={() => navigate('/employees')}>
          Back to List
        </button>
      </div>
      
      <div className="details-content">
        <div className="details-section">
          <h3>Personal Information</h3>
          <div className="info-grid">
            <div className="info-item">
              <label>ID:</label>
              <span>{employee.id}</span>
            </div>
            <div className="info-item">
              <label>Name:</label>
              <span>{employee.name}</span>
            </div>
            <div className="info-item">
              <label>Position:</label>
              <span>{employee.position}</span>
            </div>
            <div className="info-item">
              <label>Department:</label>
              <span>{employee.department}</span>
            </div>
          </div>
        </div>

        <div className="details-section">
          <h3>Contact Information</h3>
          <div className="info-grid">
            <div className="info-item">
              <label>Email:</label>
              <span>{employee.email}</span>
            </div>
            <div className="info-item">
              <label>Phone:</label>
              <span>{employee.phone}</span>
            </div>
            <div className="info-item">
              <label>Address:</label>
              <span>{employee.address}</span>
            </div>
          </div>
        </div>

        <div className="details-section">
          <h3>Additional Information</h3>
          <div className="info-grid">
            <div className="info-item">
              <label>Hire Date:</label>
              <span>{employee.hireDate}</span>
            </div>
            <div className="info-item">
              <label>Emergency Contact:</label>
              <span>{employee.emergencyContact}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default EmployeeDetails; 