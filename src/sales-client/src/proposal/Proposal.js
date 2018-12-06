import React from 'react';
import { compose } from 'react-apollo';
import gql from 'graphql-tag';

import Paper from '@material-ui/core/Paper';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import ListItemSecondaryAction from '@material-ui/core/ListItemSecondaryAction';

import Locations from '../app/Locations';
import NotFound from '../app/NotFound';
import withQuery from '../common/withQuery';
import FlexRightAligned from '../common/FlexRightAligned';

export const proposalPageFragment = gql`
    fragment proposalFragment on Proposal {
        id,
        name,
        clientName,
        countries {
            id,
            name,
            headcount,
        }
    }
`;

const query = gql`
    query Proposal($id: Int!) {
        proposal(id: $id) {
            ...proposalFragment
        }
    }
    ${proposalPageFragment}
`;

const Proposal = ({proposal, id}) => (
    <Paper>
        <Typography variant='title'>{proposal.name}</Typography>
        <Toolbar>
            <Typography variant='title'>Scope</Typography>
            <FlexRightAligned>
                <Button href={Locations.ProposalCountries.toUrl({id})}>Countries</Button>
            </FlexRightAligned>
        </Toolbar>
        <List>
            {proposal.countries.map(country => (
                <ListItem key={country.id}>
                    <ListItemText>{country.name}</ListItemText>
                    <ListItemSecondaryAction>{country.headcount}</ListItemSecondaryAction>
                </ListItem>
            ))}
        </List>
    </Paper>
);

export default compose(
    withQuery(query, {
        selector: 'proposal',
    }, NotFound),
)(Proposal);