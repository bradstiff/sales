/* ********************************************************************************************************************************
 * An HOC that requests a query, selects a value from the resulting data, and passes it as a prop.
 * options.variables can be a function of props, a literal object, or undefined.
 * If variables is undefined, props will be passed as variables. Apollo ignores variable values that are not used by the query.
 * 
 * If the query completes and the selected value is null, renders the indicated NotFound component.
 * 
 * If the query completes and the error prop is set, throws to ErrorBoundary.
 *
 * Eliminates a bunch of boilerplate.
 * ********************************************************************************************************************************/

import React from 'react';
import PropTypes from 'prop-types';
import { Query } from 'react-apollo';
import { QueryProgressConsumer } from '../app/QueryProgressContext';

const withQuery = (query, options, NotFound) => Component => {
    class ComponentWithLoadingEvents extends React.Component {
        state = {
            loading: false,
        };

        render() {
            const { data, loading, error, wrappedComponentRef, ...remainingProps } = this.props;
            if (error) {
                console.log(error);
                throw error;
            }

            const root = data[options.selector];
            if (root === undefined) {
                return null;
            }
            if (!loading && root === null) {
                return <NotFound />;
            }

            const wrappedComponentProps = {
                ...remainingProps,
                [options.selector]: root,
                ref: wrappedComponentRef
            };
            return <Component  {...wrappedComponentProps} />;
        }

        static getDerivedStateFromProps(props, state) {
            if (props.loading && !state.loading) {
                props.onStartLoading();
                return { loading: true };
            } else if (!props.loading && state.loading) {
                props.onEndLoading();
                return { loading: false };
            }
            return null;
        }
    }

    const ComponentWithQuery = props => {
        if (typeof options === 'string') {
            options = { selector: options };
        }
        const variables = typeof options.variables === 'function'
            ? options.variables(props)
            : options.variables || props;
        return <Query query={query} variables={variables}>
            {({ data, loading, error }) => (
                <QueryProgressConsumer>
                    {({ onStart, onEnd }) => (
                        <ComponentWithLoadingEvents
                            {...props}
                            data={data}
                            loading={loading}
                            error={error}
                            onStartLoading={onStart}
                            onEndLoading={onEnd}
                        />
                    )}
                </QueryProgressConsumer>
            )}
        </Query>;
    };

    ComponentWithQuery.displayName = `withQuery(${Component.displayName || Component.name})`;
    ComponentWithQuery.WrappedComponent = Component;
    ComponentWithQuery.propTypes = {
        wrappedComponentRef: PropTypes.func
    };

    return ComponentWithQuery;
};

export default withQuery;
