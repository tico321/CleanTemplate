import React, { useEffect } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import TodoListToolbar from './TodoListToolbar';
import TodoListsTable from './TodoListTable';
import { getTodoLists as getTodos } from '../../store';

const useStyles = makeStyles((theme) => ({
  root: {
    padding: theme.spacing(3),
  },
  content: {
    marginTop: theme.spacing(2),
  },
}));

const Todos = (props) => {
  const classes = useStyles();
  const {
    todoLists, getTodoLists, loadingState, isLogged,
  } = props;

  useEffect(
    () => {
      if (!isLogged) return;
      if (loadingState === 'fulfilled') return;
      if (loadingState === 'rejected') return;
      if (loadingState === 'pending') return;
      getTodoLists();
    },
    [getTodoLists, loadingState, isLogged],
  );

  return (
    <div className={classes.root}>
      <TodoListToolbar />
      <div className={classes.content}>
        <TodoListsTable todoLists={todoLists} />
      </div>
    </div>
  );
};

Todos.propTypes = {
  todoLists: PropTypes.instanceOf(Array),
  getTodoLists: PropTypes.func,
  loadingState: PropTypes.string,
  isLogged: PropTypes.bool,
};

Todos.defaultProps = {
  todoLists: [],
  getTodoLists: () => {},
  loadingState: 'idle',
  isLogged: false,
};

const mapStateToProps = (state) => ({
  todoLists: state.Todos.todoLists,
  loadingState: state.Todos.loadingState,
  isLogged: state.Auth.isLogged,
});

const mapDispatchToProps = (dispatch) => ({
  getTodoLists: getTodos(dispatch),
});

export default connect(mapStateToProps, mapDispatchToProps)(Todos);
