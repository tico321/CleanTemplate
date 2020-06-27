import { createAsyncThunk } from '@reduxjs/toolkit';

// createAsyncThunk docs https://redux-toolkit.js.org/api/createAsyncThunk
const getTodoListsThunk = createAsyncThunk(
  'todos/getTodoLists',
  () => new Promise((resolve) => {
    const fakeTodos = [
      {
        id: 1, userId: '1', description: 'Work', displayOrder: 1, count: 1,
      },
      {
        id: 2, userId: '1', description: 'Chores', displayOrder: 2, count: 0,
      },
    ];
    setTimeout(() => {
      resolve(fakeTodos);
    },
    1);
  }),
);

export const getTodoListsReducer = (builder) => {
  builder.addCase(getTodoListsThunk.fulfilled, (state, action) => {
    state.todoLists = action.payload;
    state.loadingState = 'fulfilled';
  });
  builder.addCase(getTodoListsThunk.pending, (state) => {
    state.loadingTodoLists = true;
    state.loadingState = 'pending';
  });
  builder.addCase(getTodoListsThunk.rejected, (state, action) => {
    state.todoLists = [];
    state.loadingState = 'rejected';
    state.error = action.payload;
  });
};

export const getTodoLists = (dispatch) => () => dispatch(getTodoListsThunk());
