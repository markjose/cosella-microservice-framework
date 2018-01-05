import React, { Component } from 'react';
import { GridList } from 'material-ui/GridList';
import { MicroserviceTile, MicroserviceExplorer } from '.';
import { services } from '../../services';
import FloatingActionButton from 'material-ui/FloatingActionButton';
import Refresh from 'material-ui/svg-icons/navigation/refresh';

import './microservice-list.css';

export class MicroserviceList extends Component {
  
    state = {
        exploring: null,
        microservices: []
    }

    componentDidMount() {
        this.reloadServices();
    }

    reloadServices() {
        services
            .list()
            .then(microservices => {
                this.setState({
                    ...this.state,
                    microservices
                });
            })
            .catch(error => {
                console.warn(error);
                this.setState({
                    ...this.state,
                    microservices: []
                });
                setTimeout(() => this.reloadServices(), 2000);
            });
    }

    startExploring(service) {
        this.setState({
            ...this.state,
            exploring: service
        });
    }

    stopExploring(service) {
        this.setState({
            ...this.state,
            exploring: null
        });
    }

    render() {
        const { exploring, microservices } = this.state;

        const styles = {
            floatingButton: { float: 'right', margin: '1em' }
        };

        let output = null;
        if(exploring) {
            output = <MicroserviceExplorer service={exploring} onCancel={() => this.stopExploring()} />
        }
        else {
            const tiles = microservices.map((service, key) => <MicroserviceTile
                key={key}
                service={service}
                onExplore={serviceName => this.startExploring(service)}
            />);

            output = (
                <div>
                    <FloatingActionButton 
                        style={styles.floatingButton}
                        zDepth={3}
                        secondary={true}
                        onTouchTap={() => this.reloadServices()}>
                        <Refresh />
                    </FloatingActionButton>

                    <GridList cols={3} cellHeight="auto">
                        {tiles}
                    </GridList>
                </div>
            );
        }
        return (
            <div className="microservice-list">
                {output}
            </div>
        );
    }
}
