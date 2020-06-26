import React, { useEffect } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import TodoListToolbar from './TodoListToolbar';
import TodoListsTable from './TodoListTable';
import { getTodoLists as getTodos } from '../../store/todoList';

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
  const { todoLists, getTodoLists } = props;

  useEffect(
    () => getTodoLists(),
    [],
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
};

Todos.defaultProps = {
  todoLists: [],
  getTodoLists: () => {},
};

const mapStateToProps = (state) => ({
  todoLists: state.Todos.todoLists,
});

const mapDispatchToProps = (dispatch) => ({
  getTodoLists: getTodos(dispatch),
});

export default connect(mapStateToProps, mapDispatchToProps)(Todos);
