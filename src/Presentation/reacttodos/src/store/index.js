import { configureStore, getDefaultMiddleware } from '@reduxjs/toolkit';
import Todos from './todoList';

const middleware = [
  // default middleware https://redux-toolkit.js.org/api/getDefaultMiddleware#included-default-middleware
  ...getDefaultMiddleware(),
];

// const middleware = [thunk];

const store = configureStore({
  reducer: {
    Todos,
  },
  middleware,
});

export default store;
