import React from 'react';
import { graphql, compose } from 'react-apollo';
import gql from 'graphql-tag';
import { withRouter } from 'react-router-dom';

import * as Yup from 'yup';
import { setWith, sortBy, keyBy } from 'lodash';
import { createSelector } from 'reselect'

import { proposalPageFragment } from './Proposal';
import PayrollScopeView from './PayrollScopeView';
import Locations from '../app/Locations';
import NotFound from '../app/NotFound';
import withQuery from '../common/withQuery';
import { immutableUpdate, immutableSet, deepPickBy } from '../common/Utils'

//GraphQL queries
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

//Validation schema
const wholeNumber = Yup.number().typeError('Enter a number').integer('Enter an integer').min(0, 'Enter a non-negative number');
const whenCountryInScope = (inScopeSchema, notInScopeSchema) => Yup.mixed().when(
    'levelId', 
    (levelId, schema) => levelId > 0
        ? inScopeSchema
        : notInScopeSchema
);

const wholeNumberWhenInScope = whenCountryInScope(
    wholeNumber,
    Yup.mixed().test('isNullWhenNotInScope', 'Should be null', value => value === null || value === undefined)
);

//const emptyToZero = Yup.number().transform((cv, ov) => ov === '' ? 0 : cv);

const countryPopulationSchema = Yup.object().shape({
    weeklyPayees: wholeNumberWhenInScope,
    biWeeklyPayees: wholeNumberWhenInScope,
    semiMonthlyPayees: wholeNumberWhenInScope,
    monthlyPayees: wholeNumberWhenInScope,
})

const countrySchema = Yup.object()
    .shape({
        levelId: Yup.number().typeError("Select a level or 'None'"),
    })
    .concat(countryPopulationSchema)
    .test(
        'hasPayeesWhenInScope', 
        function(value) {
            return !value.isInScope() || value.totalPayees() > 0 || this.createError({
                path: `countries.${value.id}.totalPayees`,
                message: 'Total payees must be greater than zero'
            });
        }
    );

//Memoized selectors
const visibleErrors = createSelector(
    state => state.errors,
    state => state.touched,
    state => state.showAllErrors,
    (errors, touched, showAllErrors) => showAllErrors
        ? errors
        : deepPickBy(errors, touched)
);

const viewValues = createSelector(
    state => state.values,
    values => ({
        ...values,
        countries: sortBy(Object.values(values.countries), 'name'),
    })
);

const hasSelected = createSelector(
    state => state.values.countries,
    countries => Object.values(countries).some(country => country.selected)
);

class PayrollScope extends React.Component {
    state = {
        values: { countries: {} },
        touched: {},
        isSubmitting: false,
    }

    handleSelectAllChange = event => {
        const selected = event.target.checked;
        this.setState(state => ({
            values: {
                ...state.values,
                countries: keyBy(
                    Object.values(state.values.countries).map(country => ({
                        ...country,
                        selected
                    })),
                    'id'
                )
            }
        }));
    }

    handleCountrySelectedChange = (id, selected) => {
        this.mutateCountry(id, country => country.selected = selected);
    }

    handleAssignLevelToSelected = levelId => {
        this.setState(state => ({
            values: {
                ...state.values,
                countries: keyBy(
                    Object
                        .values(state.values.countries)
                        .map(country => {
                            if (country.selected) {
                                //can this be improved?
                                const newCountry = {...country, selected: false};
                                this.setCountryLevel(newCountry, levelId);
                                return newCountry;
                            } else {
                                return country;
                            }
                        }
                    ),
                    'id'
                )
            }
        }));
    }

    handleCountryLevelChange = (id, levelId) => {
        this.mutateCountry(id, country => this.setCountryLevel(country, levelId));
    }

    setCountryLevel(country, levelId) {
        const removed = country.isInScope() > 0 && !(levelId > 0);
        const added = levelId > 0 && !country.isInScope();
        country.levelId = levelId;
        if (removed) {
            country.monthlyPayees = country.semiMonthlyPayees = country.biWeeklyPayees = country.weeklyPayees = null;
        } else if (added) {
            country.monthlyPayees = country.semiMonthlyPayees = country.biWeeklyPayees = country.weeklyPayees = 0;
        }
    }

    handleCountryPopulationChange = (id, populationField, payees) => {
        this.mutateCountry(id, country => country[populationField] = payees);
    }

    handleBlur = path => {
        this.setState(state => ({
            touched: immutableSet(state.touched, path, true)
        }));
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
                isInScope: function() { return this.levelId > 0; },
                totalPayees: function() {
                    return this.isInScope() && countryPopulationSchema.isValidSync(this) 
                        ? this.monthlyPayees + this.semiMonthlyPayees + this.biWeeklyPayees + this.weeklyPayees 
                        : undefined;
                },
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
        const { isSubmitting } = this.state;

        return <PayrollScopeView
            name={proposal.name}
            payrollLevels={proposal.productModel.payroll.levels}
            values={viewValues(this.state)}
            errors={visibleErrors(this.state)}
            hasSelected={hasSelected(this.state)}
            isSubmitting={isSubmitting} 
            canEdit={true} 
            onCountryLevelChange={this.handleCountryLevelChange}
            onCountryPopulationChange={this.handleCountryPopulationChange}
            onCountrySelectedChange={this.handleCountrySelectedChange}
            onBlur={this.handleBlur}
            onSelectAllChange={this.handleSelectAllChange}
            onAssignLevelToSelected={this.handleAssignLevelToSelected}
            onSubmit={this.handleSubmit}
            onClose={onClose}
        />
    }

    mutateCountry(id, mutateFn) {
        this.setState(state => ({
            values: immutableUpdate(state.values, `countries.${id}`, mutateFn)
        }));
    }

    validate() {
        try{
            const values = this.schema.validateSync(this.state.values, {abortEarly: false});
            this.setState({ errors: undefined });
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
                    .map(({ id, name, selected, ...values }) => ({
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
