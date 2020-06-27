import axios from 'axios';

const todoApiService = axios.create({
  baseURL: 'https://localhost:5001',
});

export default todoApiService;
