import { createAsyncThunk } from '@reduxjs/toolkit';
import userManager from '../userManager';

// createAsyncThunk docs https://redux-toolkit.js.org/api/createAsyncThunk
const getUserThunk = createAsyncThunk(
  'auth/getIdentityUser',
  async (_, { rejectWithValue }) => {
    try {
      const user = await userManager.getUser();
      if (user) {
        const {
          // eslint-disable-next-line camelcase
          id_token, session_state, access_token, token_type, scope, expires_at, profile,
        } = user;
        const {
          // eslint-disable-next-line camelcase
          email, family_name, given_name, name,
        } = profile;

        return {
          idToken: id_token,
          sessionState: session_state,
          accessToken: access_token,
          tokenType: token_type,
          scope,
          expiresAt: expires_at,
          profile: {
            email,
            name,
            familyName: family_name,
            givenName: given_name,
          },
        };
      }
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
