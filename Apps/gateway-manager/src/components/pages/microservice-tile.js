import React, { Component } from 'react';
import Paper from 'material-ui/Paper';
import FlatButton from 'material-ui/FlatButton';
import Avatar from 'material-ui/Avatar';
import { GridTile } from 'material-ui/GridList';
import { Card, CardActions, CardHeader, CardText } from 'material-ui/Card';
import Done from 'material-ui/svg-icons/action/done';
import Close from 'material-ui/svg-icons/navigation/close';

import './microservice-tile.css';

export class MicroserviceTile extends Component {
  
  state = {
    expanded: true
  }

  assignProps(props) {
      this.setState({
        ...this.state,
        ...props
      });
  }

  componentWillMount() {
      this.assignProps(this.props);
  }

  componentWillReceiveProps(props) {
      this.assignProps(props);
  }

  handleExpandChange = (expanded) => {
    this.setState({
        ...this.state,
        expanded
    });
  };

  handleManage = (serviceName) => {
    const { onManage } = this.state;
    if(onManage) {
        onManage(serviceName);
    }
  }

  handleExplore = (serviceName) => {
    const { onExplore } = this.state;
    if(onExplore) {
        onExplore(serviceName);
    }
  }

  render() {
    const styles = {
        good: { color: '#690' },
        bad: { color: '#900' },
        paper: { margin: '1em' },
        badge: { background: '#900', top: 15, right: 15 }
    };
    const { service } = this.state;

    const passingServices = (service.instances || []).filter(instance => instance.health === 'passing');
    const criticalServices = (service.instances || []).filter(instance => instance.health === 'critical');

    const avatarChar = service.serviceName[0].toUpperCase();
    const avatar = (
        <Avatar size={40}>{avatarChar}</Avatar>
    );

    const instances = [
            ...passingServices, 
            ...criticalServices
        ].map(instance => {
            const icon = instance.health === 'passing' ? <Done style={styles.good} /> : <Close style={styles.bad} />;
            const classNames = `microservice-tile-instance ${instance.health}`;
            return (
                <div className={classNames} key={instance.instanceName} >
                    <div>{instance.instanceName}</div>
                    <div><small>@</small></div>
                    <div>{instance.nodeId}</div>
                    {icon}
                </div>
            );
        });

    const hasDescriptor = service.descriptor !== null;

    return (
        <GridTile>
            <Paper style={styles.paper} zDepth={3}>
                <Card expanded={this.state.expanded} onExpandChange={this.handleExpandChange}>
                    <CardHeader
                        title={service.serviceName}
                        subtitle={service.serviceDescription}
                        avatar={avatar}
                        actAsExpander={true}
                        showExpandableButton={true} />
                    <CardText expandable={true}>
                        {instances}
                    </CardText>
                    <CardActions>
                        <FlatButton label="Manage" onTouchTap={() => this.handleManage(service.serviceName)} disabled={true} />
                        <FlatButton label="Explore" onTouchTap={() => this.handleExplore(service.serviceName)} disabled={!hasDescriptor} />
                    </CardActions>
                </Card>
            </Paper>
        </GridTile>
    );
  }
}
