import React, { Component } from 'react';
import PropTypes from 'prop-types';
import warning from 'warning';

import TableCell from '@material-ui/core/TableCell';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import TableSortLabel from '@material-ui/core/TableSortLabel';
import Tooltip from '@material-ui/core/Tooltip';

import { objectCompare } from './SortHelper';

class SortEnabledTableHead extends Component {
    createSortHandler = column => event => {
        this.props.onRequestSort(event, column.field);
    };

    render() {
        const { columns, order, orderBy } = this.props;
        const orderByCol = columns.find(column => column.field === orderBy);
        return (
            <TableHead>
                <TableRow>
                    {columns.map(column => {
                        return (
                            <TableCell
                                key={column.field}
                                numeric={column.numeric || false}
                                padding={column.padding || 'default'}
                                sortDirection={orderByCol === column ? order : false}
                            >
                                <Tooltip
                                    title="Sort"
                                    placement={column.numeric ? 'bottom-end' : 'bottom-start'}
                                    enterDelay={300}
                                >
                                    <TableSortLabel
                                        active={orderByCol === column}
                                        direction={order}
                                        onClick={this.createSortHandler(column)}
                                    >
                                        {column.label}
                                    </TableSortLabel>
                                </Tooltip>
                            </TableCell>
                        );
                    }, this)}
                </TableRow>
            </TableHead>
        );
    }

    static propTypes = {
        onRequestSort: PropTypes.func.isRequired,
        order: PropTypes.string.isRequired,
        orderBy: PropTypes.string.isRequired,
        columns: PropTypes.arrayOf(
            PropTypes.shape({
                field: PropTypes.string.isRequired,
                label: PropTypes.string.isRequired,
                numeric: PropTypes.bool,
                padding: PropTypes.oneOf(['default', 'dense', 'none',]),
                compareField: PropTypes.string,
            })).isRequired,
    };
}

export default SortEnabledTableHead;

export const makeCompareFn = (order, orderBy, columns, keyField) => (a, b) => {
    const orderByCol = columns.find(column => column.field === orderBy);
    if (!orderByCol) {
        warning(orderByCol, `SortEnabledTableHead columns prop does not contain a column named '${orderBy}'.`);
        return false;
    }
    const compareField = orderByCol.compareField || orderByCol.field;
    return order === 'asc'
        ? objectCompare(a, b, compareField, keyField)
        : objectCompare(b, a, compareField, keyField);
};