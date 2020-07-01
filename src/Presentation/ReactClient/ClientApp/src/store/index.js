import { configureStore, getDefaultMiddleware } from '@reduxjs/toolkit';
import Todos from './todoList';
import TodoItems from './todoListItems';
import Auth from './auth';

const middleware = [
  // default middleware https://redux-toolkit.js.org/api/getDefaultMiddleware#included-default-middleware
  ...getDefaultMiddleware(),
];

// const middleware = [thunk];

const store = configureStore({
  reducer: {
    Todos,
    TodoItems,
    Auth,
  },
  middleware,
});

export default store;

export {
  signinRedirect, signinCallback, getIdentityUser, signoutRedirect,
} from './auth';
export { getTodoLists } from './todoList';
export { getItemsByListId } from './todoListItems';
