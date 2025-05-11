import axios from "axios";

const axiosInstance = axios.create({
  baseURL: "http://localhost:5078/api", // Đã cập nhật để phù hợp với backend API
  headers: {
    Accept: "application/json",
  },
  withCredentials: true,
});

axiosInstance.interceptors.request.use(
  (config) => {
    // Log request for debugging
    console.log("API Request:", {
      url: config.url,
      method: config.method,
      data: config.data,
    });

    // Không set Content-Type cho multipart/form-data
    if (config.data instanceof FormData) {
      delete config.headers["Content-Type"];
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Add interceptor to handle errors
axiosInstance.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response) {
      // Handle specific error responses
      console.error("Server error:", error.response.data);
    } else if (error.request) {
      // Handle network errors
      console.error("Network error:", error.request);
    }
    return Promise.reject(error);
  }
);

export default axiosInstance;
