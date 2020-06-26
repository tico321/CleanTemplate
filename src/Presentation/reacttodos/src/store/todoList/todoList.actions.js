export const GET_TODO_LISTS_REQUEST = 'GET_TODO_LISTS_REQUEST';
export const GET_TODO_LISTS_SUCCESS = 'GET_TODO_LISTS';

export const getTodoLists = (dispatch) => () => {
  dispatch({ type: GET_TODO_LISTS_REQUEST });

  const fakeTodos = [
    {
      id: 1, userId: '1', description: 'take out trash', displayOrder: 1, count: 1,
    },
    {
      id: 2, userId: '1', description: 'do my homework', displayOrder: 2, count: 0,
    },
  ];

  setTimeout(
    () => {
      dispatch({ type: GET_TODO_LISTS_SUCCESS, payload: fakeTodos });
    },
    2000,
  );
};
