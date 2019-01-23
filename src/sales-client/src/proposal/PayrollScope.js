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
import FormControl from '@material-ui/core/FormControl';
import Select from '@material-ui/core/Select';
import FormHelperText from '@material-ui/core/FormHelperText';
import Table from '@material-ui/core/Table';
import TableHead from '@material-ui/core/TableHead';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableRow from '@material-ui/core/TableRow';

import { proposalPageFragment } from './Proposal';
import Locations from '../app/Locations';
import NotFound from '../app/NotFound';
import withQuery from '../common/withQuery';
import NumericInput from '../common/NumericInput';
import FlexRightAligned from '../common/FlexRightAligned';

const setIn = (obj, keyPath, value) => {
    const lastKeyIndex = keyPath.length-1;
    let currentObj = obj;
    for (let i = 0; i < lastKeyIndex; ++ i) {
        const key = keyPath[i];
        if (!(key in currentObj))
        currentObj[key] = {}
        currentObj = currentObj[key];
    }
    currentObj[keyPath[lastKeyIndex]] = value;
    return obj;
}

const isEmptyObject = obj =>  Object.keys(obj).length === 0 && obj.constructor === Object;

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

//const emptyToZero = Yup.number().transform((cv, ov) => ov === '' ? 0 : cv);
const wholeNumber = Yup.number().typeError('Enter a number').integer('Enter an integer').min(0, 'Enter a non-negative number');
const countryPopulationSchema = Yup.object().shape({
    weeklyPayees: wholeNumber,
    biWeeklyPayees: wholeNumber,
    semiMonthlyPayees: wholeNumber,
    monthlyPayees: wholeNumber,
})
const countrySchema = countryPopulationSchema
    .concat(Yup.object().shape({
        levelId: Yup.number().typeError('Select a payroll level or None'),
    }));

