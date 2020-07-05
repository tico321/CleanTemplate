import React from 'react';
import PropTypes from 'prop-types';

const DisplayIf = ({ children, condition }) => {
  if (!condition) return null;
  return <>{children}</>;
};

DisplayIf.propTypes = {
  children: PropTypes.instanceOf(Object).isRequired,
  condition: PropTypes.bool.isRequired,
};

export default DisplayIf;
