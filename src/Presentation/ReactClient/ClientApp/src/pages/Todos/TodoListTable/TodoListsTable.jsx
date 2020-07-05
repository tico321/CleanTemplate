import React, { useState } from 'react';
import { connect } from 'react-redux';
import clsx from 'clsx';
import PropTypes from 'prop-types';
import PerfectScrollbar from 'react-perfect-scrollbar';
import { makeStyles } from '@material-ui/core/styles';
import {
  Card,
  CardActions,
  CardContent,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  TablePagination,
} from '@material-ui/core';
import TodoListRow from './TodoListRow';
import AddTodoListForm from './AddTodoListForm';
import { todoService } from '../../../services';
import { getTodoLists as getTodos } from '../../../store';

const useStyles = makeStyles(() => ({
  root: {},
  content: {
    padding: 0,
  },
  inner: {
    minWidth: 1050,
  },
  nameContainer: {
    display: 'flex',
    alignItems: 'center',
  },
  actions: {
    justifyContent: 'flex-end',
  },
}));

const todoListDescriptionValidation = {
  rules: {
    required: true,
    minLength: 5,
  },
  getErrorMessage: (err) => {
    if (!err || !err.textInput || !err.textInput.type) return '';
    const { type } = err.textInput;
    if (type === 'required') return 'This field is required';
    if (type === 'minLength') return 'A proper description should have at least 5 characters';
    return '';
  },
};

const TodoListsTable = (props) => {
  const {
    className, todoLists, triggerRefresh, ...rest
  } = props;

  const classes = useStyles();
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [page, setPage] = useState(0);

  const handlePageChange = (event, newPage) => {
    setPage(newPage);
  };

  const handleRowsPerPageChange = (event) => {
    setRowsPerPage(event.target.value);
  };

  const onSubmit = ({ textInput, reset }) => {
    todoService
      .create(textInput)
      .then(() => {
        reset();
        return triggerRefresh();
      });
  };

  return (
    <Card
      {...rest}
      className={clsx(classes.root, className)}
    >
      <CardContent className={classes.content}>
        <PerfectScrollbar>
          <div className={classes.inner}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell padding="checkbox" />
                  <TableCell>Description</TableCell>
                  <TableCell align="right">Actions</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {todoLists.slice(0, rowsPerPage).map((todoList) => (
                  <TodoListRow
                    key={`${todoList.id}-table`}
                    todoList={todoList}
                    descriptionValidation={todoListDescriptionValidation}
                  />
                ))}
              </TableBody>
            </Table>
          </div>
        </PerfectScrollbar>
      </CardContent>
      <CardActions>
        <AddTodoListForm
          descriptionValidation={todoListDescriptionValidation}
          onSubmit={onSubmit}
        />
      </CardActions>
      <CardActions className={classes.actions}>
        <TablePagination
          component="div"
          count={todoLists.length}
          onChangePage={handlePageChange}
          onChangeRowsPerPage={handleRowsPerPageChange}
          page={page}
          rowsPerPage={rowsPerPage}
          rowsPerPageOptions={[5, 10, 25]}
        />
      </CardActions>
    </Card>
  );
};

TodoListsTable.propTypes = {
  className: PropTypes.string,
  todoLists: PropTypes.instanceOf(Array).isRequired,
  triggerRefresh: PropTypes.func,
};

TodoListsTable.defaultProps = {
  className: '',
  triggerRefresh: () => {},
};

const mapStateToProps = () => ({});

const mapDispatchToProps = (dispatch) => ({
  triggerRefresh: getTodos(dispatch),
});

export default connect(mapStateToProps, mapDispatchToProps)(TodoListsTable);
