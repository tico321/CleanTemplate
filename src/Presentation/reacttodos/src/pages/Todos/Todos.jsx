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
  const { todoLists, getTodoLists, loadingState } = props;

  useEffect(
    () => {
      if (loadingState === 'fulfilled') return;
      getTodoLists();
    },
    [getTodoLists, loadingState],
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
};

Todos.defaultProps = {
  todoLists: [],
  getTodoLists: () => {},
  loadingState: 'pending',
};

const mapStateToProps = (state) => ({
  todoLists: state.Todos.todoLists,
  loadingState: state.Todos.loadingState,
});

const mapDispatchToProps = (dispatch) => ({
  getTodoLists: getTodos(dispatch),
});

export default connect(mapStateToProps, mapDispatchToProps)(Todos);
