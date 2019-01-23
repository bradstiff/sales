import React from 'react';
import PropTypes from 'prop-types';
import TextField from '@material-ui/core/TextField';

export default class NumericInput extends React.PureComponent {
    state = {}
    handleChange = event => {
        const {name, value} = event.target;
        this.setState({ value });
        event.persist();
        event.target = {
            name,
            value: Number(value.trim() || 0),
        }
        this.props.onChange(event);
    }
    render() {
        const {value, error, helperText, onChange, ...inputProps} = this.props;
        const hasError = error.length > 0;
        return <TextField 
            value={this.state.value} 
            error={hasError}
            helperText={error || helperText}
            onChange={this.handleChange} 
            {...inputProps} 
        />;
    }
    static getDerivedStateFromProps(props, state) {
        // if (isNaN(props.value)) {
        //     return null;
        // }
        // const stringValue = 
        //     props.value === undefined || props.value === null ? ''
        //     : props.value === 0 && (state.value === '' || state.value === undefined) ? ''
        //     : String(props.value);
        // if (stringValue === state.value) {
        //     return null;
        // }
        // return {
        //     value: stringValue
        // };

        const stringifiedPropValue = {
            value: String(props.value || '') //transform 0, undefined and null to ''
        };

        if (state.value === undefined) {
            return stringifiedPropValue; //initialize state 
        } else {
            const numericStateValue = Number(state.value);
            if (isNaN(numericStateValue) && isNaN(props.value)) {
                return null;
            } else if (typeof props.value === 'string' && props.value !== state.value) {
                return stringifiedPropValue; //update state
            } else if (numericStateValue !== props.value) {
                return stringifiedPropValue; //update state
            }
            return null;
        }
    }
    static defaultProps() {
        return {
            error: '',
            helperText: '',
        };
    }
}
