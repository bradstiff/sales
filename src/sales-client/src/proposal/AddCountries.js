import React from 'react';
import {  compose } from 'react-apollo';
import gql from 'graphql-tag';

import AppBar from '@material-ui/core/AppBar';
import Dialog from '@material-ui/core/Dialog';
import Slide from '@material-ui/core/Slide';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';
import IconButton from '@material-ui/core/IconButton';
import CloseIcon from '@material-ui/icons/Close';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import ExpansionPanel from '@material-ui/core/ExpansionPanel';
import ExpansionPanelDetails from '@material-ui/core/ExpansionPanelDetails';
import ExpansionPanelSummary from '@material-ui/core/ExpansionPanelSummary';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Checkbox from '@material-ui/core/Checkbox';
import { withStyles } from '@material-ui/core/styles';

import NotFound from '../app/NotFound';
import withQuery from '../common/withQuery';
import { objectCompare } from '../common/SortHelper';

const styles = theme => ({
    appBar: {
        position: 'relative',
    },
    flex: {
        flex: 'auto',
    },
});

const query = gql`
    query Countries {
        countries {
            id,
            name,
            regionName
        }
    }
`;

const SlideUp = props => (<Slide direction='up' {...props} />);
const NoTransition = props => (
    <React.Fragment>
        {props.children}
    </React.Fragment>
)

class Region extends React.PureComponent {
    handleExpandedChange = (event, expanded) => {
        const {onExpandedChange, region} = this.props;
        onExpandedChange(region.name, expanded);
    }

    render() {
        const {region, expanded, onToggleCountry} = this.props;
        return (
            <ExpansionPanel expanded={expanded} onChange={this.handleExpandedChange}>
                <ExpansionPanelSummary expandIcon={<ExpandMoreIcon />}>
                    <Typography>{region.name}</Typography>
                </ExpansionPanelSummary>
                <ExpansionPanelDetails style={{flexDirection: 'column'}}>
                    {region.countries.map(country => (
                        <Country
                            key={country.id}
                            country={country}
                            onToggle={onToggleCountry}
                        />
                    ))}
                </ExpansionPanelDetails>
            </ExpansionPanel>                    
        );
    }
}

class Country extends React.PureComponent {
    state = {
        checked: false,
    }
    handleToggle = event => {
        const {country, onToggle} = this.props;
        this.setState({
            checked: !this.state.checked
        });
        onToggle(country.id);
    }
    render() {
        const {country} = this.props;
        return (
            <div>
                <FormControlLabel
                    control={
                        <Checkbox
                            checked={this.state.checked}
                            tabIndex={-1}
                            disableRipple
                            onChange={this.handleToggle}
                        />
                    }
                    label={country.name}
                />
            </div>
        );
    }
}

class AddCountries extends React.Component{
    state = {
        expandedRegion: null,
    };
    
    regions = null;
    addedCountryIds = [];

    handleCancel = () => {
        this.props.onClose();
    }

    handleSave = () => {
        const { onClose, countries} = this.props;
        const addedCountries = this.addedCountryIds.map(id => ({
            id,
            name: countries.find(country => country.id === id).name,
        }));
        onClose(addedCountries);
    }

    handleExpandedRegionChange = (name, expanded) => {
        this.setState({
            expandedRegion: expanded ? name : false,
        });
    }

    handleToggleCountry = id => {
        const currentIndex = this.addedCountryIds.indexOf(id);
    
        if (currentIndex === -1) {
            this.addedCountryIds.push(id);
        } else {
            this.addedCountryIds = this.addedCountryIds.splice(currentIndex, 1);
        }
    };    

    componentDidUpdate() {
        if (this.regions !== null) {
            return;
        }

        const {existingCountryIds, countries} = this.props;
        this.regions = countries
            .filter(country => !existingCountryIds.includes(country.id))
            .reduce((acc, country) => {
                let region = acc.find(r => r.name === country.regionName);
                if (!region) {
                    region = {name: country.regionName, countries: []};
                    acc.push(region);
                }
                region.countries.push(country);
                return acc;
            }, []);
            
        this.regions.sort((a, b) => objectCompare(a, b, 'name', 'name'));
        this.regions.forEach(region => region.countries.sort((a,b) => objectCompare(a, b, 'name', 'id')));

        if (this.regions.length) {
            this.setState({
                expandedRegion: this.regions[0].name
            });
        }
    }

    render() {
        const {open, classes} = this.props;
        const {expandedRegion} = this.state;
        const regions = this.regions || [];
        return (
            <Dialog
                fullScreen
                open={open}
                onClose={this.handleCancel}
                TransitionComponent={SlideUp}
            >
                <AppBar className={classes.appBar}>
                    <Toolbar>
                        <IconButton color="inherit" onClick={this.handleCancel} aria-label="Close">
                            <CloseIcon />
                        </IconButton>
                        <Typography variant="h6" color="inherit" className={classes.flex}>
                            Add Countries
                        </Typography>
                        <Button color="inherit" onClick={this.handleSave}>
                            save
                        </Button>
                    </Toolbar>
                </AppBar>
                {regions.map(region => (
                    <Region 
                        key={region.name}
                        region={region} 
                        expanded={expandedRegion === region.name}
                        onExpandedChange={this.handleExpandedRegionChange} 
                        onToggleCountry={this.handleToggleCountry} 
                    />
                ))}
            </Dialog>                
        );
    }
}

export default compose(
    withStyles(styles),
    withQuery(query, {selector: 'countries', variables: {}}, NotFound),
)(AddCountries);
