import { createAsyncThunk } from '@reduxjs/toolkit';
import userManager from '../userManager';

// createAsyncThunk docs https://redux-toolkit.js.org/api/createAsyncThunk
const getUserThunk = createAsyncThunk(
  'auth/getIdentityUser',
  async (_, { rejectWithValue }) => {
    try {
      const user = await userManager.getUser();
      if (user) return user;
    } catch (err) {
      if (!err.response) {
        throw err;
      }
      return rejectWithValue(err.response);
    }

    throw new Error('User not found');
  },
);

export const getIdentityUserReducer = (builder) => {
  builder.addCase(getUserThunk.fulfilled, (state, action) => {
    state.user = action.payload;
    state.userState = 'fulfilled';
    state.isLogged = true;
  });
  builder.addCase(getUserThunk.pending, (state) => {
    state.userState = 'pending';
  });
  builder.addCase(getUserThunk.rejected, (state) => {
    state.user = null;
    state.userState = 'rejected';
  });
};

export const getIdentityUser = (dispatch) => () => dispatch(getUserThunk());
