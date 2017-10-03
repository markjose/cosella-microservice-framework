import React, { Component } from 'react';

import brace from 'brace';
import AceEditor from 'react-ace';

import 'brace/mode/markdown';
import 'brace/theme/monokai';

import './App.css';

class App extends Component {
  onLoad() {

  }
  onChange(newValue) {
    console.log('change',newValue);
  }
	render() {
		return (
      <div className="App">
        <AceEditor
          mode="markdown"
          theme="monokai"
          name="csharpEditor"
          width="100%"
          height="100%"
          onLoad={this.onLoad}
          onChange={this.onChange}
          fontSize={14}
          showPrintMargin={true}
          showGutter={true}
          highlightActiveLine={true}
          value={`// Create your C# code here`}
          setOptions={{
            enableBasicAutocompletion: false,
            enableLiveAutocompletion: false,
            enableSnippets: false,
            showLineNumbers: true,
            tabSize: 4,
          }}/>
        </div>
    );
	}
}

export default App;
