import {
  createStore, combineReducers, compose, applyMiddleware,
} from 'redux';
import thunk from 'redux-thunk';
import Todos from './todoList';

const storeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

const reducer = combineReducers({ Todos });

// const middleware = [thunk];

const store = createStore(
  reducer,
  storeEnhancers(applyMiddleware(thunk)),
);

export default store;
