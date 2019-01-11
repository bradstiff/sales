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

const PayrollScope = ({ proposal, onSubmit, onClose, classes }) => {
    const canEdit = true;
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
    }
    return (
        <Paper>
            <Formik
                initialValues={initialValues}
                validationSchema={schema}
                onSubmit={onSubmit}
                render={({ values, errors, handleChange, isSubmitting }) => {
                    console.log(errors);
                    return (
                        <Form>
                            <Toolbar>
                                <Typography variant='h5'>{proposal.name} Payroll Scope</Typography>
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
                                    {values.countries.map((country, index) => (
                                        <TableRow key={index}>
                                            <TableCell />
                                            <TableCell>{country.name}</TableCell>
                                            <TableCell>
                                                <Select
                                                    value={country.levelId}
                                                    onChange={handleChange}
                                                    inputProps={{
                                                        name: `countries.${index}.levelId`
                                                    }}
                                                >
                                                    <MenuItem value=""><em>Select</em></MenuItem>
                                                    <MenuItem value="0">None</MenuItem>
                                                    {proposal.productModel.payroll.levels.map(level => <MenuItem value={level.id}>{level.name}</MenuItem>)}
                                                </Select>
                                            </TableCell>
                                            <TableCell>
                                                <Field name={`countries.${index}.monthlyPayees`} />
                                                <ErrorMessage name={`countries[${index}].monthlyPayees`} />
                                            </TableCell>
                                            <TableCell>
                                                <Field name={`countries.${index}.semiMonthlyPayees`} />
                                                <ErrorMessage name={`countries[${index}].semiMonthlyPayees`} />
                                            </TableCell>
                                            <TableCell>
                                                <Field name={`countries.${index}.biWeeklyPayees`} />
                                                <ErrorMessage name={`countries[${index}].biWeeklyPayees`} />
                                            </TableCell>
                                            <TableCell>
                                                <Field name={`countries.${index}.weeklyPayees`} />
                                                <ErrorMessage name={`countries[${index}].weeklyPayees`} />
                                            </TableCell>
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
                            {errors && <div>{errors[0]}</div>}
                        </Form>
                    );
                }}
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
                    .then(() => history.push(Locations.Proposal.toUrl({id})));
            },
            onClose: () => history.push(Locations.Proposal.toUrl({id}))
        }),
    }),
)(PayrollScope);