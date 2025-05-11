import React, { useState } from 'react'
import './List.css'
import { toast } from 'react-toastify'
import axios from 'axios' //dung axios de goi api
import { food_list } from '../../assets/assets1'

const List = () => {
  const [foods, setFoods] = useState(food_list)
  const [showAddModal, setShowAddModal] = useState(false)
  const [showDeleteModal, setShowDeleteModal] = useState(false)
  const [showDetailModal, setShowDetailModal] = useState(false)
  const [selectedFood, setSelectedFood] = useState(null)
  const [newFood, setNewFood] = useState({
    name: '',
    price: '',
    description: '',
    category: ''
  })

  const handleDelete = (food) => {
    setSelectedFood(food)
    setShowDeleteModal(true)
  }

  const handleDeleteConfirm = () => {
    setFoods(foods.filter(food => food._id !== selectedFood._id))
    setShowDeleteModal(false)
    setSelectedFood(null)
  }

  const handleDetail = (food) => {
    setSelectedFood(food)
    setShowDetailModal(true)
  }

  return (
    <div className="list-container">
      <div className="list-header">
        <h2>Food List</h2>
      </div>

      <div className="foods-grid">
        {foods.map((food) => (
          <div key={food._id} className="food-card">
            <div className="food-image">
              <img src={food.image} alt={food.name} />
            </div>
            <div className="food-info">
              <h3>{food.name}</h3>
              <p className="food-category">{food.category}</p>
              <p className="food-price">${food.price}</p>
              <p className="food-description">{food.description}</p>
            </div>
            <div className="food-actions">
              <button 
                className="btn-detail"
                onClick={() => handleDetail(food)}
              >
                Details
              </button>
              <button 
                className="btn-delete"
                onClick={() => handleDelete(food)}
              >
                Delete
              </button>
            </div>
          </div>
        ))}
      </div>

      {/* chuc nang xem detail */}
      {showDetailModal && selectedFood && (
        <div className="modal-overlay">
          <div className="modal-content detail-modal">
            <h3>Food Details</h3>
            <div className="detail-content">
              <div className="detail-image">
                <img src={selectedFood.image} alt={selectedFood.name} />
              </div>
              <div className="detail-info">
                <h4>{selectedFood.name}</h4>
                <p className="detail-category">Category: {selectedFood.category}</p>
                <p className="detail-price">Price: ${selectedFood.price}</p>
                <p className="detail-description">{selectedFood.description}</p>
                <p className="detail-id">ID: {selectedFood._id}</p>
              </div>
            </div>
            <div className="modal-actions">
              <button onClick={() => setShowDetailModal(false)}>
                Close
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Delete Modal */}
      {showDeleteModal && selectedFood && (
        <div className="modal-overlay">
          <div className="modal-content">
            <h3>Delete Food</h3>
            <p>Are you sure you want to delete {selectedFood.name}?</p>
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
  )
}

export default List
