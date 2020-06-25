import React from 'react';
import { ThemeProvider } from '@material-ui/core/styles';
import './App.css';
import theme from './config/theme';
import AppRoutes from './Routes';

function App() {
  return (
    <ThemeProvider theme={theme}>
      <AppRoutes />
    </ThemeProvider>
  );
}

export default App;
