import React, { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import './EditEmployee.css';

const EditEmployee = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  // This would typically come from an API call
  const initialEmployee = {
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

  const [employee, setEmployee] = useState(initialEmployee);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setEmployee(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    // Add API call to update employee
    console.log('Updated employee:', employee);
    navigate('/employees');
  };

  return (
    <div className="edit-employee-container">
      <div className="edit-header">
        <h2>Edit Employee</h2>
        <button className="btn-back" onClick={() => navigate('/employees')}>
          Back to List
        </button>
      </div>

      <form onSubmit={handleSubmit} className="edit-form">
        <div className="form-section">
          <h3>Personal Information</h3>
          <div className="form-grid">
            <div className="form-group">
              <label htmlFor="name">Name</label>
              <input
                type="text"
                id="name"
                name="name"
                value={employee.name}
                onChange={handleChange}
                required
              />
            </div>
            <div className="form-group">
              <label htmlFor="position">Position</label>
              <input
                type="text"
                id="position"
                name="position"
                value={employee.position}
                onChange={handleChange}
                required
              />
            </div>
            <div className="form-group">
              <label htmlFor="department">Department</label>
              <input
                type="text"
                id="department"
                name="department"
                value={employee.department}
                onChange={handleChange}
                required
              />
            </div>
          </div>
        </div>

        <div className="form-section">
          <h3>Contact Information</h3>
          <div className="form-grid">
            <div className="form-group">
              <label htmlFor="email">Email</label>
              <input
                type="email"
                id="email"
                name="email"
                value={employee.email}
                onChange={handleChange}
                required
              />
            </div>
            <div className="form-group">
              <label htmlFor="phone">Phone</label>
              <input
                type="tel"
                id="phone"
                name="phone"
                value={employee.phone}
                onChange={handleChange}
                required
              />
            </div>
            <div className="form-group">
              <label htmlFor="address">Address</label>
              <input
                type="text"
                id="address"
                name="address"
                value={employee.address}
                onChange={handleChange}
                required
              />
            </div>
          </div>
        </div>

        <div className="form-section">
          <h3>Additional Information</h3>
          <div className="form-grid">
            <div className="form-group">
              <label htmlFor="hireDate">Hire Date</label>
              <input
                type="date"
                id="hireDate"
                name="hireDate"
                value={employee.hireDate}
                onChange={handleChange}
                required
              />
            </div>
            <div className="form-group">
              <label htmlFor="emergencyContact">Emergency Contact</label>
              <input
                type="text"
                id="emergencyContact"
                name="emergencyContact"
                value={employee.emergencyContact}
                onChange={handleChange}
                required
              />
            </div>
          </div>
        </div>

        <div className="form-actions">
          <button type="button" className="btn-cancel" onClick={() => navigate('/employees')}>
            Cancel
          </button>
          <button type="submit" className="btn-save">
            Save Changes
          </button>
        </div>
      </form>
    </div>
  );
};

export default EditEmployee; 