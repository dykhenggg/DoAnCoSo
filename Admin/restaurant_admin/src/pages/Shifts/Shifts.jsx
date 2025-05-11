import React, { useState } from 'react';
import './Shifts.css';

const Shifts = () => {
  // Sample data - replace with actual data from your backend
  const [shifts, setShifts] = useState([
    { id: 1, beginHour: '08:00', endHour: '12:00' },
    { id: 2, beginHour: '12:00', endHour: '16:00' },
    { id: 3, beginHour: '16:00', endHour: '20:00' },
  ]);

  // State for modals
  const [showAddModal, setShowAddModal] = useState(false);
  const [showEditModal, setShowEditModal] = useState(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [selectedShift, setSelectedShift] = useState(null);
  const [newShift, setNewShift] = useState({ beginHour: '' });

  // Handle add shift
  const handleAdd = () => {
    setShowAddModal(true);
  };

  const handleAddSubmit = (e) => {
    e.preventDefault();
    const beginTime = new Date(`2000-01-01T${newShift.beginHour}`);
    const endTime = new Date(beginTime.getTime() + 4 * 60 * 60 * 1000);
    const endHour = endTime.toTimeString().slice(0, 5);

    const newShiftData = {
      id: shifts.length + 1,
      beginHour: newShift.beginHour,
      endHour: endHour
    };

    setShifts([...shifts, newShiftData]);
    setShowAddModal(false);
    setNewShift({ beginHour: '' });
  };

  // Handle edit shift
  const handleEdit = (shift) => {
    setSelectedShift(shift);
    setShowEditModal(true);
  };

  const handleEditSubmit = (e) => {
    e.preventDefault();
    const beginTime = new Date(`2000-01-01T${selectedShift.beginHour}`);
    const endTime = new Date(beginTime.getTime() + 4 * 60 * 60 * 1000);
    const endHour = endTime.toTimeString().slice(0, 5);

    const updatedShifts = shifts.map(shift => 
      shift.id === selectedShift.id 
        ? { ...shift, beginHour: selectedShift.beginHour, endHour: endHour }
        : shift
    );

    setShifts(updatedShifts);
    setShowEditModal(false);
    setSelectedShift(null);
  };

  // Handle delete shift
  const handleDelete = (shift) => {
    setSelectedShift(shift);
    setShowDeleteModal(true);
  };

  const handleDeleteConfirm = () => {
    setShifts(shifts.filter(shift => shift.id !== selectedShift.id));
    setShowDeleteModal(false);
    setSelectedShift(null);
  };

  return (
    <div className="shifts-container">
      <div className="shifts-header">
        <h2>Shift Management</h2>
        <button className="btn-add" onClick={handleAdd}>
          Add Shift
        </button>
      </div>

      <div className="shifts-grid">
        {shifts.map((shift) => (
          <div key={shift.id} className="shift-card">
            <div className="shift-info">
              <h3>Shift #{shift.id}</h3>
              <p>Begin: {shift.beginHour}</p>
              <p>End: {shift.endHour}</p>
            </div>
            <div className="shift-actions">
              <button 
                className="btn-edit"
                onClick={() => handleEdit(shift)}
              >
                Edit
              </button>
              <button 
                className="btn-delete"
                onClick={() => handleDelete(shift)}
              >
                Delete
              </button>
            </div>
          </div>
        ))}
      </div>

      {/* Add Shift Modal */}
      {showAddModal && (
        <div className="modal-overlay">
          <div className="modal-content">
            <h3>Add New Shift</h3>
            <form onSubmit={handleAddSubmit}>
              <div className="form-group">
                <label>Begin Hour:</label>
                <input
                  type="time"
                  value={newShift.beginHour}
                  onChange={(e) => setNewShift({ beginHour: e.target.value })}
                  required
                />
              </div>
              <div className="modal-actions">
                <button type="button" onClick={() => setShowAddModal(false)}>
                  Cancel
                </button>
                <button type="submit">Add Shift</button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Edit Shift Modal */}
      {showEditModal && selectedShift && (
        <div className="modal-overlay">
          <div className="modal-content">
            <h3>Edit Shift</h3>
            <form onSubmit={handleEditSubmit}>
              <div className="form-group">
                <label>Begin Hour:</label>
                <input
                  type="time"
                  value={selectedShift.beginHour}
                  onChange={(e) => setSelectedShift({
                    ...selectedShift,
                    beginHour: e.target.value
                  })}
                  required
                />
              </div>
              <div className="modal-actions">
                <button type="button" onClick={() => setShowEditModal(false)}>
                  Cancel
                </button>
                <button type="submit">Save Changes</button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Delete Confirmation Modal */}
      {showDeleteModal && selectedShift && (
        <div className="modal-overlay">
          <div className="modal-content">
            <h3>Delete Shift</h3>
            <p>Are you sure you want to delete Shift #{selectedShift.id}?</p>
            <div className="modal-actions">
              <button onClick={() => setShowDeleteModal(false)}>
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

export default Shifts;
