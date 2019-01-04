import React from 'react';
import PropTypes from 'prop-types';

import FormControlLabel from '@material-ui/core/FormControlLabel';
import Checkbox from '@material-ui/core/Checkbox';

export default class ToggleCountry extends React.PureComponent {
    static propTypes = {
        id: PropTypes.number,
        name: PropTypes.string,
        onToggle: PropTypes.func,
    };

    state = {
        checked: false,
    };

    handleToggle = event => {
        const { id, onToggle } = this.props;
        this.setState({
            checked: !this.state.checked
        });
        onToggle(id);
    }
    render() {
        const { name } = this.props;
        return (
            <div>
                <FormControlLabel
                    control={
                        <Checkbox
                            checked={this.state.checked}
                            tabIndex={-1}
                            disableRipple
                            onChange={this.handleToggle}
                        />
                    }
                    label={name}
                />
            </div>
        );
    }
}

