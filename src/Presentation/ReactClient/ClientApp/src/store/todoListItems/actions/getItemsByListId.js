import { createAsyncThunk } from '@reduxjs/toolkit';
import { todoService } from '../../../services';

// createAsyncThunk docs https://redux-toolkit.js.org/api/createAsyncThunk
const getItemsByListIdThunk = createAsyncThunk(
  'todos/items/byId',
  async (id, { rejectWithValue }) => {
    try {
      return await todoService.getById(id);
    } catch (e) {
      if (!e.response) throw e;

      return rejectWithValue(e.response);
    }
  },
);

export const getItemsByListIdReducer = (builder) => {
  builder.addCase(getItemsByListIdThunk.fulfilled, (state, action) => {
    const { id, todos } = action.payload;
    state.todoItems[`${id}`] = {
      items: todos,
      loadingState: 'fulfilled',
      error: null,
    };
  });
  builder.addCase(getItemsByListIdThunk.pending, (state, { meta: { arg } }) => {
    state.todoItems[`${arg}`] = state.todoItems.defaultId;
  });
  builder.addCase(getItemsByListIdThunk.rejected, (state, action) => {
    const { meta: { arg }, payload: { data } } = action;
    state.todoItems[`${arg}`] = {
      items: [],
      loadingState: 'rejected',
      error: data,
    };
  });
};

export const getItemsByListId = (dispatch) => (id) => dispatch(getItemsByListIdThunk(id));
