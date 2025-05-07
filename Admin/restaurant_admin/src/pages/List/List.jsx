import React, { useState } from 'react'
import './List.css'
import { toast } from 'react-toastify'
import axios from 'axios' //dung axios de goi api

const List = () => {

  const url = ""
  const [foodList, setFoodList] = useState([]);

  const fetchList = async () => {
    const response = await axios.get(`${url}/api/food/list`);
    if (response.data.success) {
      setFoodList(response.data.foods);
    } else {
      toast.error(response.data.message);
    }
  }


  return (
    <div className='list add flex-col'>
      <p>All Food List</p>
      <div className="list-table">
        <div className="list-table-format title">
          <b>Image</b>
          <b>Name</b>
          <b>Category</b>
          <b>Price</b>
          <b>Action</b>
        </div>
        {/* Làm danh sách mon an ở đây 
        <div className="list-table-format">
          <img src="" alt="" />
          <p>Food Name</p>
          <p>Category</p>
          <p>Price</p>
          <button>Edit</button>
          <button>Delete</button>

         */}
      </div>
    </div>
  )
}

export default List
