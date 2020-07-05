import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { useForm } from 'react-hook-form';
import { IconButton, TextField, Container } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import { PlayArrow } from '@material-ui/icons';
import { todoService } from '../../../services';
import { getTodoLists as getTodos } from '../../../store';

const useStyles = makeStyles((theme) => ({
  root: {
    display: 'flex',
  },
  flexGrow: {
    flexGrow: 1,
  },
  submitButton: {
    marginRight: theme.spacing(2),
  },
}));

const AddTodoListForm = (props) => {
  const {
    register, handleSubmit, errors, reset,
  } = useForm();
  const {
    triggerRefresh, descriptionValidation,
  } = props;
  const classes = useStyles();

  const onSubmit = ({ todoListInput }) => {
    todoService
      .create(todoListInput)
      .then(() => {
        reset();
        return triggerRefresh();
      });
  };

  const inputError = descriptionValidation.getErrorMessage(errors);
  const displayError = !!(errors && errors.todoListInput);

  return (
    <Container>
      <form className={classes.root} noValidate autoComplete="off" onSubmit={handleSubmit(onSubmit)}>
        <TextField
          name="todoListInput"
          label="Add new todo list"
          className={classes.flexGrow}
          error={displayError}
          helperText={inputError}
          inputRef={register(descriptionValidation.rules)}
        />
        <IconButton type="submit" edge="start" className={classes.submitButton} color="inherit" aria-label="menu">
          <PlayArrow />
        </IconButton>
      </form>
    </Container>
  );
};

AddTodoListForm.propTypes = {
  triggerRefresh: PropTypes.func,
  descriptionValidation: PropTypes.instanceOf(Object).isRequired,
};

AddTodoListForm.defaultProps = {
  triggerRefresh: () => {},
};

const mapStateToProps = () => ({});

const mapDispatchToProps = (dispatch) => ({
  triggerRefresh: getTodos(dispatch),
});

export default connect(mapStateToProps, mapDispatchToProps)(AddTodoListForm);
