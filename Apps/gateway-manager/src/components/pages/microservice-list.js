import React, { Component } from 'react';
import { GridList } from 'material-ui/GridList';
import { MicroserviceTile, MicroserviceExplorer } from '.';
import { services } from '../../services';

import './microservice-list.css';

export class MicroserviceList extends Component {
  
    state = {
        exploring: null
    }

    startExploring(service) {
        this.setState({
            ...this.state,
            exploring: service
        });
    }

    render() {
        const { exploring } = this.state;
        const microservices = services.list();

        let output = null;
        if(exploring) {
            output = <MicroserviceExplorer service={exploring} />
        }
        else {
            const tiles = microservices.map((service, key) => <MicroserviceTile
                key={key}
                serviceName={service.ServiceName}
                serviceDescription={''}
                serviceInstances={service.Instances}
                onExplore={serviceName => this.startExploring(service)}
            />);

            output = (
                <GridList cols={4} cellHeight="auto">
                    {tiles}
                </GridList>
            );
        }
        return (
            <div className="microservice-list">
                {output}
            </div>
        );
    }
}
