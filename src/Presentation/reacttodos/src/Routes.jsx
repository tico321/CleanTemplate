import React from 'react';
import {
  Switch, Redirect, BrowserRouter, Route,
} from 'react-router-dom';
import RouteWithLayout from './components/RouteWithLayout';
import Main from './layouts/Main/index';
import { Welcome, NotFound } from './pages';

function AppRoutes() {
  return (
    <BrowserRouter>
      <Switch>
        <Redirect exact from="/" to="/welcome" />
        <RouteWithLayout component={Welcome} layout={Main} path="/welcome" />
        <Route path="/not-found">
          <NotFound />
        </Route>
        <Redirect to="/not-found" />
      </Switch>
    </BrowserRouter>
  );
}

export default AppRoutes;
