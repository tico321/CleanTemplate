import { createAsyncThunk } from '@reduxjs/toolkit';

// createAsyncThunk docs https://redux-toolkit.js.org/api/createAsyncThunk
const getTodoListsThunk = createAsyncThunk(
  'todos/getTodoLists',
  () => new Promise((resolve) => {
    const fakeTodos = [
      {
        id: 1, userId: '1', description: 'take out trash', displayOrder: 1, count: 1,
      },
      {
        id: 2, userId: '1', description: 'do my homework', displayOrder: 2, count: 0,
      },
    ];
    setTimeout(() => {
      resolve(fakeTodos);
    },
    2000);
  }),
);

export const getTodoListsReducer = (builder) => {
  builder.addCase(getTodoListsThunk.fulfilled, (state, action) => {
    state.todoLists = action.payload;
    state.loadingTodoLists = false;
  });
  builder.addCase(getTodoListsThunk.pending, (state) => {
    state.loadingTodoLists = true;
  });
  builder.addCase(getTodoListsThunk.rejected, (state, action) => {
    state.todoLists = [];
    state.loadingTodoLists = false;
    state.error = action.payload;
  });
};

export const getTodoLists = (dispatch) => () => dispatch(getTodoListsThunk());
