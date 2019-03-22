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
        const { id, options, value, defaultCaption, disabled } = this.props;
        const selected = options.find(option => option.value === value);
        const caption = selected 
            ? selected.text 
            : defaultCaption || '';
        return (
            <span>
                <Button
                    aria-owns={anchorEl ? id : null}
                    aria-haspopup="true"
                    onClick={this.handleOpen}
                    disabled={disabled}
                >
                    {caption}
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
                            selected={option.value === value}
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
        defaultCaption: PropTypes.string,
        disabled: PropTypes.bool,
    };
}

export default SelectMenu;