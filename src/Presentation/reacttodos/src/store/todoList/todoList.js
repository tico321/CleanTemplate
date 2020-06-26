import { createSlice } from '@reduxjs/toolkit';
import { getTodoListsReducer } from './actions';

export const initialState = {
  todoLists: [],
  loadingTodoLists: false,
  error: null,
};

const todoListSlice = createSlice({
  name: 'Todos',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    getTodoListsReducer(builder);
  },
});

const todoListReducer = todoListSlice.reducer;
export default todoListReducer;
