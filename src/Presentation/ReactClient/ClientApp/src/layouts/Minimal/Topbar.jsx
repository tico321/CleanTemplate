import React from 'react';
import { Link as RouterLink } from 'react-router-dom';
import { connect } from 'react-redux';
import clsx from 'clsx';
import PropTypes from 'prop-types';
import { makeStyles } from '@material-ui/core/styles';
import { AppBar, Toolbar, Button } from '@material-ui/core';
import { signinRedirect } from '../../store';

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
    boxShadow: 'none',
  },
  menuButton: {
    marginRight: theme.spacing(2),
  },
  title: {
    flexGrow: 1,
  },
}));

const LoginButton = (props) => {
  const { isLogged, onClick } = props;
  if (isLogged) return null;
  return (
    <Button
      color="secondary"
      variant="contained"
      onClick={onClick}
    >
      Login
    </Button>
  );
};

LoginButton.propTypes = {
  isLogged: PropTypes.bool.isRequired,
  onClick: PropTypes.func.isRequired,
};

const Topbar = (props) => {
  const {
    className, isLogged, startSignin, ...rest
  } = props;

  const classes = useStyles();

  return (
    <AppBar
      {...rest}
      className={clsx(classes.root, className)}
      color="primary"
      position="fixed"
    >
      <Toolbar>
        <RouterLink to="/" className={classes.title}>
          <img
            alt="Logo"
            src="/images/logos/logo--white.svg"
          />
        </RouterLink>
        <LoginButton isLogged={isLogged} onClick={startSignin} />
      </Toolbar>
    </AppBar>
  );
};

Topbar.propTypes = {
  className: PropTypes.string,
  isLogged: PropTypes.bool,
  startSignin: PropTypes.func,
};

Topbar.defaultProps = {
  className: '',
  isLogged: false,
  startSignin: () => {},
};

const mapStateToProps = (state) => ({
  isLogged: state.Auth.isLogged,
});

const mapDispatchToProps = (dispatch) => ({
  startSignin: signinRedirect(dispatch),
});

export default connect(mapStateToProps, mapDispatchToProps)(Topbar);
