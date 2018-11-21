import React from 'react';

const fillerStyle = {
    flexGrow: 1
};

export default ({children}) => (
    <React.Fragment>
        <div style={fillerStyle}></div>
        {children}
    </React.Fragment>
)