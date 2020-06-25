import React from 'react';
import { Route } from 'react-router-dom';
import PropTypes from 'prop-types';

const RouteWithLayout = (props) => {
  const { layout: Layout, component: Component, ...rest } = props;

  return (
    <Route
      {...rest}
      render={(matchProps) => (
        <Layout>
          <Component {...matchProps} />
        </Layout>
      )}
    />
  );
};

RouteWithLayout.propTypes = {
  path: PropTypes.string.isRequired,
  layout: PropTypes.any, // eslint-disable-line
  component: PropTypes.any, // eslint-disable-line
};

export default RouteWithLayout;
