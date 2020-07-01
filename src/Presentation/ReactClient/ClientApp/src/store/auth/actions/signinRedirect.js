import { createAsyncThunk } from '@reduxjs/toolkit';
import userManager from '../userManager';

// createAsyncThunk docs https://redux-toolkit.js.org/api/createAsyncThunk
const signinRedirectThunk = createAsyncThunk(
  'auth/signinRedirect',
  async (_, { rejectWithValue }) => {
    try {
      return await userManager.signinRedirect();
    } catch (err) {
      if (!err.response) {
        throw err;
      }

      return rejectWithValue(err.response.data);
    }
  },
);

export const signinRedirectReducer = (builder) => {
  builder.addCase(signinRedirectThunk.fulfilled, (state, action) => {
    state.signingRedirect = action.payload;
    state.signingRedirectState = 'fulfilled';
  });
  builder.addCase(signinRedirectThunk.pending, (state) => {
    state.signingRedirectState = 'pending';
  });
  builder.addCase(signinRedirectThunk.rejected, (state, action) => {
    state.signingRedirect = action.payload;
    state.signingRedirectState = 'rejected';
  });
};

export const signinRedirect = (dispatch) => () => dispatch(signinRedirectThunk());
