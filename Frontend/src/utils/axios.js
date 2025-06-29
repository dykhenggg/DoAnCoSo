import axios from 'axios';

const api = axios.create({
    baseURL: 'http://localhost:5078/api',
    headers: {
        'Content-Type': 'application/json',
    },
    withCredentials: true
});

// Add a request interceptor
api.interceptors.request.use(
    (config) => {
        const token = window.localStorage.getItem('token');
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

// Add a response interceptor
api.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response?.status === 401) {
            // Handle unauthorized access
            window.localStorage.removeItem('token');
            window.localStorage.removeItem('user');
            window.location.href = '/';
        }
        return Promise.reject(error);
    }
);

export default api; 