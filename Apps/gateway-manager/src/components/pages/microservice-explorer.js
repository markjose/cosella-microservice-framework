import React, { Component } from 'react';
import Paper from 'material-ui/Paper';
import Divider from 'material-ui/Divider';
import Menu from 'material-ui/Menu';
import MenuItem from 'material-ui/MenuItem';
import IconButton from 'material-ui/IconButton';
import RaisedButton from 'material-ui/RaisedButton';
import TextField from 'material-ui/TextField';
import ChevronLeft from 'material-ui/svg-icons/navigation/chevron-left';
import { Toolbar, ToolbarGroup, ToolbarTitle } from 'material-ui/Toolbar';
import { Card, CardText } from 'material-ui/Card';

import './microservice-explorer.css';

export class MicroserviceExplorer extends Component {
  
    state = {
    }

    assignProps(props) {
        const { service, onCancel } = props;
        const paths = Object
            .getOwnPropertyNames(service.Descriptor.paths)
            .map(path => Object
                .getOwnPropertyNames(service.Descriptor.paths[path])
                .map(method => ({ method, path, key: `${method}|${path}` })))
            .reduce((a, b) => a.concat(b));

        this.setState({
            ...this.state,
            service,
            onCancel,
            paths,
            pathKey: paths[0].key
        });
    }

    componentWillMount() {
        this.assignProps(this.props);
    }

    componentWillReceiveProps(props) {
        this.assignProps(props);
    }

    changePath(event, pathKey) {
        this.setState({
            ...this.state,
            pathKey
        });
    }

    onCancel() {
        const { onCancel } =  this.state;
        if(onCancel) {
            onCancel();
        }
    }

    render() {
        const { service, paths, pathKey } = this.state;
        const styles = {
            paper: { margin: '1em', padding: '1em 2em' },
            card: { minHeight: '200px' }
        };

        const pathComponents = (
            <Menu onChange={(event, key) => this.changePath(event, key)}>
            {paths.map(path => (
                <MenuItem 
                    key={path.key}
                    value={path.key}
                    primaryText={`${path.method.toUpperCase()} ${path.path}`} />
            ))}
            </Menu>
        );

        const path = paths.filter(path => path.key === pathKey)[0];
        const endpoint = service.Descriptor.paths[path.path][path.method];

        const parameters = (endpoint.parameters || []).map(parameter => (
            <TextField
                key={parameter.name}
                hintText={parameter.required ? 'Required' : 'Optional'}
                floatingLabelText={parameter.name}
                fullWidth={true}
                />
        ));

        const content = (
            <div className="endpoint">
                <h1>{path.path}</h1>
                <Toolbar>
                    <ToolbarGroup>
                        <ToolbarTitle text={path.method.toUpperCase()} />
                    </ToolbarGroup>
                    <ToolbarGroup>
                        <RaisedButton label="Save request" disabled={true} />
                        <RaisedButton label="Send request" primary={true} />
                    </ToolbarGroup>
                </Toolbar>
                <div className="endpoint-metadata">
                    <div className="params">
                        <h2>Parameters</h2>
                        {parameters}    
                    </div>    
                    <div className="response">
                        <h2>Response</h2>
                        <Card style={styles.card}>
                            <CardText>
                                {'{}'}
                            </CardText>
                        </Card>
                    </div> 
                </div>
            </div>
        );

        return (
            <div className="microservice-explorer">
                <div className="sidebar">
                    <div className="sidebar-header">
                        <IconButton onTouchTap={() => this.onCancel()}>
                            <ChevronLeft />
                        </IconButton>
                        <h2>Microservice Explorer</h2>
                    </div>
                    <Divider />
                    <h1>{service.ServiceName}</h1>
                    <h3>Version {service.Version}</h3>
                    <p>{service.Description}</p>
                    <Divider />
                    {pathComponents}
                </div>
                <div className="content">
                    <Paper style={styles.paper} zDepth={3}>
                        {content}
                    </Paper>
                </div>
            </div>
        );
    }
}
