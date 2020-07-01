import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { makeStyles } from '@material-ui/core/styles';
import {
  IconButton, TableCell, TableRow, Typography, Box, Collapse,
} from '@material-ui/core';
import KeyboardArrowDownIcon from '@material-ui/icons/KeyboardArrowDown';
import KeyboardArrowUpIcon from '@material-ui/icons/KeyboardArrowUp';
import TodoListItems from './TodoListItems';

const useStyles = makeStyles(() => ({
  nameContainer: {
    display: 'flex',
    alignItems: 'center',
  },
  remainingItems: {
    paddingLeft: '5px',
  },
}));

const TodoListRow = (props) => {
  const { todoList } = props;
  const classes = useStyles();
  const [open, setOpen] = useState(false);

  return (
    <>
      <TableRow
        hover
        key={todoList.id}
        selected={open}
      >
        <TableCell>
          <IconButton aria-label="expand row" size="small" onClick={() => setOpen(!open)}>
            {open ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
          </IconButton>
        </TableCell>
        <TableCell>
          <div className={classes.nameContainer}>
            <Typography variant="h5">{todoList.description}</Typography>
            <Typography className={classes.remainingItems} variant="body2">{`(${todoList.count})`}</Typography>
          </div>
        </TableCell>
      </TableRow>
      <TableRow key={`${todoList.id}-todoListItems`}>
        <TableCell style={{ paddingBottom: 0, paddingTop: 0 }} colSpan={6}>
          {/* Collapse produces warning and it's a know issue https://github.com/mui-org/material-ui/issues/13394 */}
          <Collapse in={open} timeout="auto" unmountOnExit>
            <Box margin={1}>
              <Typography variant="h6" gutterBottom component="div">
                Pending
              </Typography>
              <TodoListItems id={todoList.id} />
            </Box>
          </Collapse>
        </TableCell>
      </TableRow>
    </>
  );
};

TodoListRow.propTypes = {
  todoList: PropTypes.instanceOf(Object).isRequired,
};

export default TodoListRow;
