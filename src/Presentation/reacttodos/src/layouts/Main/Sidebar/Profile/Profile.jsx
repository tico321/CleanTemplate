import React from 'react';
import { connect } from 'react-redux';
import { Link as RouterLink, Redirect } from 'react-router-dom';
import clsx from 'clsx';
import PropTypes from 'prop-types';
import { makeStyles } from '@material-ui/core/styles';
import { Avatar, Typography } from '@material-ui/core';

const useStyles = makeStyles((theme) => ({
  root: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    minHeight: 'fit-content',
  },
  avatar: {
    width: 60,
    height: 60,
  },
  name: {
    marginTop: theme.spacing(1),
  },
}));

const Profile = (props) => {
  const {
    className, isLogged, user, ...rest
  } = props;
  const classes = useStyles();

  if (!isLogged) return <Redirect to="/" />;

  return (
    <div
      {...rest}
      className={clsx(classes.root, className)}
    >
      <Avatar
        alt="Person"
        className={classes.avatar}
        component={RouterLink}
        src="https://thispersondoesnotexist.com/image"
        to="/settings"
      />
      <Typography
        className={classes.name}
        variant="h4"
      >
        {user.profile.name}
      </Typography>
      <Typography variant="body2">{user.profile.email}</Typography>
    </div>
  );
};

Profile.propTypes = {
  className: PropTypes.string,
  isLogged: PropTypes.bool,
  user: PropTypes.instanceOf(Object),
};

Profile.defaultProps = {
  className: '',
  isLogged: false,
  user: {},
};

const mapStateToProps = (state) => ({
  isLogged: state.Auth.isLogged,
  user: state.Auth.user,
});

export default connect(mapStateToProps, null)(Profile);
