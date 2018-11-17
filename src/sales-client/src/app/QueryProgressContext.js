import React from 'react';
import ModalCircularProgress from '../common/ModalCircularProgress';

export const QueryProgressContext = React.createContext({
    inProgress: false,
    onStart: () => { },
    onEnd: () => { },
});

export class QueryProgressProvider extends React.Component {
    constructor(props) {
        super(props);

        //use state for context values instead of literal object to avoid remounting
        this.state = {
            inProgress: false,
            onStart: this.handleQueryStart,
            onEnd: this.handleQueryEnd,
        };
    }

    handleQueryStart = () => {
        if (!this.state.inProgress) {
            this.setState({ inProgress: true })
        }
    }

    handleQueryEnd = () => this.setState({ inProgress: false })

    render() {
        return (
            <QueryProgressContext.Provider value={this.state}>
                {this.props.children}
                <QueryProgressContext.Consumer>
                    {({ inProgress }) => <ModalCircularProgress open={inProgress} />}
                </QueryProgressContext.Consumer>
            </QueryProgressContext.Provider>
        );
    }
}

export const QueryProgressConsumer = QueryProgressContext.Consumer;