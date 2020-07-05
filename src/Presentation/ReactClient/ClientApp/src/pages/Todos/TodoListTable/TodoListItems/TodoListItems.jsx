import React, { useEffect } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import clsx from 'clsx';
import {
  Table, TableBody, TableCell, TableHead, TableRow, Checkbox, IconButton,
} from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import { Delete } from '@material-ui/icons';
import { getItemsByListId } from '../../../../store';
import AddTodoListForm from '../AddTodoListForm';
import { todoService } from '../../../../services';

const todoDescriptionValidation = {
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

const useStyles = makeStyles(() => ({
  doneTodo: {
    textDecoration: 'line-through',
  },
  actionIcon: {
    float: 'right',
  },
}));

const TodoListItems = (props) => {
  const { id, getItems, todoItems } = props;
  const current = todoItems[id] || todoItems.defaultId;
  const todos = current.items;
  const classes = useStyles();
  useEffect(() => {
    if (current.loadingState === 'fulfilled') return;
    if (current.loadingState === 'rejected') return;
    if (current.loadingState === 'pending') return;
    getItems(id);
  },
  [id, current.loadingState, getItems]);
  const handleCheck = (event) => {
    const itemId = parseInt(event.target.value, 10);
    const item = todos.find((t) => t.id === itemId);
    const isCompleted = item.state === 'Completed';
    todoService
      .updateItem(id, {
        ...item, id, itemId: item.id, state: isCompleted ? 'Pending' : 'Completed',
      })
      .then(() => getItems(id))
      // eslint-disable-next-line no-console
      .catch(console.log);
  };

  const onSubmit = ({ textInput, reset }) => {
    todoService
      .addItem(id, { todoListId: id, description: textInput })
      .then(() => {
        reset();
        return getItems(id);
      });
  };

  const deleteItem = (itemId) => {
    todoService.deleteItem(id, itemId).then(() => getItems(id));
  };

  return (
    <>
      <Table size="small" aria-label="purchases">
        <TableHead>
          <TableRow>
            <TableCell padding="checkbox" />
            <TableCell>Description</TableCell>
            <TableCell align="right">Actions</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {todos.map((item) => (
            <TableRow key={`list-${id}-item-${item.id}`}>
              <TableCell>
                <Checkbox
                  checked={item.state === 'Completed'}
                  color="primary"
                  onChange={handleCheck}
                  value={item.id}
                />
              </TableCell>
              <TableCell
                component="th"
                scope="row"
                className={clsx({ [classes.doneTodo]: item.state === 'Completed' })}
              >
                {item.description}
              </TableCell>
              <TableCell>
                <IconButton className={classes.actionIcon} size="small" onClick={() => deleteItem(item.id)}>
                  <Delete />
                </IconButton>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
      <AddTodoListForm
        descriptionValidation={todoDescriptionValidation}
        onSubmit={onSubmit}
      />
    </>
  );
};

TodoListItems.propTypes = {
  id: PropTypes.number.isRequired,
  getItems: PropTypes.func,
  todoItems: PropTypes.instanceOf(Object),
};

TodoListItems.defaultProps = {
  getItems: () => {},
  todoItems: {
    defaultId: {
      items: [],
      loadingState: 'pending',
      error: null,
    },
  },
};

const mapStateToProps = (state) => ({
  todoItems: state.TodoItems.todoItems,
});

const mapDispatchToProps = (dispatch) => ({
  getItems: getItemsByListId(dispatch),
});

export default connect(mapStateToProps, mapDispatchToProps)(TodoListItems);
