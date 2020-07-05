import { createAsyncThunk } from '@reduxjs/toolkit';
import userManager from '../userManager';

// createAsyncThunk docs https://redux-toolkit.js.org/api/createAsyncThunk
const signoutRedirectThunk = createAsyncThunk(
  'auth/signoutRedirect',
  async (_, { rejectWithValue }) => {
    try {
      return await userManager.signoutRedirect();
    } catch (err) {
      if (!err.response) {
        throw err;
      }

      return rejectWithValue(err.response.data);
    }
  },
);

export const signoutRedirectReducer = (builder) => {
  builder.addCase(signoutRedirectThunk.fulfilled, (state, action) => {
    state.signoutgRedirect = action.payload;
    state.signoutgRedirectState = 'fulfilled';
  });
  builder.addCase(signoutRedirectThunk.pending, (state) => {
    state.signoutgRedirectState = 'pending';
  });
  builder.addCase(signoutRedirectThunk.rejected, (state, action) => {
    state.signoutgRedirect = action.payload;
    state.signoutgRedirectState = 'rejected';
  });
};

export const signoutRedirect = (dispatch) => () => dispatch(signoutRedirectThunk());
