import { createSlice } from '@reduxjs/toolkit';
import { getItemsByListIdReducer } from './actions';

export const initialState = {
  todoItems: {
    defaultId: {
      items: [],
      loadingState: 'pending',
      error: null,
    },
  },
};

const todoListItems = createSlice({
  name: 'TodoItems',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    getItemsByListIdReducer(builder);
  },
});

const todoListItemsReducer = todoListItems.reducer;
export default todoListItemsReducer;
