import React from 'react';
import { graphql, compose } from 'react-apollo';
import gql from 'graphql-tag';
import * as Yup from 'yup';
import {withRouter} from 'react-router-dom';
import { Formik, Form, FieldArray, Field, ErrorMessage } from 'formik';

import Paper from '@material-ui/core/Paper';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import ListItemSecondaryAction from '@material-ui/core/ListItemSecondaryAction';
import IconButton from '@material-ui/core/IconButton';
import AddIcon from '@material-ui/icons/Add';
import DeleteIcon from '@material-ui/icons/Delete';
import TextField from '@material-ui/core/TextField';
import { withStyles } from '@material-ui/core/styles';

import Locations from '../app/Locations';
import NotFound from '../app/NotFound';
import withQuery from '../common/withQuery';
import AddCountries from './AddCountries';
import FlexRightAligned from '../common/FlexRightAligned';

const styles = theme => ({
    fab: {
      position: 'absolute',
      bottom: theme.spacing.unit * 2,
      right: theme.spacing.unit * 2,
    },
    grow: {
        flexGrow: 1,
    },
});

const query = gql`
    query ProposalCountries($id: Int!) {
        proposal(id: $id) {
            name,
            countries {
                id,
                countryId,
                name,
                headcount,
            }
        }
    }
`;

const mutation = gql`
    mutation updateProposalCountries($id: Int!, $proposalCountries: [ProposalCountryInput!]!) {
        updateProposalCountries(proposalId: $id, proposalCountries: $proposalCountries) 
    }
`;

const nullableNumber = Yup.number().transform((cv, ov) => ov === '' ? null : cv).nullable();
const schema = Yup.object().shape({
    countries:  Yup.array().of(
      Yup.object().shape({
        headcount: nullableNumber.integer().positive(),
      })
    )
});

class ProposalCountries extends React.Component {
    state = {
        addingCountries: false,
    };

    handleAddCountriesClick = () => {
        this.setState({
            addingCountries: true,
        });
    }

    handleDoneAddingCountries = countries => addedCountries => {
        this.setState({
            addingCountries: false,
        })

        if (!addedCountries) {
            return;
        }

        const newCountries = addedCountries.map(added => ({
            countryId: added.id,
            name: added.name,
            headcount: null
        }));
        countries.push(...newCountries);
    }

    render() {
        const { proposal: {name, countries}, onSubmit, onClose, classes } = this.props;
        const { addingCountries } = this.state;
        const canEdit = true;
        return (
            <Paper>
                <Formik
                    initialValues={{countries}}
                    validationSchema={schema}
                    onSubmit={onSubmit}
                    render={({ values, isSubmitting }) => {
                        const countryIds = values.countries.map(country => country.countryId);
                        return (
                            <React.Fragment>
                                <Form>
                                    <Toolbar>
                                        <Typography variant='h5'>{name}</Typography>
                                        <FlexRightAligned>
                                            <Button color='primary' disabled={isSubmitting} onClick={onClose}>{canEdit ? 'Cancel' : 'Close'}</Button>
                                            {canEdit && <Button color='primary' variant='outlined' type='submit' disabled={isSubmitting}>Save</Button>}
                                        </FlexRightAligned>
                                    </Toolbar>
                                    <FieldArray
                                        name='countries'
                                        render={arrayHelpers => (
                                            <List>
                                                {values.countries.map((country, index) => {
                                                    const name = `countries.${index}.headcount`;
                                                    return (
                                                        <ListItem key={country.countryId}>
                                                            <ListItemText>
                                                                <Typography>{country.name}</Typography>
                                                                <Field name={`countries.${index}.headcount`} />
                                                                <ErrorMessage name={`countries.${index}.headcount`} />
                                                            </ListItemText>
                                                            <ListItemSecondaryAction>
                                                                <IconButton aria-label="Delete" onClick={() => arrayHelpers.remove(index)} tabIndex={-1}>
                                                                    <DeleteIcon />
                                                                </IconButton>
                                                            </ListItemSecondaryAction>
                                                        </ListItem>
                                                    );
                                                })}
                                            </List>
                                        )}
                                    />
                                </Form>
                                <Button variant='fab' className={classes.fab} color='primary' onClick={this.handleAddCountriesClick} >
                                    <AddIcon />
                                </Button>
                                <AddCountries open={addingCountries} onClose={this.handleDoneAddingCountries(values.countries)} existingCountryIds={countryIds} />
                            </React.Fragment>
                        );
                    }}
                />
            </Paper>
        );
    }
}

export default compose(
    withRouter,
    withStyles(styles),
    withQuery(query, {selector: 'proposal',}, NotFound),
    graphql(mutation, {
        name: 'updateProposalCountries',
        props: ({updateProposalCountries, ownProps: {history, id}}) => ({
            onSubmit: values => {
                const proposalCountries = schema
                    .validateSync(values) //validation has already ocurred...this is necessary to get typecast values
                    .countries.map(country => ({
                        countryId: country.countryId,
                        headcount: country.headcount
                    }));
                updateProposalCountries({variables: {id, proposalCountries}})
                    .then(() => history.push(Locations.Proposal.toUrl({id})));
            },
            onClose: () => history.push(Locations.Proposal.toUrl({id}))
        }),
    }),
)(ProposalCountries);