const PayrollScope = ({ proposal, onSubmit, onClose }) => {
    const countries = proposal.countries.reduce((acc, country) => {
        const scope = country.payrollScope || {};
        acc[country.countryId] = {
            id: country.countryId,
            name: country.name,
            levelId: scope.level ? scope.level.id : 0, //todo:how to identify new countries
            monthlyPayees: scope.monthlyPayees,
            semiMonthlyPayees: scope.semiMonthlyPayees,
            biWeeklyPayees: scope.biWeeklyPayees,
            weeklyPayees: scope.weeklyPayees,
        }
        return acc;
    }, {});
    const schema = Yup.object().shape({
        countries: Yup.object().shape(proposal.countries.reduce((acc, country) => {
            acc[country.countryId] = countrySchema;
            return acc;
        }, {}))
    });
    return (
        <Paper>
            <PayrollScopeForm
                name={proposal.name}
                initialValues={{ countries }}
                schema={schema}
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
            onSubmit: values => {
                const countryScopes = Object.values(values.countries)
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

    mutateCountry(id, mutateFn) {
        this.setState((state, props) => {
            const mutableCountry = Object.assign({}, state.values.countries[id]);
            const values = {
                ...state.values,
                countries: {
                    ...state.values.countries,
                    [id]: mutableCountry
                }
            };
            mutateFn(mutableCountry);
            return { values }
        });
    }

    validate() {
        try{
            const values = this.props.schema.validateSync(this.state.values, {abortEarly: false});
            this.setState({ errors: { countries: {} }});
            return values;
        }
        catch(err) {
            if (err.name === 'ValidationError') {
                const errors = err.inner.reduce((acc, error) => setIn(acc, error.path.split('.'), error.message), {});
                this.setState({ errors });
            }
            return null;
        }
    }

    handleToggleSelectAll = selected => {
        const { countries } = this.state;
        this.setState({
            selected,
            countries: countries.map(country => ({
                ...country,
                selected
            }))
        });
    }

    handleCountrySelectedChange = (id, selected) => {
        this.mutateCountry(id, country => country.selected = selected);
    }

    handleCountryLevelChange = (id, levelId) => {
        this.mutateCountry(id, country => {
            const removed = country.levelId > 0 && !(levelId > 0);
            const added = levelId > 0 && !(country.levelId > 0);
            country.levelId = levelId;
            if (removed) {
                country.monthlyPayees = country.semiMonthlyPayees = country.biWeeklyPayees = country.weeklyPayees = null;
            } else if (added) {
                country.monthlyPayees = country.semiMonthlyPayees = country.biWeeklyPayees = country.weeklyPayees = 0;
            }
        });
    }

    handleCountryPopulationChange = (id, populationField, payees) => {
        this.mutateCountry(id, country => country[populationField] = payees);
    }

    handleCountryFieldBlur = (id, field) => {
        this.setState(state => { 
            const touched = {
                ...state.touched,
                countries: {
                    ...state.touched.countries,
                    [id]: {
                        ...state.touched.countries[id],
                        [field]: true,
                    }
                }
            }
            return { touched };
        });
        this.validate();
    }

    handleSubmit = event => {
        const values = this.validate();
        if (values) {
            this.props.onSubmit(values);
        }
        event.preventDefault();
    }

    static getDerivedStateFromProps(props, state) {
        if (isEmptyObject(state)) {
            return { 
                values: {
                    countries: props.initialValues.countries
                },
                touched: {
                    countries: {}
                },
                errors:  {
                    countries: {}
                },
                isSubmitting: false,
            };
        }
        return null;
    }

    render() {
        const { name, payrollLevels, onClose, classes } = this.props;
        const { values: {countries}, touched, errors, isSubmitting } = this.state;
        const canEdit = true;
        return (
            <form onSubmit={this.handleSubmit} autoComplete="off">
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
                            <TableCell>Total</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {Object.keys(countries).map(id => (
                            <PayrollCountryScope
                                key={id}
                                country={countries[id]}
                                levels={payrollLevels}
                                onSelectedChange={this.handleCountrySelectedChange}
                                onLevelChange={this.handleCountryLevelChange}
                                onPopulationChange={this.handleCountryPopulationChange}
                                onFieldBlur={this.handleCountryFieldBlur}
                                touched={touched.countries[id]}
                                errors={errors.countries[id]}
                            />
                        ))}
                    </TableBody>
                </Table>
            </form>
        );

    }
}

class PayrollCountryScope extends React.PureComponent {
    handleLevelChange = event => {
        const { country, onLevelChange } = this.props;
        onLevelChange(country.id, event.target.value);
    }
    handlePopulationChange = event => {
        const { country, onPopulationChange } = this.props;
        onPopulationChange(country.id, event.target.name, event.target.value);
    }
    handleSelectedChange = event => {
        const { country, onSelectedChange } = this.props;
        onSelectedChange(country.id, event.target.checked);
    }
    handleBlur = event => {
        const { country, onFieldBlur } = this.props;
        onFieldBlur(country.id, event.target.name);
    }
    render() {
        const { country, levels, touched = {}, errors = {}} = this.props;
        const isInScope = country.levelId > 0;
        const totalPayees = isInScope && countryPopulationSchema.isValidSync(country)
            ? country.monthlyPayees + country.semiMonthlyPayees + country.biWeeklyPayees + country.weeklyPayees 
            : '';
        const fieldProps = name => ({
            name,
            value: country[name],
            error: touched[name] && errors[name] ? errors[name] : '',
            onBlur: this.handleBlur,
        })
        return (
            <TableRow key={country.id}>
                <TableCell>
                    <input type='checkbox' name='selected' checked={country.selected} onChange={this.handleSelectedChange} />
                </TableCell>
                <TableCell>{country.name}</TableCell>
                <TableCell>
                    <FormControl error={touched.levelId && errors.levelId}>
                        <Select
                            value={country.levelId}
                            onChange={this.handleLevelChange}
                            onBlur={this.handleBlur}
                            name='levelId'
                        >
                            <MenuItem key="" value=""><em>Select</em></MenuItem>
                            <MenuItem key="0" value={0}>None</MenuItem>
                            {levels.map(level => <MenuItem key={level.id} value={level.id}>{level.name}</MenuItem>)}
                        </Select>
                        {touched.levelId && errors.levelId && <FormHelperText>{errors.levelId}</FormHelperText>}
                    </FormControl>
                </TableCell>
                <TableCell>
                    <NumericInput {...fieldProps('monthlyPayees')} onChange={this.handlePopulationChange} disabled={!isInScope} />
                </TableCell>
                <TableCell>
                    <NumericInput {...fieldProps('semiMonthlyPayees')} onChange={this.handlePopulationChange} disabled={!isInScope} />
                </TableCell>
                <TableCell>
                    <NumericInput {...fieldProps('biWeeklyPayees')} onChange={this.handlePopulationChange} disabled={!isInScope} />
                </TableCell>
                <TableCell>
                    <NumericInput {...fieldProps('weeklyPayees')} onChange={this.handlePopulationChange} disabled={!isInScope} />
                </TableCell>
                <TableCell>{totalPayees}</TableCell>
            </TableRow>
        );
    }
}