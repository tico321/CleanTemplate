import React from 'react';
import {
  Switch, Redirect, BrowserRouter,
} from 'react-router-dom';
import RouteWithLayout from './components/RouteWithLayout';
import { Main, Minimal } from './layouts';
import { Welcome, NotFound, Todos } from './pages';

function AppRoutes() {
  return (
    <BrowserRouter>
      <Switch>
        <Redirect exact from="/" to="/welcome" />
        <RouteWithLayout component={Welcome} layout={Minimal} path="/welcome" />
        <RouteWithLayout component={NotFound} layout={Minimal} path="/not-found" />
        <RouteWithLayout component={Todos} layout={Main} path="/todos" />
        <Redirect to="/not-found" />
      </Switch>
    </BrowserRouter>
  );
}

export default AppRoutes;
