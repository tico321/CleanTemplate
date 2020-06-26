import { GET_TODO_LISTS_REQUEST, GET_TODO_LISTS_SUCCESS } from './todoList.actions';

const initialState = {
  todoLists: [],
  loadingTodoLists: false,
};

function todoListReducer(state = initialState, action) {
  if (action.type === GET_TODO_LISTS_REQUEST) {
    return { ...state, loadingTodoLists: true };
  }
  if (action.type === GET_TODO_LISTS_SUCCESS) {
    return { ...state, todoLists: action.payload, loadingTodoLists: false };
  }

  return state;
}

export default todoListReducer;
