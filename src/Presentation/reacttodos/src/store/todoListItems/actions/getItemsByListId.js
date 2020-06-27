import { createAsyncThunk } from '@reduxjs/toolkit';

// createAsyncThunk docs https://redux-toolkit.js.org/api/createAsyncThunk
const getItemsByListIdThunk = createAsyncThunk(
  'todos/items/byId',
  (id) => new Promise((resolve) => {
    const fakeTodo = {
      id,
      todos: [
        {
          id: 1, userId: '1', description: 'take out trash', state: 'Pending',
        },
        {
          id: 2, userId: '1', description: 'do my homework', state: 'Completed',
        },
      ],
    };
    setTimeout(() => {
      resolve(fakeTodo);
    },
    1);
  }),
);

export const getItemsByListIdReducer = (builder) => {
  builder.addCase(getItemsByListIdThunk.fulfilled, (state, action) => {
    const { id, todos } = action.payload;
    state.todoItems[`${id}`] = {
      items: todos,
      loadingState: 'fulfilled',
      error: null,
    };
    console.log(state.todoItems[`${id}`]);
  });
  builder.addCase(getItemsByListIdThunk.pending, (state) => {
    // todo
  });
  builder.addCase(getItemsByListIdThunk.rejected, (state, action) => {
    // todo
  });
};

export const getItemsByListId = (dispatch) => (id) => dispatch(getItemsByListIdThunk(id));
