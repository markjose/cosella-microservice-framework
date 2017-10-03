import React, { Component } from 'react';

import brace from 'brace';
import AceEditor from 'react-ace';

import 'brace/mode/csharp';
import 'brace/theme/tomorrow';

import './codeEditor.css';

export class CodeEditor extends Component {

    state = {
        code: `// Create your C# code here`
    }

    onLoad() {

    }

    onChange(newValue) {
        console.log('change',newValue);
    }

	render() {
        const { code } = this.state;

        const options = {
            enableBasicAutocompletion: false,
            enableLiveAutocompletion: false,
            enableSnippets: false,
            showLineNumbers: true,
            tabSize: 4,
        };
        
		return (
            <div className="CodeEditor">
                <h2>Code Editor</h2>
                <AceEditor
                    mode="csharp"
                    theme="tomorrow"
                    name="csharpEditor"
                    width="100%"
                    height="100%"
                    onLoad={this.onLoad}
                    onChange={this.onChange}
                    fontSize={14}
                    showPrintMargin={true}
                    showGutter={true}
                    highlightActiveLine={true}
                    value={code}
                    setOptions={options}/>
            </div>
        );
	}
}
