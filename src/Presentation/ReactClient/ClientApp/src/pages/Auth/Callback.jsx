import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { Redirect } from 'react-router-dom';
import { signinCallback, getIdentityUser } from '../../store';

const Callback = (props) => {
  const {
    callbackState, triggerCallback, user, userState, getUser,
  } = props;
  const [redirect, setRedirect] = useState(false);

  // triggerCallback will return the auth token
  useEffect(() => {
    if (user || userState === 'fulfilled') return;
    if (callbackState === 'pending') return;
    if (callbackState === 'fulfilled') return;
    triggerCallback();
  },
  [user, userState, callbackState, triggerCallback]);

  // getUser set the userProfile
  useEffect(() => {
    if (user || userState === 'fulfilled') {
      setRedirect(true);
      return;
    }
    if (userState === 'pending') return;
    if (callbackState === 'fulfilled') {
      getUser();
    }
  },
  [user, userState, callbackState, getUser]);

  if (!redirect) return <div />;
  return <Redirect to="/todos" />;
};

Callback.propTypes = {
  triggerCallback: PropTypes.func,
  callbackState: PropTypes.string,
  user: PropTypes.instanceOf(Object),
  userState: PropTypes.string,
  getUser: PropTypes.func,
};

Callback.defaultProps = {
  triggerCallback: () => {},
  getUser: () => {},
  callbackState: 'idle',
  user: null,
  userState: 'idle',
};

const mapStateToProps = (state) => ({
  callback: state.Auth.callback,
  callbackState: state.Auth.callbackState,
  user: state.Auth.user,
  userState: state.Auth.userState,
});

const mapDispatchToProps = (dispatch) => ({
  triggerCallback: signinCallback(dispatch),
  getUser: getIdentityUser(dispatch),
});

export default connect(mapStateToProps, mapDispatchToProps)(Callback);
