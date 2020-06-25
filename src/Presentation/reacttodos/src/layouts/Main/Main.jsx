import React, { useState } from 'react';
import PropTypes from 'prop-types';
import clsx from 'clsx';
import { makeStyles, useTheme } from '@material-ui/core/styles';
import { useMediaQuery } from '@material-ui/core';

import Footer from './Footer';
import Sidebar from './Sidebar';
import Topbar from './Topbar';

const useStyles = makeStyles((theme) => ({
  root: {
    paddingTop: 56,
    height: '100%',
    [theme.breakpoints.up('sm')]: {
      paddingTop: 64,
    },
  },
  shiftContent: {
    paddingLeft: 240,
  },
  content: {
    height: '100%',
  },
  hidden: {
    display: 'none',
  },
}));

const Main = (props) => {
  const { children } = props;

  const classes = useStyles();
  const theme = useTheme();
  const isDesktop = useMediaQuery(theme.breakpoints.up('lg'), {
    defaultMatches: true,
  });

  const [openSidebar, setOpenSidebar] = useState(isDesktop);
  const closeSidebar = () => setOpenSidebar(true);

  return (
    <div
      className={clsx({
        [classes.root]: true,
        [classes.shiftContent]: openSidebar,
      })}
    >
      <Topbar handleMenuClick={closeSidebar} displayMenu={openSidebar} />
      <Sidebar
        setOpen={setOpenSidebar}
        open={openSidebar}
        variant={isDesktop ? 'persistent' : 'temporary'}
      />
      <main className={classes.content}>
        {children}
        <Footer />
      </main>
    </div>
  );
};

Main.propTypes = {
  children: PropTypes.node.isRequired,
};

export default Main;
