import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { useForm } from 'react-hook-form';
import { makeStyles } from '@material-ui/core/styles';
import { TextField } from '@material-ui/core';
import { todoService } from '../../../services';
import { getTodoLists as getTodos } from '../../../store';

const useStyles = makeStyles(() => ({
  root: {
    display: 'flex',
    width: '100%',
  },
  flexGrow: {
    flexGrow: 1,
  },
}));

const EditTodoListForm = (props) => {
  const {
    register, handleSubmit, errors, reset,
  } = useForm();
  const {
    triggerRefresh, descriptionValidation, onSuccess, onError, id, todos,
  } = props;
  const classes = useStyles();
  const todo = todos.find((t) => t.id === id);

  const onSubmit = ({ todoListInput }) => {
    const updated = { ...todo, description: todoListInput };
    todoService
      .update(updated)
      .then(() => {
        reset();
        return triggerRefresh();
      })
      .then(onSuccess)
      .catch(onError);
  };

  const inputError = descriptionValidation.getErrorMessage(errors);
  const displayError = !!(errors && errors.todoListInput);

  return (
    <form className={classes.root} noValidate autoComplete="off" onSubmit={handleSubmit(onSubmit)}>
      <TextField
        name="todoListInput"
        label="Update the description"
        className={classes.flexGrow}
        error={displayError}
        helperText={inputError}
        inputRef={register(descriptionValidation.rules)}
        defaultValue={todo.description}
      />
    </form>
  );
};

EditTodoListForm.propTypes = {
  triggerRefresh: PropTypes.func,
  descriptionValidation: PropTypes.instanceOf(Object).isRequired,
  id: PropTypes.number.isRequired,
  todos: PropTypes.instanceOf(Array),
  onSuccess: PropTypes.func.isRequired,
  onError: PropTypes.func.isRequired,
};

EditTodoListForm.defaultProps = {
  triggerRefresh: () => {},
  todos: [],
};

const mapStateToProps = (state) => ({
  todos: state.Todos.todoLists,
});

const mapDispatchToProps = (dispatch) => ({
  triggerRefresh: getTodos(dispatch),
});

export default connect(mapStateToProps, mapDispatchToProps)(EditTodoListForm);
