import todoApiService from '../config/api';

const service = {
  get: () => todoApiService.get('api/Todos'),
  create: (description) => todoApiService.post('api/Todos', { description }),
  getById: (id) => todoApiService.get(`api/Todos/${id}`),
  deleteById: (id) => todoApiService.delete(`api/Todos/${id}`),
  update: (todoList) => todoApiService.put(`api/Todos/${todoList.id}`, todoList),
};

export default service;
