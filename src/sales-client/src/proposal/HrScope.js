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
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Checkbox from '@material-ui/core/Checkbox';
import InputLabel from '@material-ui/core/InputLabel';
import MenuItem from '@material-ui/core/MenuItem';
import FormHelperText from '@material-ui/core/FormHelperText';
import FormControl from '@material-ui/core/FormControl';
import Select from '@material-ui/core/Select';

import { proposalPageFragment } from './Proposal';
import Locations from '../app/Locations';
import NotFound from '../app/NotFound';
import withQuery from '../common/withQuery';
import FlexRightAligned from '../common/FlexRightAligned';
import ToggleCountry from './ToggleCountry';

const query = gql`
    query ProposalHr($id: Int!) {
        proposal(id: $id) {
            id
            name
            productModel {
                hr {
                    levels {
                        id
                        name
                    }
                }
            }
            hrScope {
                level {
                    id
                    name
                }
            }
            countries {
                countryId
                name
                hrScope {
                    level {
                        id
                    }
                }
            }
        }
    }
`;

const mutation = gql`
    mutation UpdateHrScope($id: Int!, $countryIds: [Int!]!, $levelId: Int!) {
        updateHrScope(proposalId: $id, countryIds: $countryIds, levelId: $levelId) {
            ...proposalFragment
        }
    }
    ${proposalPageFragment}
`;

const schema = Yup.object().shape({
    levelId: Yup.number().integer()
});

const HrProduct = ({ proposal, onSubmit, onClose, classes }) => {
    const canEdit = true;
    const initialValues = {
        levelId: proposal.hrScope && proposal.hrScope.level.id,
        countries: proposal.countries.map(c => ({
            id: c.countryId,
            name: c.name,
            inScope: !!c.hrScope
        }))
    }
    return (
        <Paper>
            <Formik
                initialValues={initialValues}
                validationSchema={schema}
                onSubmit={onSubmit}
                render={({ values, handleChange, isSubmitting }) => (
                    <Form>
                        <Toolbar>
                            <Typography variant='h5'>{proposal.name} HR Scope</Typography>
                            <FlexRightAligned>
                                <Button color='primary' disabled={isSubmitting} onClick={onClose}>{canEdit ? 'Cancel' : 'Close'}</Button>
                                {canEdit && <Button color='primary' variant='outlined' type='submit' disabled={isSubmitting}>Save</Button>}
                            </FlexRightAligned>
                        </Toolbar>
                        <div>
                            <FormControl>
                                <InputLabel htmlFor="levelId">Level</InputLabel>
                                <Select
                                    value={values.levelId}
                                    onChange={handleChange}
                                    inputProps={{
                                        name: 'levelId',
                                        id: 'levelId',
                                    }}
                                >
                                    <MenuItem value="">
                                        <em>Select</em>
                                    </MenuItem>
                                    {proposal.productModel.hr.levels.map(level => <MenuItem value={level.id}>{level.name}</MenuItem>)}
                                </Select>
                            </FormControl>
                        </div>
                        {values.countries.map((country, index) => (
                            <div key={index}>
                                <FormControlLabel
                                    control={
                                        <Checkbox
                                            name={`countries.${index}.inScope`}
                                            checked={country.inScope}
                                            tabIndex={-1}
                                            disableRipple
                                            onChange={handleChange}
                                        />
                                    }
                                    label={country.name}
                                />
                            </div>
                        ))}
                    </Form>
                )}
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
        name: 'updateProposalHr',
        props: ({updateProposalHr, ownProps: {history, id}}) => ({
            onSubmit: rawValues => {
                //validation has already ocurred...this is necessary to get typecast values
                const values = schema.validateSync(rawValues);
                const countryIds = values.countries
                    .filter(country => country.inScope)
                    .map(country => country.id);
                updateProposalHr({variables: {id, countryIds, levelId: values.levelId}})
                    .then(() => history.push(Locations.Proposal.toUrl({id})));
            },
            onClose: () => history.push(Locations.Proposal.toUrl({id}))
        }),
    }),
)(HrProduct);