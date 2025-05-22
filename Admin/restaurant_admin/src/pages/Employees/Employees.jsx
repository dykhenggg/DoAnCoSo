import React, { useState } from "react";
import "./Employees.css";

const Employees = () => {
  const [selectedEmployee, setSelectedEmployee] = useState(null);
  const [showDetailsModal, setShowDetailsModal] = useState(false);
  const [showEditModal, setShowEditModal] = useState(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [showAddModal, setShowAddModal] = useState(false);

  const employees = [
    {
      id: 1,
      name: "John Smith",
      position: "Chef",
      email: "john.smith@restaurant.com",
      phone: "+1 (555) 123-4567",
      hireDate: "2023-01-15",
      department: "Kitchen",
      address: "123 Main St, City, State",
      emergencyContact: "Jane Smith (Spouse) - +1 (555) 987-6543",
    },
    {
      id: 2,
      name: "Sarah Johnson",
      position: "Server",
      email: "sarah.j@restaurant.com",
      phone: "+1 (555) 234-5678",
      hireDate: "2023-02-20",
      department: "Service",
      address: "456 Oak St, City, State",
      emergencyContact: "Tom Johnson (Spouse) - +1 (555) 876-5432",
    },
    {
      id: 3,
      name: "Michael Brown",
      position: "Manager",
      email: "michael.b@restaurant.com",
      phone: "+1 (555) 345-6789",
      hireDate: "2023-03-10",
      department: "Management",
      address: "789 Pine St, City, State",
      emergencyContact: "Lisa Brown (Spouse) - +1 (555) 765-4321",
    },
  ];

  const [newEmployee, setNewEmployee] = useState({
    name: "",
    position: "",
    email: "",
    phone: "",
    hireDate: "",
    department: "",
    address: "",
    emergencyContact: "",
  });

  const handleDetails = (employee) => {
    setSelectedEmployee(employee);
    setShowDetailsModal(true);
  };

  const handleEdit = (employee) => {
    setSelectedEmployee(employee);
    setShowEditModal(true);
  };

  const handleDelete = (employee) => {
    setSelectedEmployee(employee);
    setShowDeleteModal(true);
  };

  const handleAdd = () => {
    setNewEmployee({
      name: "",
      position: "",
      email: "",
      phone: "",
      hireDate: "",
      department: "",
      address: "",
      emergencyContact: "",
    });
    setShowAddModal(true);
  };

  const handleAddSubmit = (e) => {
    e.preventDefault();
    // Add API call to create new employee
    console.log("New employee:", newEmployee);
    setShowAddModal(false);
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    if (selectedEmployee) {
      setSelectedEmployee((prev) => ({
        ...prev,
        [name]: value,
      }));
    } else {
      setNewEmployee((prev) => ({
        ...prev,
        [name]: value,
      }));
    }
  };

  const handleEditSubmit = (e) => {
    e.preventDefault();
    // Add API call to update employee
    console.log("Updated employee:", selectedEmployee);
    setShowEditModal(false);
  };

  const handleDeleteConfirm = () => {
    // Add API call to delete employee
    console.log("Deleting employee:", selectedEmployee.id);
    setShowDeleteModal(false);
  };

  return (
    <div className="employees-container">
      <div className="employees-header">
        <h2>Employee List</h2>
      </div>
      <table className="employees-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Position</th>
            <th>Email</th>
            <th>Phone Number</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {employees.map((employee) => (
            <tr key={employee.id}>
              <td>{employee.id}</td>
              <td>{employee.name}</td>
              <td>{employee.position}</td>
              <td>{employee.email}</td>
              <td>{employee.phone}</td>
              <td className="action-buttons">
                <button
                  className="btn-details"
                  onClick={() => handleDetails(employee)}
                >
                  Details
                </button>
                <button
                  className="btn-edit"
                  onClick={() => handleEdit(employee)}
                >
                  Edit
                </button>
                <button
                  className="btn-delete"
                  onClick={() => handleDelete(employee)}
                >
                  Delete
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      <div className="floating-add-button">
        <button className="btn-add" onClick={handleAdd}>
          Add Employee
        </button>
      </div>

      {/* Cua so chuc nang add nhan vien */}
      {showAddModal && (
        <div className="modal-overlay">
          <div className="modal-content">
            <div className="modal-header">
              <h3>Add New Employee</h3>
              <button
                className="btn-close"
                onClick={() => setShowAddModal(false)}
              >
                ×
              </button>
            </div>
            <form onSubmit={handleAddSubmit} className="modal-body">
              <div className="form-section">
                <h4>Personal Information</h4>
                <div className="form-grid">
                  <div className="form-group">
                    <label htmlFor="name">Name</label>
                    <input
                      type="text"
                      id="name"
                      name="name"
                      value={newEmployee.name}
                      onChange={handleInputChange}
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="position">Position</label>
                    <input
                      type="text"
                      id="position"
                      name="position"
                      value={newEmployee.position}
                      onChange={handleInputChange}
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="department">Department</label>
                    <input
                      type="text"
                      id="department"
                      name="department"
                      value={newEmployee.department}
                      onChange={handleInputChange}
                      required
                    />
                  </div>
                </div>
              </div>
              <div className="form-section">
                <h4>Contact Information</h4>
                <div className="form-grid">
                  <div className="form-group">
                    <label htmlFor="email">Email</label>
                    <input
                      type="email"
                      id="email"
                      name="email"
                      value={newEmployee.email}
                      onChange={handleInputChange}
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="phone">Phone</label>
                    <input
                      type="tel"
                      id="phone"
                      name="phone"
                      value={newEmployee.phone}
                      onChange={handleInputChange}
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="address">Address</label>
                    <input
                      type="text"
                      id="address"
                      name="address"
                      value={newEmployee.address}
                      onChange={handleInputChange}
                      required
                    />
                  </div>
                </div>
              </div>
              <div className="form-section">
                <h4>Additional Information</h4>
                <div className="form-grid">
                  <div className="form-group">
                    <label htmlFor="hireDate">Hire Date</label>
                    <input
                      type="date"
                      id="hireDate"
                      name="hireDate"
                      value={newEmployee.hireDate}
                      onChange={handleInputChange}
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="emergencyContact">Emergency Contact</label>
                    <input
                      type="text"
                      id="emergencyContact"
                      name="emergencyContact"
                      value={newEmployee.emergencyContact}
                      onChange={handleInputChange}
                      required
                      placeholder="Name (Relationship) - Phone"
                    />
                  </div>
                </div>
              </div>
              <div className="modal-footer">
                <button
                  type="button"
                  className="btn-cancel"
                  onClick={() => setShowAddModal(false)}
                >
                  Cancel
                </button>
                <button type="submit" className="btn-save">
                  Add Employee
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* cua so chuc nang xem chi tiet nhan vien */}
      {showDetailsModal && selectedEmployee && (
        <div className="modal-overlay">
          <div className="modal-content">
            <div className="modal-header">
              <h3>Employee Details</h3>
              <button
                className="btn-close"
                onClick={() => setShowDetailsModal(false)}
              >
                ×
              </button>
            </div>
            <div className="modal-body">
              <div className="details-section">
                <h4>Personal Information</h4>
                <div className="info-grid">
                  <div className="info-item">
                    <label>ID:</label>
                    <span>{selectedEmployee.id}</span>
                  </div>
                  <div className="info-item">
                    <label>Name:</label>
                    <span>{selectedEmployee.name}</span>
                  </div>
                  <div className="info-item">
                    <label>Position:</label>
                    <span>{selectedEmployee.position}</span>
                  </div>
                  <div className="info-item">
                    <label>Department:</label>
                    <span>{selectedEmployee.department}</span>
                  </div>
                </div>
              </div>
              <div className="details-section">
                <h4>Contact Information</h4>
                <div className="info-grid">
                  <div className="info-item">
                    <label>Email:</label>
                    <span>{selectedEmployee.email}</span>
                  </div>
                  <div className="info-item">
                    <label>Phone:</label>
                    <span>{selectedEmployee.phone}</span>
                  </div>
                  <div className="info-item">
                    <label>Address:</label>
                    <span>{selectedEmployee.address}</span>
                  </div>
                </div>
              </div>
              <div className="details-section">
                <h4>Additional Information</h4>
                <div className="info-grid">
                  <div className="info-item">
                    <label>Hire Date:</label>
                    <span>{selectedEmployee.hireDate}</span>
                  </div>
                  <div className="info-item">
                    <label>Emergency Contact:</label>
                    <span>{selectedEmployee.emergencyContact}</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      )}

      {/* cua so chuc nang sua thong tin nhan vien */}
      {showEditModal && selectedEmployee && (
        <div className="modal-overlay">
          <div className="modal-content">
            <div className="modal-header">
              <h3>Edit Employee</h3>
              <button
                className="btn-close"
                onClick={() => setShowEditModal(false)}
              >
                ×
              </button>
            </div>
            <form onSubmit={handleEditSubmit} className="modal-body">
              <div className="form-section">
                <h4>Personal Information</h4>
                <div className="form-grid">
                  <div className="form-group">
                    <label htmlFor="name">Name</label>
                    <input
                      type="text"
                      id="name"
                      name="name"
                      value={selectedEmployee.name}
                      onChange={handleInputChange}
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="position">Position</label>
                    <input
                      type="text"
                      id="position"
                      name="position"
                      value={selectedEmployee.position}
                      onChange={handleInputChange}
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="department">Department</label>
                    <input
                      type="text"
                      id="department"
                      name="department"
                      value={selectedEmployee.department}
                      onChange={handleInputChange}
                      required
                    />
                  </div>
                </div>
              </div>
              <div className="form-section">
                <h4>Contact Information</h4>
                <div className="form-grid">
                  <div className="form-group">
                    <label htmlFor="email">Email</label>
                    <input
                      type="email"
                      id="email"
                      name="email"
                      value={selectedEmployee.email}
                      onChange={handleInputChange}
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="phone">Phone</label>
                    <input
                      type="tel"
                      id="phone"
                      name="phone"
                      value={selectedEmployee.phone}
                      onChange={handleInputChange}
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="address">Address</label>
                    <input
                      type="text"
                      id="address"
                      name="address"
                      value={selectedEmployee.address}
                      onChange={handleInputChange}
                      required
                    />
                  </div>
                </div>
              </div>
              <div className="form-section">
                <h4>Additional Information</h4>
                <div className="form-grid">
                  <div className="form-group">
                    <label htmlFor="hireDate">Hire Date</label>
                    <input
                      type="date"
                      id="hireDate"
                      name="hireDate"
                      value={selectedEmployee.hireDate}
                      onChange={handleInputChange}
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="emergencyContact">Emergency Contact</label>
                    <input
                      type="text"
                      id="emergencyContact"
                      name="emergencyContact"
                      value={selectedEmployee.emergencyContact}
                      onChange={handleInputChange}
                      required
                    />
                  </div>
                </div>
              </div>
              <div className="modal-footer">
                <button
                  type="button"
                  className="btn-cancel"
                  onClick={() => setShowEditModal(false)}
                >
                  Cancel
                </button>
                <button type="submit" className="btn-save">
                  Save Changes
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* cua so chuc nang xoa nhan vien */}
      {showDeleteModal && selectedEmployee && (
        <div className="modal-overlay">
          <div className="modal-content delete-modal">
            <div className="modal-header">
              <h3>Delete Employee</h3>
              <button
                className="btn-close"
                onClick={() => setShowDeleteModal(false)}
              >
                ×
              </button>
            </div>
            <div className="modal-body">
              <p>Are you sure you want to delete {selectedEmployee.name}?</p>
              <p>This action cannot be undone.</p>
            </div>
            <div className="modal-footer">
              <button
                className="btn-cancel"
                onClick={() => setShowDeleteModal(false)}
              >
                Cancel
              </button>
              <button className="btn-delete" onClick={handleDeleteConfirm}>
                Delete
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default Employees;
