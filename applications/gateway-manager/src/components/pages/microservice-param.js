import React, { Component } from 'react';
import TextField from 'material-ui/TextField';
import Toggle from 'material-ui/Toggle';

import './microservice-param.css';

const definitionsRegex = /#\/definitions\/([a-zA-Z0-9]+)/g;

export class MicroserviceParam extends Component {
  
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

    onBlur(ev) {
        const { onChange } = this.state;
        if(onChange) {
            onChange(ev.target.value);
        }
    }

    renderString(name, required) {
        return (
            <TextField
                type="text"
                id={name}
                onBlur={ev => this.onBlur(ev)}
                hintText={required ? 'Required' : 'Optional'}
                floatingLabelText={name}
                fullWidth={true}
                errorText={required ? 'Required' : null}
            />
        );
    }

    renderBoolean(name) {
        return (
            <Toggle
                key={name}
                id={name}
                onBlur={(ev, newValue) => this.onBlur(newValue)}
                label={name}
                defaultToggled={false}
            />
        );
    }

    renderComplex(name, schema) {
        const { definitions } = this.state;

        const refs = definitionsRegex.exec(schema['$ref']);
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
                <h3>{name} ({typeName})</h3>
                {properties}
            </div>
        );
    }

    renderArray() {
        const { meta, arrayItems } = this.state;

        const dom = (arrayItems || []).map(item => (
            <p>{item.toString()}</p>
        ));

        return (
            <div>
                {dom}
                <TextField
                    type={"text"}
                    id={meta.name}
                    onBlur={ev => this.onBlur(ev)}
                    hintText={meta.required ? 'Required' : 'Optional'}
                    floatingLabelText={meta.name}
                    fullWidth={true}
                    errorText={meta.required ? 'Required' : null}
                />
            </div>
        );
    }

    render() {
        const { meta } = this.state;

        let output = (
            <pre>{JSON.stringify(meta, null, 2)}</pre>
        );

        if (meta.type) {
            if(meta.type.toLowerCase() === 'string') {
                output = this.renderString(meta.name, meta.required);
            }
            else if(meta.type.toLowerCase() === 'boolean') {
                output = this.renderBoolean(meta.name);
            }
            else if(meta.type.toLowerCase() === 'array') {
                output = this.renderArray();
            }
        }
        if(meta.schema) {
            output = this.renderComplex(meta.name, meta.schema);
        }

        return output;
    }
}
