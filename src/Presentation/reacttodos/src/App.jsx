import React from 'react';
import './App.css';
import { Provider } from 'react-redux';
import { ThemeProvider } from '@material-ui/core/styles';
import theme from './config/theme';
import AppRoutes from './Routes';
import store from './store';

function App() {
  return (
    <Provider store={store}>
      <ThemeProvider theme={theme}>
        <AppRoutes />
      </ThemeProvider>
    </Provider>
  );
}

export default App;
