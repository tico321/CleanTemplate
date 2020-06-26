import React from 'react';
import clsx from 'clsx';
import PropTypes from 'prop-types';
import { makeStyles, useTheme } from '@material-ui/core/styles';
import { Divider, Drawer } from '@material-ui/core';
import DashboardIcon from '@material-ui/icons/Dashboard';
import PeopleIcon from '@material-ui/icons/People';
import Description from '@material-ui/icons/Description';
import AccountBoxIcon from '@material-ui/icons/AccountBox';
import SettingsIcon from '@material-ui/icons/Settings';
import IconButton from '@material-ui/core/IconButton';
import ChevronLeftIcon from '@material-ui/icons/ChevronLeft';
import ChevronRightIcon from '@material-ui/icons/ChevronRight';

import Profile from './Profile';
import SidebarNav from './SidebarNav';

const useStyles = makeStyles((theme) => ({
  drawer: {
    width: 240,
    [theme.breakpoints.up('lg')]: {
      marginTop: 64,
      height: 'calc(100% - 64px)',
    },
  },
  root: {
    backgroundColor: theme.palette.white,
    display: 'flex',
    flexDirection: 'column',
    height: '100%',
    padding: theme.spacing(2),
  },
  divider: {
    margin: theme.spacing(2, 0),
  },
  nav: {
    marginBottom: theme.spacing(2),
  },
}));

const Sidebar = (props) => {
  const {
    open, variant, setOpen, className, ...rest
  } = props;

  const handleDrawerClose = () => {
    setOpen(false);
  };

  const classes = useStyles();
  const theme = useTheme();

  const pages = [
    {
      title: 'Settings',
      href: '/settings',
      icon: <SettingsIcon />,
    },
    {
      title: 'Todos',
      href: '/todos',
      icon: <DashboardIcon />,
    },
    {
      title: 'Users',
      href: '/users',
      icon: <PeopleIcon />,
    },
    {
      title: 'Logs',
      href: '/logs',
      icon: <Description />,
    },
    {
      title: 'Account',
      href: '/account',
      icon: <AccountBoxIcon />,
    },
  ];

  return (
    <Drawer
      anchor="left"
      classes={{ paper: classes.drawer }}
      open={open}
      variant={variant}
    >
      <div className={classes.drawerHeader}>
        <IconButton onClick={handleDrawerClose}>
          {theme.direction === 'ltr' ? <ChevronLeftIcon /> : <ChevronRightIcon />}
        </IconButton>
      </div>
      <div
        {...rest}
        className={clsx(classes.root, className)}
      >
        <Profile />
        <Divider className={classes.divider} />
        <SidebarNav
          className={classes.nav}
          pages={pages}
        />
      </div>
    </Drawer>
  );
};

Sidebar.propTypes = {
  className: PropTypes.string,
  setOpen: PropTypes.func.isRequired,
  open: PropTypes.bool.isRequired,
  variant: PropTypes.string.isRequired,
};

Sidebar.defaultProps = {
  className: '',
};

export default Sidebar;
