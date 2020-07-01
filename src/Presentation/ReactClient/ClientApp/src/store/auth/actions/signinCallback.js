import { createAsyncThunk } from '@reduxjs/toolkit';
import userManager from '../userManager';

// createAsyncThunk docs https://redux-toolkit.js.org/api/createAsyncThunk
const signinCallbackThunk = createAsyncThunk(
  'auth/signinCallback',
  async (_, { rejectWithValue }) => {
    try {
      return await userManager.signinRedirectCallback();
    } catch (err) {
      if (!err.response) {
        throw err;
      }

      return rejectWithValue(err.response);
    }
  },
);

export const signinCallbackReducer = (builder) => {
  builder.addCase(signinCallbackThunk.fulfilled, (state, action) => {
    state.callback = action.payload;
    state.callbackState = 'fulfilled';
  });
  builder.addCase(signinCallbackThunk.pending, (state) => {
    state.callbackState = 'pending';
  });
  builder.addCase(signinCallbackThunk.rejected, (state, action) => {
    state.callback = action.payload;
    state.callbackState = 'rejected';
  });
};

export const signinCallback = (dispatch) => () => dispatch(signinCallbackThunk());
