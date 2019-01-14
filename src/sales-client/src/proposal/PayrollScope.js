import React from 'react';
import { graphql, compose } from 'react-apollo';
import gql from 'graphql-tag';
import { withRouter } from 'react-router-dom';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import * as Yup from 'yup';

import Paper from '@material-ui/core/Paper';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';
import MenuItem from '@material-ui/core/MenuItem';
import Select from '@material-ui/core/Select';
import Table from '@material-ui/core/Table';
import TableHead from '@material-ui/core/TableHead';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableRow from '@material-ui/core/TableRow';

import { proposalPageFragment } from './Proposal';
import Locations from '../app/Locations';
import NotFound from '../app/NotFound';
import withQuery from '../common/withQuery';
import FlexRightAligned from '../common/FlexRightAligned';

const query = gql`
    query PayrollScope($id: Int!) {
        proposal(id: $id) {
            id
            name
            productModel {
                payroll {
                    levels {
                        id
                        name
                    }
                }
            }
            countries {
                countryId
                name
                payrollScope {
                    level {
                        id
                    }
                    monthlyPayees
                    semiMonthlyPayees
                    biWeeklyPayees
                    weeklyPayees
                }
            }
        }
    }
`;

const mutation = gql`
    mutation UpdatePayrollScope($id: Int!, $countryScopes: [UpdatePayrollCountryScope!]!) {
        updatePayrollScope(proposalId: $id, countryScopes: $countryScopes) {
            ...proposalFragment
        }
    }
    ${proposalPageFragment}
`;

const emptyToZero = Yup.number().transform((cv, ov) => ov === '' ? 0 : cv);
const schema = Yup.object().shape({
    countries: Yup.array().of(
        Yup.object().shape({
            levelId: Yup.number().integer(),
            weeklyPayees: emptyToZero.integer(),
            biWeeklyPayees: emptyToZero.integer(),
            semiMonthlyPayees: emptyToZero.integer(),
            monthlyPayees: emptyToZero.integer(),
        })
    )
});

const PayrollScope = ({ proposal, onSubmit, onClose }) => {
    const initialValues = {
        countries: proposal.countries.map(c => {
            const scope = c.payrollScope || {};
            return ({
                id: c.countryId,
                name: c.name,
                levelId: scope.level ? scope.level.id : 0, //todo:how to identify new countries
                monthlyPayees: scope.monthlyPayees,
                semiMonthlyPayees: scope.semiMonthlyPayees,
                biWeeklyPayees: scope.biWeeklyPayees,
                weeklyPayees: scope.weeklyPayees,
            });
        })
    };
    return (
        <Paper>
            <PayrollScopeForm
                name={proposal.name}
                initialValues={initialValues}
                payrollLevels={proposal.productModel.payroll.levels}
                onSubmit={onSubmit}
                onClose={onClose}
            />
        </Paper>
    );
}

export default compose(
    withRouter,
    withQuery(query, {
        selector: 'proposal',
    }, NotFound),
    graphql(mutation, {
        name: 'updatePayrollScope',
        props: ({ updatePayrollScope, ownProps: {history, id}}) => ({
            onSubmit: rawValues => {
                //validation has already ocurred...this is necessary to get typecast values
                const values = schema.validateSync(rawValues);
                const countryScopes = values.countries
                    .filter(country => country.levelId > 0)
                    .map(({ id, name, ...values }) => ({
                        //throw away country.name
                        countryId: id,
                        ...values
                    }));
                updatePayrollScope({variables: {id, countryScopes}})
                    .then(() => history.push(Locations.Proposal.toUrl({ id })));
            },
            onClose: () => history.push(Locations.Proposal.toUrl({id}))
        }),
    }),
)(PayrollScope);

class PayrollScopeForm extends React.Component {
    state = {}

    handleChange = (name, value, index) => {
        const countries = this.state.countries.map((country, i) => {
            if (i === index) {
                return {
                    ...country,
                    [name]: value
                };
            };
            return country;
        });
        this.setState({
            countries
        })
    }

    handleSubmit = event => {
        this.props.onSubmit({
            countries: this.state.countries
        });
        event.preventDefault();
    }

    static getDerivedStateFromProps(props, state) {
        if (state.countries === undefined) {
            const { countries } = props.initialValues;
            return { countries };
        }
        return null;
    }

    render() {
        const { name, payrollLevels, onClose, classes } = this.props;
        const countries = this.state.countries || [];
        const isSubmitting = false;
        const canEdit = true;
        return (
            <form onSubmit={this.handleSubmit.bind(this)}>
                <Toolbar>
                    <Typography variant='h5'>{name} Payroll Scope</Typography>
                    <FlexRightAligned>
                        <Button color='primary' disabled={isSubmitting} onClick={onClose}>{canEdit ? 'Cancel' : 'Close'}</Button>
                        {canEdit && <Button color='primary' variant='outlined' type='submit' disabled={isSubmitting}>Save</Button>}
                    </FlexRightAligned>
                </Toolbar>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell />
                            <TableCell>Country</TableCell>
                            <TableCell>Level</TableCell>
                            <TableCell>Monthly</TableCell>
                            <TableCell>Semi-monthly</TableCell>
                            <TableCell>Bi-weekly</TableCell>
                            <TableCell>Weekly</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {countries.map((country, index) => (
                            <PayrollCountryScope
                                key={index}
                                country={country}
                                index={index}
                                levels={payrollLevels}
                                onChange={this.handleChange}
                            />
                        ))}
                    </TableBody>
                </Table>
            </form>
        );

    }
}

class PayrollCountryScope extends React.PureComponent {
    handleChange = event => {
        const { index, onChange } = this.props;
        onChange(event.target.name, event.target.value, index);
    }
    render() {
        const { country, levels} = this.props;
        return (
            <TableRow key={country.id}>
                <TableCell />
                <TableCell>{country.name}</TableCell>
                <TableCell>
                    <Select
                        value={country.levelId}
                        onChange={this.handleChange}
                        inputProps={{
                            name: 'levelId'
                        }}
                    >
                        <MenuItem value=""><em>Select</em></MenuItem>
                        <MenuItem value="0">None</MenuItem>
                        {levels.map(level => <MenuItem value={level.id}>{level.name}</MenuItem>)}
                    </Select>
                </TableCell>
                <TableCell>
                    <input name='monthlyPayees' value={country.monthlyPayees} onChange={this.handleChange} />
                </TableCell>
                <TableCell>
                    <input name='semiMonthlyPayees' value={country.semiMonthlyPayees} onChange={this.handleChange} />
                </TableCell>
                <TableCell>
                    <input name='biWeeklyPayees' value={country.biWeeklyPayees} onChange={this.handleChange} />
                </TableCell>
                <TableCell>
                    <input name='weeklyPayees' value={country.weeklyPayees} onChange={this.handleChange} />
                </TableCell>
            </TableRow>
        );
    }
}