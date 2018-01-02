import React, { Component } from 'react';
import Paper from 'material-ui/Paper';
import Divider from 'material-ui/Divider';
import Menu from 'material-ui/Menu';
import MenuItem from 'material-ui/MenuItem';
import IconButton from 'material-ui/IconButton';
import RaisedButton from 'material-ui/RaisedButton';
import ChevronLeft from 'material-ui/svg-icons/navigation/chevron-left';
import { Toolbar, ToolbarGroup, ToolbarTitle } from 'material-ui/Toolbar';
import { Card, CardText } from 'material-ui/Card';
import { MicroserviceParam } from '.';

import './microservice-explorer.css';

export class MicroserviceExplorer extends Component {
  
    state = {
        requestUrl: "",
        requestBody: {},
        requestQuery: {},
        responseCode: 0,
        responseBody: {}
    }

    assignProps(props) {
        const { service, onCancel } = props;
        const paths = Object
            .getOwnPropertyNames(service.descriptor.paths)
            .map(path => Object
                .getOwnPropertyNames(service.descriptor.paths[path])
                .map(method => ({ method, path, key: `${method}|${path}` })))
            .reduce((a, b) => a.concat(b));

        this.setState({
            ...this.state,
            service,
            onCancel,
            paths,
            pathKey: paths[0].key,
            requestUrl: paths[0].path
        });
    }

    componentWillMount() {
        this.assignProps(this.props);
    }

    componentWillReceiveProps(props) {
        this.assignProps(props);
    }

    changePath(event, path) {
        this.setState({
            ...this.state,
            pathKey: path.key,
            requestUrl: path.path
        });
    }

    onChange(parameter, newValue) {
        if(parameter && parameter.in && parameter.in === 'query') {
            const requestQuery = { ...this.state.requestQuery };
            requestQuery[parameter.name] = newValue;
            this.setState({
                ...this.state,
                requestQuery
            })
        }
    }

    onCancel() {
        const { onCancel } =  this.state;
        if(onCancel) {
            onCancel();
        }
    }

    render() {
        const {
            service,
            paths,
            pathKey,
            requestUrl,
            requestQuery,
            requestBody,
            responseCode,
            responseBody
        } = this.state;

        const styles = {
            paper: { margin: '1em', padding: '1em 2em' },
            card: { minHeight: '200px' }
        };

        const queryKeys = Object.keys(requestQuery)
            .filter(key => requestQuery[key] !== null && requestQuery[key].toString().length > 0);

        const queryParams = queryKeys.length ?
            queryKeys.reduce((prev, curr) => prev + `${curr}=${requestQuery[curr]}`, "?") :
            "";

        const pathComponents = (
            <Menu onChange={(event, path) => this.changePath(event, path)}>
            {paths
                .sort((a, b) => a.path.localeCompare(b.path))
                .map(path => (
                    <MenuItem 
                        key={path.key}
                        value={path}
                        primaryText={path.path}
                        secondaryText={path.method.toUpperCase()} />
                ))}
            </Menu>
        );

        const path = paths.filter(path => path.key === pathKey)[0];
        const endpoint = service.descriptor.paths[path.path][path.method];

        const parameters = (endpoint.parameters || []).map((parameter, key) => (
            <MicroserviceParam
                key={key}
                meta={parameter}
                definitions={service.descriptor.definitions}
                onChange={newValue => this.onChange(parameter, newValue)}
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
                    <div className="request">
                        <h2>Request</h2>
                        <p>URL: {requestUrl}{queryParams}</p>
                        <Card style={styles.card}>
                            <CardText>{JSON.stringify(requestBody,null,2)}</CardText>
                        </Card>
                    </div> 
                    <div className="response">
                        <h2>Response</h2>
                        <p>StatusCode: {responseCode}</p>
                        <Card style={styles.card}>
                            <CardText>{JSON.stringify(responseBody,null,2)}</CardText>
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
                    <h1>{service.serviceName}</h1>
                    <h3>REST Version {service.version}</h3>
                    <p>{service.description}</p>
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
