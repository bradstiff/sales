import React from 'react';
import PropTypes from 'prop-types';

import Button from '@material-ui/core/Button';
import Menu from '@material-ui/core/Menu';
import MenuItem from '@material-ui/core/MenuItem';

class SelectMenu extends React.Component {
    state = {
        anchorEl: null,
    };

    handleOpen = event => {
        this.setState({ anchorEl: event.currentTarget });
    };

    handleClose = () => {
        this.setState({ anchorEl: null });
    };

    handleMenuItemClick = (event, option) => {
        this.setState({
            anchorEl: null
        });
        this.props.onSelect(option.value);
    };

    render() {
        const { anchorEl } = this.state;
        const { id, options, value } = this.props;
        const selected = options.find(option => option.value === value) || {};
        return (
            <span>
                <Button
                    aria-owns={anchorEl ? id : null}
                    aria-haspopup="true"
                    onClick={this.handleOpen}
                >
                    {selected.text}
                </Button>
                <Menu
                    id={id}
                    anchorEl={anchorEl}
                    open={Boolean(anchorEl)}
                    onClose={this.handleClose}
                >
                    {options.map(option => (
                        <MenuItem
                            key={option.text}
                            selected={selected.value === option.value}
                            onClick={event => this.handleMenuItemClick(event, option)}
                        >
                            {option.text}
                        </MenuItem>
                    ))}
                </Menu>
            </span>
        );
    }

    static propTypes = {
        id: PropTypes.string.isRequired,
        onSelect: PropTypes.func.isRequired,
        options: PropTypes
            .arrayOf(PropTypes.shape({
                value: PropTypes.any,
                text: PropTypes.string.isRequired,
            }))
            .isRequired,
        value: PropTypes.any,
    };
}

export default SelectMenu;