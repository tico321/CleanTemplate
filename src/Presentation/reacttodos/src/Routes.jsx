import React from 'react';
import {
  Switch, Redirect, BrowserRouter,
} from 'react-router-dom';
import RouteWithLayout from './components/RouteWithLayout';
import Welcome from './pages/Welcome';
import Main from './layouts/Main/index';

function AppRoutes() {
  return (
    <BrowserRouter>
      <Switch>
        <Redirect exact from="/" to="/welcome" />
        <RouteWithLayout component={Welcome} layout={Main} path="/welcome" />
      </Switch>
    </BrowserRouter>
  );
}

export default AppRoutes;
