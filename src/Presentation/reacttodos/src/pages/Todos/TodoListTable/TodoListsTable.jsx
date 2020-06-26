import React, { useState } from 'react';
import clsx from 'clsx';
import PropTypes from 'prop-types';
import PerfectScrollbar from 'react-perfect-scrollbar';
import { makeStyles } from '@material-ui/core/styles';
import {
  Card,
  CardActions,
  CardContent,
  Checkbox,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  Typography,
  TablePagination,
} from '@material-ui/core';

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

const TodoListsTable = (props) => {
  const { className, todoLists, ...rest } = props;

  const classes = useStyles();

  const [selectedTodos, setSelectedTodos] = useState([]);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [page, setPage] = useState(0);

  const handleSelectOne = (event, id) => {
    const selectedIndex = selectedTodos.indexOf(id);
    let newSelectedUsers = [];

    if (selectedIndex === -1) {
      newSelectedUsers = newSelectedUsers.concat(selectedTodos, id);
    } else if (selectedIndex === 0) {
      newSelectedUsers = newSelectedUsers.concat(selectedTodos.slice(1));
    } else if (selectedIndex === selectedTodos.length - 1) {
      newSelectedUsers = newSelectedUsers.concat(selectedTodos.slice(0, -1));
    } else if (selectedIndex > 0) {
      newSelectedUsers = newSelectedUsers.concat(
        selectedTodos.slice(0, selectedIndex),
        selectedTodos.slice(selectedIndex + 1),
      );
    }

    setSelectedTodos(newSelectedUsers);
  };

  const handlePageChange = (event, newPage) => {
    setPage(newPage);
  };

  const handleRowsPerPageChange = (event) => {
    setRowsPerPage(event.target.value);
  };

  const handleSelectAll = (event) => {
    console.log(event.target);
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
                  <TableCell padding="checkbox">
                    <Checkbox
                      checked={selectedTodos.length === todoLists.length}
                      color="primary"
                      indeterminate={
                        selectedTodos.length > 0
                        && selectedTodos.length < todoLists.length
                      }
                      onChange={handleSelectAll}
                    />
                  </TableCell>
                  <TableCell>Description</TableCell>
                  <TableCell>Items</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {todoLists.slice(0, rowsPerPage).map((todoList) => (
                  <TableRow
                    hover
                    key={todoList.id}
                    selected={selectedTodos.indexOf(todoList.id) !== -1}
                  >
                    <TableCell padding="checkbox">
                      <Checkbox
                        checked={selectedTodos.indexOf(todoList.id) !== -1}
                        color="primary"
                        onChange={(event) => handleSelectOne(event, todoList.id)}
                        value="true"
                      />
                    </TableCell>
                    <TableCell>
                      <div className={classes.nameContainer}>
                        <Typography variant="body1">{todoList.description}</Typography>
                      </div>
                    </TableCell>
                    <TableCell>{todoList.count}</TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </div>
        </PerfectScrollbar>
      </CardContent>
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
