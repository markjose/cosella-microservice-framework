import React, { Component } from 'react';
import Paper from 'material-ui/Paper';
import Avatar from 'material-ui/Avatar';
import { Card, CardHeader, CardText } from 'material-ui/Card';

import './microservice-explorer.css';

export class MicroserviceExplorer extends Component {
  
    state = {}

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

    render() {
        const { service } = this.state;
        const styles = {
            paper: { margin: '1em' }
        };
        const avatar = (
            <Avatar size={40}>{service.ServiceName[0].toUpperCase()}</Avatar>
        );

        return (
            <div className="microservice-explorer">
                <Paper style={styles.paper} zDepth={3}>
                    <Card expanded={this.state.expanded} onExpandChange={this.handleExpandChange}>
                        <CardText>
                            <h1>{service.ServiceName}</h1>
                            <h2>Microservice Explorer</h2>
                        </CardText>
                    </Card>
                </Paper>
            </div>
        );
    }
}
