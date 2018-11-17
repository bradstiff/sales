import React from 'react';
import { Link } from 'react-router-dom';
import PropTypes from 'prop-types';

import Button from '@material-ui/core/Button';

const internalStyle = {
    textTransform: 'none',
    minWidth: 1, //disable padding which causes alignment problems in lists
};

const LinkButton = ({ to, children, style }) => (
    <Button color='primary' component={Link} style={{ ...internalStyle, ...style }} to={to}>
        {children}
    </Button>
);

LinkButton.propTypes = {
    to: PropTypes.string.isRequired,
    children: PropTypes.string.isRequired,
    style: PropTypes.object,
}
export default LinkButton;