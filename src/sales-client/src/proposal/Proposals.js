import React from 'react';
import { compose } from 'react-apollo';
import gql from 'graphql-tag';

import { withStyles } from '@material-ui/core/styles';
import Paper from '@material-ui/core/Paper';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableRow from '@material-ui/core/TableRow';
import TablePagination from '@material-ui/core/TablePagination';
import Typography from '@material-ui/core/Typography';
import Toolbar from '@material-ui/core/Toolbar';
import FilterListIcon from '@material-ui/icons/FilterList';

import SortEnabledTableHead from '../common/SortEnabledTableHead';
import LinkButton from '../common/LinkButton';
import Locations from '../app/Locations';
import NotFound from '../app/NotFound';
import withQuery from '../common/withQuery';

const query = gql`
    query ProposalListPage($page: Int!, $rowsPerPage: Int!, $orderBy: String!, $order: String!) {
        proposalListPage(page: $page, rowsPerPage: $rowsPerPage, orderBy: $orderBy, order: $order) {
            totalCount,
            rows {
                id,
                name,
                clientName,
                comments
            }
        }
    }
`;

const columnData = [
    { field: 'name', label: 'Name', padding: 'dense', },
    { field: 'clientName', label: 'Client', padding: 'dense',  },
];

const styles = theme => ({
    spacer: {
        flex: 'auto',
    },
    actions: {
        flex: 'none',
        color: theme.palette.text.secondary,
    },
    title: {
        flex: 'none',
    },
    tableContainer: {
        overflowX: 'auto',
    },
});


class Proposals extends React.Component {
    replaceLocation(qsParams) {
        const nextLocation = Locations.Proposals.toUrl({
            ...this.props,
            ...qsParams
        });
        this.props.history.replace(nextLocation);
    }

    handleChangePage = (event, page) => this.replaceLocation({ page });
    handleChangeRowsPerPage = event => this.replaceLocation({ rowsPerPage: event.target.value });

    handleRequestSort = (event, fieldName) => {
        const order = this.props.orderBy === fieldName && this.props.order === 'asc'
            ? 'desc'
            : 'asc';
        this.replaceLocation({ order, orderBy: fieldName, });
    };

    render() {
        const { classes, proposalListPage, page, rowsPerPage, order, orderBy, showFilter } = this.props;
        return (
            <Paper>
                <Toolbar>
                    <div className={classes.title}>
                        <Typography variant="headline">
                            Proposals
                        </Typography>
                    </div>
                    <div className={classes.spacer} />
                    <div className={classes.actions}>
                    </div>
                </Toolbar>
                {proposalListPage.totalCount && (
                    <div className={classes.tableContainer}>
                        <Table>
                            <SortEnabledTableHead
                                order={order}
                                orderBy={orderBy}
                                onRequestSort={this.handleRequestSort}
                                columns={columnData}
                            />
                            <TableBody>
                                {proposalListPage.rows
                                    .slice()
                                    .map(proposal => (
                                        <TableRow key={proposal.id}>
                                            <TableCell component="th" scope="row" padding='dense'><LinkButton to={Locations.Proposal.toUrl({ id: proposal.id })}>{proposal.name}</LinkButton></TableCell>
                                            <TableCell padding='dense'>{proposal.clientName}</TableCell>
                                        </TableRow>
                                    ))
                                }
                            </TableBody>
                        </Table>
                        <TablePagination
                            component="div"
                            count={proposalListPage.totalCount}
                            rowsPerPage={rowsPerPage}
                            rowsPerPageOptions={[25, 50, 100, 200]}
                            page={page}
                            backIconButtonProps={{
                                'aria-label': 'Previous Page',
                            }}
                            nextIconButtonProps={{
                                'aria-label': 'Next Page',
                            }}
                            onChangePage={this.handleChangePage}
                            onChangeRowsPerPage={this.handleChangeRowsPerPage}
                        />
                    </div>
                )}
            </Paper>
        );
    }
}

const mapPropsToVariables = props => ({
    page: props.page,
    rowsPerPage: props.rowsPerPage,
    orderBy: props.orderBy.toUpperCase(),
    order: props.order.toUpperCase(),
});

export default compose(
    withQuery(query, {
        selector: 'proposalListPage',
        variables: mapPropsToVariables,
    }, NotFound),
    withStyles(styles)
)(Proposals);