import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { makeStyles } from '@material-ui/core/styles';
import {
  IconButton, TableCell, TableRow, Typography, Box, Collapse,
} from '@material-ui/core';
import {
  KeyboardArrowDown, KeyboardArrowUp, Delete, Edit,
} from '@material-ui/icons';
import { connect } from 'react-redux';
import TodoListItems from './TodoListItems';
import { todoService } from '../../../services';
import { getTodoLists as getTodos } from '../../../store';
import DisplayIf from '../../../components/DisplayIf';
import EditTodoListForm from './EditTodoListForm';

const useStyles = makeStyles(() => ({
  nameContainer: {
    display: 'flex',
    alignItems: 'center',
  },
  remainingItems: {
    paddingLeft: '5px',
  },
  actionIcon: {
    float: 'right',
  },
}));

const TodoListRow = (props) => {
  const { todoList, triggerRefresh, descriptionValidation } = props;
  const classes = useStyles();
  const [open, setOpen] = useState(false);
  const [edit, setEdit] = useState(false);

  const startEditing = () => setEdit(true);
  const stopEditing = () => setEdit(false);
  const deleteItem = () => {
    todoService.deleteById(todoList.id)
      .then(() => triggerRefresh());
  };

  return (
    <>
      <TableRow
        hover
        key={todoList.id}
        selected={open}
      >
        <TableCell>
          <IconButton aria-label="expand row" size="small" onClick={() => setOpen(!open)}>
            {open ? <KeyboardArrowUp /> : <KeyboardArrowDown />}
          </IconButton>
        </TableCell>
        <TableCell>
          <DisplayIf condition={!edit}>
            <div className={classes.nameContainer}>
              <Typography variant="h5">{todoList.description}</Typography>
              <Typography className={classes.remainingItems} variant="body2">{`(${todoList.count})`}</Typography>
            </div>
          </DisplayIf>
          <DisplayIf condition={edit}>
            <div className={classes.nameContainer}>
              <EditTodoListForm
                id={todoList.id}
                descriptionValidation={descriptionValidation}
                onSuccess={stopEditing}
                onError={stopEditing}
              />
            </div>
          </DisplayIf>
        </TableCell>
        <TableCell>
          <IconButton className={classes.actionIcon} size="small" onClick={deleteItem}>
            <Delete />
          </IconButton>
          <IconButton className={classes.actionIcon} size="small" onClick={startEditing}>
            <Edit />
          </IconButton>
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
  descriptionValidation: PropTypes.instanceOf(Object).isRequired,
  triggerRefresh: PropTypes.func,
};

TodoListRow.defaultProps = {
  triggerRefresh: () => {},
};

const mapStateToProps = () => ({});

const mapDispatchToProps = (dispatch) => ({
  triggerRefresh: getTodos(dispatch),
});

export default connect(mapStateToProps, mapDispatchToProps)(TodoListRow);
