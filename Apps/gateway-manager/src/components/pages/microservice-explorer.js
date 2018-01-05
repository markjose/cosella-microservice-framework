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
import { services } from '../../services/index';

export class MicroserviceExplorer extends Component {
  
    state = {
        requestUrlTemplate: "",
        requestUrlParams: {},
        requestBody: null,
        requestQuery: {},
        responseBody: null
    }

    assignProps(props) {
        const { service, onCancel } = props;

        if(service && service.descriptor) {
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
                requestUrlTemplate: paths[0].path
            });
        }
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
            requestUrlTemplate: path.path,
            requestQuery: {},
            requestBody: null,
            responseBody: null
    });
    }

    onChange(parameter, newValue) {
        console.log('onChange(parameter, newValue)', parameter, newValue);
        if(parameter) {

            parameter.value = newValue;

            if(parameter.in && parameter.in === 'query') {
                const requestQuery = { ...this.state.requestQuery };
                requestQuery[parameter.name] = parameter.value;
                this.setState({
                    ...this.state,
                    requestQuery,
                    responseBody: null
                })
            }

            if(parameter.in && parameter.in === 'path') {
                const requestUrlParams = { ...this.state.requestUrlParams };
                requestUrlParams[parameter.name] = parameter.value;
                this.setState({
                    ...this.state,
                    requestUrlParams,
                    responseBody: null
                })
            }
        }
    }

    onCancel() {
        const { onCancel } =  this.state;
        if(onCancel) {
            onCancel();
        }
    }

    sendRequest() {
        const {
            service,
            pathKey,
            requestUrlParams,
            requestQuery,
            requestBody
        } = this.state;

        services
            .proxyRequest(service.serviceName, this.templateReplace(pathKey, requestUrlParams), requestQuery, requestBody)
            .then(response => {
                this.setState({
                    ...this.state,
                    responseBody: response
                });
            })
            .catch(error => {
                this.setState({
                    ...this.state,
                    responseBody: {

                    }
                });
                console.warn('sendRequest', error);
            });
    }

    templateReplace(template, params) {
        return Object.keys(params)
            .filter(key => params[key] !== null && params[key].toString().length > 0)
            .reduce((prev, key) => prev.replace(`{${key}}`, params[key]), template);
    }

    render() {
        const {
            service,
            paths,
            pathKey,
            requestUrlTemplate,
            requestUrlParams,
            requestQuery,
            requestBody,
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

        const {statusCode, statusMessage, responseStatus} = responseBody || {};
        const statusCodeGroup = statusCode ? statusCode.toString()[0] : '0';

        const payloadOutput = responseBody 
            ? JSON.stringify(responseBody.payload || responseBody.error, null, 2)
            : ''

        const responseMeta = responseBody ? (
            <div className="response-metadata">
                <h4>Metadata</h4>
                <p>Full URL: {responseBody.requestUri}, Version: {responseBody.serviceVersion}</p>
                <p>Instance: {responseBody.serviceInstance}, Node: {responseBody.serviceNode}</p>
            </div>
        ) : null;

        const content = (
            <div className="endpoint">
                <h1>{path.path}</h1>
                <Toolbar>
                    <ToolbarGroup>
                        <ToolbarTitle text={path.method.toUpperCase()} />
                    </ToolbarGroup>
                    <ToolbarGroup>
                        <RaisedButton label="Save request" disabled={true} />
                        <RaisedButton label="Send request" primary={true} onClick={() => this.sendRequest()} />
                    </ToolbarGroup>
                </Toolbar>
                <div className="endpoint-metadata">
                    <div className="params">
                        <h2>Parameters</h2>
                        {parameters}    
                    </div>    
                    <div className="request">
                        <h2>Request</h2>
                        <p>URL: {this.templateReplace(requestUrlTemplate, requestUrlParams)}{queryParams}</p>
                        <Card style={styles.card}>
                            <CardText><pre>{requestBody ? JSON.stringify(requestBody,null,2) : ''}</pre></CardText>
                        </Card>
                    </div> 
                    <div className="response">
                        <h2>Response</h2>
                        <p>
                            Status Code:&nbsp;
                            <strong className={`status-${statusCodeGroup}`}>{statusCode}</strong>,
                            Status Message:&nbsp;
                            <strong className={`status-${statusCodeGroup}`}>{statusMessage || responseStatus}</strong>
                        </p>
                        <Card style={styles.card}>
                            <CardText><pre>{payloadOutput}</pre></CardText>
                        </Card>
                        {responseMeta}
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
