import React, { useEffect } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import {
  Table, TableBody, TableCell, TableHead, TableRow, Checkbox,
} from '@material-ui/core';
import { getItemsByListId } from '../../../../store';

const TodoListItems = (props) => {
  const { id, getItems, todoItems } = props;
  const current = todoItems[id] || todoItems.defaultId;
  const todos = current.items;
  useEffect(() => {
    if (current.loadingState === 'fulfilled') return;
    if (current.loadingState === 'rejected') return;
    getItems(id);
  },
  [id, current.loadingState, getItems]);

  const handleCheck = () => {};

  return (
    <Table size="small" aria-label="purchases">
      <TableHead>
        <TableRow>
          <TableCell padding="checkbox" />
          <TableCell>Description</TableCell>
        </TableRow>
      </TableHead>
      <TableBody>
        {todos.map((item) => (
          <TableRow key={`${id}-${item.id}`}>
            <TableCell>
              <Checkbox
                checked={item.state === 'Completed'}
                color="primary"
                onChange={handleCheck}
                value="true"
              />
            </TableCell>
            <TableCell component="th" scope="row">
              {item.description}
            </TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  );
};

TodoListItems.propTypes = {
  id: PropTypes.string.isRequired,
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
