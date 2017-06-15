import React, { Component } from 'react';
import TextField from 'material-ui/TextField';
import Toggle from 'material-ui/Toggle';

import './microservice-param.css';

const definitionsRegex = /#\/definitions\/([a-zA-Z0-9]+)/g;

export class MicroserviceParam extends Component {
  
    state = {
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

    renderString() {
        const { meta } = this.state;
        return (
            <TextField
                hintText={meta.required ? 'Required' : 'Optional'}
                floatingLabelText={meta.name}
                fullWidth={true}
                errorText={meta.required ? 'Required' : null}
            />
        );
    }

    renderBoolean() {
        const { meta } = this.state;
        return (
            <Toggle label={meta.name} defaultToggled={false} />
        );
    }

    renderComplex() {
        const { meta, definitions } = this.state;

        const refs = definitionsRegex.exec(meta.schema['$ref']);
        const typeName = refs && refs.length > 1 ? refs[1] : "unknown";
        const definition = definitions[typeName];

        let properties = <pre>{JSON.stringify(definition, null, 2)}</pre>
        if(definition && definition.type && definition.type.toLowerCase() === 'object') {
            if(definition.properties) {
                properties = Object.keys(definition.properties).map((propertyName, key) => (
                    <MicroserviceParam
                        key={key}
                        meta={{...definition.properties[propertyName], name: propertyName}}
                        definitions={definitions} />
                ));
            }
        }

        return (
            <div>
                <h3>{meta.name} ({typeName})</h3>
                {properties}
            </div>
        );
    }

    render() {
        const { meta } = this.state;
        const styles = {
        };

        let output = (
            <pre>{JSON.stringify(meta, null, 2)}</pre>
        );

        if (meta.type) {
            if(meta.type.toLowerCase() === 'string') {
                output = this.renderString();
            }
            else if(meta.type.toLowerCase() === 'boolean') {
                output = this.renderBoolean();
            }
        }
        if(meta.schema) {
            output = this.renderComplex();
        }

        return output;
    }
}
