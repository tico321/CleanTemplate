import { createSlice } from '@reduxjs/toolkit';
import {
  signinRedirectReducer, signinCallbackReducer, getIdentityUserReducer, signoutRedirectReducer,
} from './actions';

export const initialState = {
  isLogged: false,
  signingRedirect: {},
  signingRedirectState: 'pending',
  callback: {},
  callbackState: 'idle',
  user: null,
  userState: 'idle',
};

const authSlice = createSlice({
  name: 'Auth',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    signinRedirectReducer(builder);
    signinCallbackReducer(builder);
    getIdentityUserReducer(builder);
    signoutRedirectReducer(builder);
  },
});

const authReducer = authSlice.reducer;
export default authReducer;
