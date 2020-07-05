import React, { useEffect } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { Redirect } from 'react-router-dom';
import logo from '../../logo.svg';
import { getIdentityUser } from '../../store/auth/actions';

const Welcome = (props) => {
  const { isLogged, userState, getUser } = props;

  useEffect(() => {
    if (userState === 'idle') {
      getUser();
    }
  }, [userState, getUser]);

  if (isLogged) return <Redirect to="/todos" />;

  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit
          <code>src/App.js</code>
          and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React now
        </a>
      </header>
    </div>
  );
};

Welcome.propTypes = {
  isLogged: PropTypes.bool,
  userState: PropTypes.string,
  getUser: PropTypes.func,
};

Welcome.defaultProps = {
  getUser: () => {},
  isLogged: false,
  userState: 'idle',
};

const mapStateToProps = (state) => ({
  isLogged: state.Auth.isLogged,
  user: state.Auth.user,
  userState: state.Auth.userState,
});

const mapDispatchToProps = (dispatch) => ({
  getUser: getIdentityUser(dispatch),
});

export default connect(mapStateToProps, mapDispatchToProps)(Welcome);
