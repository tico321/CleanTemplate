import todoApiService from '../config/api';

const service = {
  get: () => todoApiService.get('api/Todos'),
  create: (description) => todoApiService.post('api/Todos', { description }),
  getById: (id) => todoApiService.get(`api/Todos/${id}`),
  deleteById: (id) => todoApiService.delete(`api/Todos/${id}`),
  update: (todoList) => todoApiService.put(`api/Todos/${todoList.id}`, todoList),
  addItem: (id, item) => todoApiService.post(`api/Todos/${id}/Item`, item),
  getItem: (id, itemId) => todoApiService.get(`api/Todos/${id}/Item/${itemId}`),
  updateItem: (id, item) => todoApiService.put(`api/Todos/${id}/Item/${item.id}`, item),
  deleteItem: (id, itemId) => todoApiService.delete(`api/Todos/${id}/Item/${itemId}`),
};

export default service;
