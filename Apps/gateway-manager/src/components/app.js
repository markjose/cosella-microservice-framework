import React, { Component } from 'react';
import AppBar from 'material-ui/AppBar';
import Drawer from 'material-ui/Drawer';
import MenuItem from 'material-ui/MenuItem';
import Divider from 'material-ui/Divider';
import IconButton from 'material-ui/IconButton';
import ChevronLeft from 'material-ui/svg-icons/navigation/chevron-left';

import { MicroserviceList } from './pages';

import './app.css';

import logo from '../assets/logo.svg';

export class App extends Component {
  
  constructor(props) {
    super(props);
    this.state = {
      isDrawerOpen: false
    };
  }

  toggleDrawer() {
    this.setState({
      ...this.state,
      isDrawerOpen: !this.state.isDrawerOpen
    });
  }

  render() {
    const { isDrawerOpen } = this.state;

    const iconElementLeft = <IconButton><ChevronLeft /></IconButton>;

    return (
      <div className="app">
        <AppBar title="Gateway Manager" onLeftIconButtonTouchTap={() => this.toggleDrawer()}>
          <img src={logo} alt="Cosella - Gateway Manager" />
        </AppBar>
        <Drawer open={isDrawerOpen}>
          <AppBar title="Dashboard Menu" onLeftIconButtonTouchTap={() => this.toggleDrawer()} iconElementLeft={iconElementLeft} />
          <MenuItem disabled={true}>Available Microservices</MenuItem>
          <MenuItem disabled={true}>Available APIs</MenuItem>
          <Divider />
          <MenuItem>Logout</MenuItem>
        </Drawer>
        <div className="app-content">
          <MicroserviceList />
        </div>
      </div>
    );
  }
}
