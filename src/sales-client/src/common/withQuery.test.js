import React from 'react';
import gql from 'graphql-tag';
import withQuery from './withQuery';
import { QueryProgressContext } from '../app/QueryProgressContext';

import { MockedProvider } from 'react-apollo/test-utils';
import { render, cleanup, wait } from 'react-testing-library';

const query = gql`
    query getDog($name: String!) {    
        dog(name: $name) {
            id,
            name,
            breed
        }
    }
`;

const mocks = [
    {
        request: {
            query,
            variables: {
                name: 'Buck',
            },
        },
        result: {
            data: {
                dog: { id: '1', name: 'Buck', breed: 'bulldog' },
            },
        },
    },
    {
        request: {
            query,
            variables: {
                name: 'Buddy',
            },
        },
        result: {
            data: {
                dog: null,
            },
        },
    },
];

const NotFound = () => <div>Not found</div>;
const TestComponent = ({ dog }) => <div>{dog.name}</div>
let queryInProgress = false;

afterEach(() => {
    queryInProgress = false;
    cleanup();
});

test('runs query passing props as variables, and renders component with injected dog prop', async () => {
    const ComponentWithQuery = withQuery(query, 'dog', NotFound)(TestComponent);
    const { container, debug } = render((
        <MockedProvider mocks={mocks} addTypename={false}>
            <ComponentWithQuery name='Buck' />
        </MockedProvider>
    ));

    await wait(() => {
        expect(container.innerHTML).toMatch('Buck');
    });
});

test('runs query using hard-coded variables, and renders component with injected dog prop', async () => {
    const options = {
        selector: 'dog',
        variables: { name: 'Buck' }
    };

    const ComponentWithQuery = withQuery(query, options, NotFound)(TestComponent);
    const { container, debug } = render((
        <MockedProvider mocks={mocks} addTypename={false}>
            <ComponentWithQuery />
        </MockedProvider>
    ));

    await wait(() => {
        expect(container.innerHTML).toMatch('Buck');
    });
});

test('runs query using variables as function of props, and renders component with injected dog prop', async () => {
    const options = {
        selector: 'dog',
        variables: (props) => ({ name: props.name })
    };

    const ComponentWithQuery = withQuery(query, options, NotFound)(TestComponent);
    const { container, debug } = render((
        <MockedProvider mocks={mocks} addTypename={false}>
            <ComponentWithQuery name='Buck' />
        </MockedProvider>
    ));

    await wait(() => {
        expect(container.innerHTML).toMatch('Buck');
    });
});

test('runs query with no results, renders NotFound component', async () => {
    const ComponentWithQuery = withQuery(query, 'dog', NotFound)(TestComponent);
    const { container, debug } = render((
        <MockedProvider mocks={mocks} addTypename={false}>
            <ComponentWithQuery name='Buddy' />
        </MockedProvider>
    ));

    await wait(() => {
        expect(container.innerHTML).toMatch('Not found');
    });
});

test('runs query, triggering start and end states', async () => {
    class MockedQueryProgressProvider extends React.Component {
        constructor(props) {
            super(props);

            this.state = {
                inProgress: false,
                onStart: this.handleQueryStart,
                onEnd: this.handleQueryEnd,
            };
        }

        handleQueryStart = () => {
            if (!this.state.inProgress) {
                this.setState({ inProgress: true })
                queryInProgress = true;
            }
        }

        handleQueryEnd = () => {
            this.setState({ inProgress: false });
            queryInProgress = false;
        }

        render() {
            return (
                <QueryProgressContext.Provider value={this.state}>
                    {this.props.children}
                </QueryProgressContext.Provider>
            );
        }
    }

    const ComponentWithQuery = withQuery(query, 'dog', NotFound)(TestComponent);
    const { container, debug } = render((
        <MockedProvider mocks={mocks} addTypename={false}>
            <MockedQueryProgressProvider>
                <ComponentWithQuery name='Buck' />
            </MockedQueryProgressProvider>
        </MockedProvider>
    ));

    expect(queryInProgress).toBe(true);
    await wait(() => {
        expect(container.innerHTML).toMatch('Buck');
        expect(queryInProgress).toBe(false);
    });
});

