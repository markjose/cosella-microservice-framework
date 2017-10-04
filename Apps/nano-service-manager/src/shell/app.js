import React, { Component } from 'react';
import { CodeEditor, Compiler, Output, ServiceList } from '../components';
import './app.css';

export class App extends Component {
	render() {
		return (
            <div className="App">
                <div className="Header">
                    <h1>Nanoservice Manager</h1>
                </div>
                <div className="Workspace">
                    <div className="Sidebar">
                        <ServiceList />
                    </div>
                    <div className="Content">
                        <CodeEditor />
                        <Compiler />
                        <Output />
                    </div>
                </div>
                <div className="Footer">
                    <p>Ready...</p>
                </div>
            </div>
        );
	}
}
