import React from 'react';
import { graphql, compose } from 'react-apollo';
import gql from 'graphql-tag';
import { withRouter } from 'react-router-dom';

import * as Yup from 'yup';
import { setWith, clone, pick, isObject } from 'lodash';
import { createSelector } from 'reselect'

import { proposalPageFragment } from './Proposal';
import PayrollScopeView from './PayrollScopeView';
import Locations from '../app/Locations';
import NotFound from '../app/NotFound';
import withQuery from '../common/withQuery';

const setIn = (obj, path, value) => setWith(obj, path, value, (value, key, object) => {
    return value === undefined
        ? {} //override creation of arrays for numeric keys
        : clone(value);
});

const deepPaths = (obj = {}, basePath) => Object.entries(obj).reduce(
    (result, [key, value]) => { 
        const path = basePath
            ? [basePath, key].join('.')
            : key;
        return isObject(value) 
            ? result.concat(deepPaths(value, path)) 
            : result.concat(path)}, 
    []
);

const deepPickBy = (object, pickBy) => pick(object, deepPaths(pickBy));

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
const wholeNumberWhenInScope = Yup.mixed().when('levelId', 
    (levelId, schema) => levelId > 0
        ? wholeNumber 
        : Yup.mixed().test('isNullWhenNotInScope', 'Should be null', value => value === null || value === undefined)
);

const countryPopulationSchema = Yup.object().shape({
    weeklyPayees: wholeNumberWhenInScope,
    biWeeklyPayees: wholeNumberWhenInScope,
    semiMonthlyPayees: wholeNumberWhenInScope,
    monthlyPayees: wholeNumberWhenInScope,
})
const countrySchema = countryPopulationSchema
    .concat(Yup.object().shape({
        levelId: Yup.number().typeError('Select a payroll level or None'),
    }));

export const isCountryInScope = country => country.levelId > 0;
export const countryTotalPayees = country => isCountryInScope(country) &&  countryPopulationSchema.isValidSync(country)
    ? country.monthlyPayees + country.semiMonthlyPayees + country.biWeeklyPayees + country.weeklyPayees 
    : '';

const errorsSelector = state => state.errors;
const touchedSelector = state => state.touched;
const showAllErrorsSelector = state => state.showAllErrors;

const visibleErrors = createSelector(
    errorsSelector,
    touchedSelector,
    showAllErrorsSelector,
    (errors, touched, showAllErrors) => showAllErrors
        ? errors
        : deepPickBy(errors, touched)
);

class PayrollScope extends React.Component {
    state = {
        values: { countries: {} },
        touched: {},
        isSubmitting: false,
    }

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

    handleBlur = path => {
        this.setState(state => setIn(state.touched, path, true));
        this.validate();
    }

    handleSubmit = event => {
        const values = this.validate();
        if (values) {
            this.setState({
                isSubmitting: true
            })
            this.props.onSubmit(values);
        } else {
            this.setState({
                showAllErrors: true
            });
        }
        event.preventDefault();
    }

    validate() {
        try{
            const values = this.schema.validateSync(this.state.values, {abortEarly: false});
            this.setState({ errors: undefined});
            return values;
        }
        catch(err) {
            if (err.name === 'ValidationError') {
                const errors = err.inner.reduce(
                    (result, error) => setWith(result, error.path, error.message, Object), //override array creation for numeric keys
                    {});
                this.setState({ errors });
            }
            return null;
        }
    }

    componentDidMount() {
        const { proposal } = this.props;
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

        this.setState({
            values: { countries }
        })

        this.schema = Yup.object().shape({
            countries: Yup.object().shape(proposal.countries.reduce((acc, country) => {
                acc[country.countryId] = countrySchema;
                return acc;
            }, {}))
        });    
    }

    render() {
        const { proposal, onClose} = this.props;
        const {values, isSubmitting} = this.state;

        return <PayrollScopeView
            name={proposal.name}
            payrollLevels={proposal.productModel.payroll.levels}
            values={values}
            errors={visibleErrors(this.state)}
            isSubmitting={isSubmitting} 
            canEdit={true} 
            onCountryLevelChange={this.handleCountryLevelChange}
            onCountryPopulationChange={this.handleCountryPopulationChange}
            onCountrySelectedChange={this.handleCountrySelectedChange}
            onBlur={this.handleBlur}
            onSubmit={this.handleSubmit}
            onClose={onClose}
        />
    }
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
