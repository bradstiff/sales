import React from 'react';

import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableRow from '@material-ui/core/TableRow';

import SortEnabledTableHead, { makeCompareFn } from './SortEnabledTableHead';

import { render, cleanup, fireEvent } from 'react-testing-library';

afterEach(cleanup);

const columnData = [
    {
        field: 'id',
        label: 'ID',
        numeric: true,
    }, {
        field: 'name',
        label: 'Name',
    }, {
        field: 'timeDisplay',
        label: 'Time',
        compareField: 'time',
    }
];

const data = [
    { id: 2, name: 'Everest', time: 1, timeDisplay: '1:00 AM' },
    { id: 1, name: 'Everest', time: 22, timeDisplay: '10:00 PM' },
    { id: 3, name: 'Etna', time: null, timeDisplay: null },
    { id: 4, name: 'Eldora', time: 12.30, timeDisplay: '12:30 PM' },
    { id: 5, name: 'Fuji', time: 24, timeDisplay: '12:00 AM' },
];

class TestTable extends React.Component {
    state = {
        orderBy: 'id',
        order: 'asc',
    };

    handleRequestSort = (event, field) => {
        const { orderBy, order } = this.state;
        if (orderBy === field) {
            this.setState({
                order: order === 'asc'
                    ? 'desc'
                    : 'asc',
            });
        } else {
            this.setState({
                order: 'asc',
                orderBy: field,
            });
        }
    };

    render() {
        const { orderBy, order } = this.state;
        return (
            <Table>
                <SortEnabledTableHead columns={columnData} orderBy={orderBy} order={order} onRequestSort={this.handleRequestSort} />
                <TableBody>
                    {data
                        .slice()
                        .sort(makeCompareFn(order, orderBy, columnData, 'id'))
                        .map(row => (
                            <TableRow key={row.id}>
                                <TableCell data-id>{row.id}</TableCell>
                                <TableCell>{row.name}</TableCell>
                                <TableCell>{row.timeDisplay}</TableCell>
                            </TableRow>
                        ))}
                </TableBody>
            </Table>
        );
    }
}

const getSortedIDs = container => [...container.querySelectorAll('td[data-id]')].map(td => parseInt(td.innerHTML));
const clickColumnSortButton = (label, getByText) => {
    const button = getByText(label);
    fireEvent.click(button);
}

test('sorting by ID asc, then desc', () => {
    const { container, getByText, debug } = render(<TestTable />);
    expect(getSortedIDs(container)).toEqual([1, 2, 3, 4, 5]);

    clickColumnSortButton('ID', getByText);
    expect(getSortedIDs(container)).toEqual([5, 4, 3, 2, 1]);
});

test('sorting by name asc, then desc, ID used as tie-breaker', () => {
    const { container, getByText, debug } = render(<TestTable />);
    clickColumnSortButton('Name', getByText);
    expect(getSortedIDs(container)).toEqual([4,3,1,2,5]);

    clickColumnSortButton('Name', getByText);
    expect(getSortedIDs(container)).toEqual([5,2,1,3,4]);
});

test('sorting by timeDisplay asc, then desc, time column used for sorting, nulls sort to the top when asc', () => {
    const { container, getByText, debug } = render(<TestTable />);
    clickColumnSortButton('Time', getByText);
    expect(getSortedIDs(container)).toEqual([3,2,4,1,5]);

    clickColumnSortButton('Time', getByText);
    expect(getSortedIDs(container)).toEqual([5,1,4,2,3]);
});