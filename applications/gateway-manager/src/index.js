import React from 'react';
import ReactDOM from 'react-dom';
import lightBaseTheme from 'material-ui/styles/baseThemes/lightBaseTheme';
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import getMuiTheme from 'material-ui/styles/getMuiTheme';
import injectTapEventPlugin from 'react-tap-event-plugin';
import registerServiceWorker from './registerServiceWorker';
import { App } from './components';

import './index.css';

injectTapEventPlugin();

const theme = {
    ...lightBaseTheme,
    fontFamily: 'Open Sans Condensed',
    palette: {
        primary1Color: '#334044'/*,
        primary2Color: cyan700,
        primary3Color: grey400,*/
    },
    menuItem: {
        fontSize: '2em'
    }
};

ReactDOM.render((
    <MuiThemeProvider muiTheme={getMuiTheme(theme)}>
        <App />
    </MuiThemeProvider>
), document.getElementById('root'));
registerServiceWorker();
