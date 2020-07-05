import React, { useState } from 'react';
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
    if (!err || !err.todoListInput || !err.todoListInput.type) return '';
    const { type } = err.todoListInput;
    if (type === 'required') return 'This field is required';
    if (type === 'minLength') return 'A proper description should have at least 5 characters';
    return '';
  },
};

const TodoListsTable = (props) => {
  const { className, todoLists, ...rest } = props;

  const classes = useStyles();
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [page, setPage] = useState(0);

  const handlePageChange = (event, newPage) => {
    setPage(newPage);
  };

  const handleRowsPerPageChange = (event) => {
    setRowsPerPage(event.target.value);
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
                  <TodoListRow key={`${todoList.id}-table`} todoList={todoList} descriptionValidation={todoListDescriptionValidation} />
                ))}
              </TableBody>
            </Table>
          </div>
        </PerfectScrollbar>
      </CardContent>
      <CardActions>
        <AddTodoListForm descriptionValidation={todoListDescriptionValidation} />
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
};

TodoListsTable.defaultProps = {
  className: '',
};

export default TodoListsTable;
