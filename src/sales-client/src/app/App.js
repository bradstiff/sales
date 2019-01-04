import React, { Component } from 'react';
import { BrowserRouter, Switch, Route, Redirect } from 'react-router-dom';
import ApolloClient from 'apollo-boost';
import { ApolloProvider } from 'react-apollo';

import CssBaseline from '@material-ui/core/CssBaseline';
import { MuiThemeProvider, createMuiTheme } from '@material-ui/core/styles';
import lightBlue from '@material-ui/core/colors/lightBlue';
import orange from '@material-ui/core/colors/orange';

import { QueryProgressProvider } from './QueryProgressContext';
import Proposals from '../proposal/Proposals';
import Proposal from '../proposal/Proposal';
import ProposalCountries from '../proposal/ProposalCountries';
import HrProduct from '../proposal/HrProduct';
import NotFound from './NotFound';
import Locations from './Locations';
import ErrorPage from './Error';
import ErrorBoundary from '../common/ErrorBoundary';

import logo from './logo.svg';
import './App.css';


const client = new ApolloClient({
});

const theme = createMuiTheme({
palette: {
  type: 'dark',
  primary: lightBlue,
  secondary: orange,
  background: {
      paper: '#343434',
      default: '#282828',
  },
  divider: 'rgba(230, 230, 230, 0.12)',
},
});

class App extends React.Component {
  render() {
    return (
        <ApolloProvider client={client} >
            <CssBaseline>
                <MuiThemeProvider theme={theme}>
                    <ErrorBoundary component={ErrorPage}>
                        <BrowserRouter>
                                <QueryProgressProvider>
                                    <Switch>
                                        {Locations.Proposals.toRoute({ component: Proposals, invalid: NotFound}, true)};
                                        {Locations.Proposal.toRoute({ component: Proposal, invalid: NotFound}, true)};
                                        {Locations.ProposalCountries.toRoute({ component: ProposalCountries, invalid: NotFound}, true)};
                                        {Locations.ProposalHr.toRoute({ component: HrProduct, invalid: NotFound}, true)};
                                        <Route component={NotFound} />
                                    </Switch>
                                </QueryProgressProvider>
                        </BrowserRouter>
                    </ErrorBoundary>
                </MuiThemeProvider>
            </CssBaseline>
        </ApolloProvider>
    );
  }
}

export default App;
