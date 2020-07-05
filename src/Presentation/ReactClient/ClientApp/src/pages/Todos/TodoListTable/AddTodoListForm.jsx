import React from 'react';
import PropTypes from 'prop-types';
import { useForm } from 'react-hook-form';
import { IconButton, TextField, Container } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import { PlayArrow } from '@material-ui/icons';

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
    descriptionValidation, onSubmit,
  } = props;
  const classes = useStyles();

  const innerSubmit = ({ textInput }) => {
    onSubmit({ textInput, reset });
  };

  const inputError = descriptionValidation.getErrorMessage(errors);
  const displayError = !!(errors && errors.todoListInput);

  return (
    <Container>
      <form className={classes.root} noValidate autoComplete="off" onSubmit={handleSubmit(innerSubmit)}>
        <TextField
          name="textInput"
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
  onSubmit: PropTypes.func,
  descriptionValidation: PropTypes.instanceOf(Object).isRequired,
};

AddTodoListForm.defaultProps = {
  onSubmit: () => {},
};

export default AddTodoListForm;
