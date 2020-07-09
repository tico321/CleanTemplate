import axios from 'axios';
import { todoServiceConfig } from '../config/api';

const todoApiService = axios.create({
  baseURL: todoServiceConfig.baseURL,
});

const service = {
  get: () => todoApiService.get('api/Todos').then((res) => res.data.result.todos),
  create: (description) => todoApiService.post('api/Todos', { description }),
  getById: (id) => todoApiService.get(`api/Todos/${id}`).then((res) => res.data.result),
  deleteById: (id) => todoApiService.delete(`api/Todos/${id}`),
  update: (todoList) => todoApiService.put(`api/Todos/${todoList.id}`, todoList),
  addItem: (id, item) => todoApiService.post(`api/Todos/${id}/Item`, item),
  getItem: (id, itemId) => todoApiService.get(`api/Todos/${id}/Item/${itemId}`),
  updateItem: (id, item) => todoApiService.put(`api/Todos/${id}/Item/${item.id}`, item),
  deleteItem: (id, itemId) => todoApiService.delete(`api/Todos/${id}/Item/${itemId}`),
  setToken: (token) => {
    axios.defaults.headers.common = { Authorization: `Bearer ${token}` };
  },
};

export default service;
