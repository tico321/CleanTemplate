import { createAsyncThunk } from '@reduxjs/toolkit';
import { todoService } from '../../../services';

// createAsyncThunk docs https://redux-toolkit.js.org/api/createAsyncThunk
const getTodoListsThunk = createAsyncThunk(
  'todos/getTodoLists',
  async (_, { rejectWithValue }) => {
    try {
      const response = await todoService.get();
      return response.data.result.todos;
    } catch (err) {
      if (!err.response) {
        throw err;
      }

      return rejectWithValue(err.response.data);
    }
  },
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
