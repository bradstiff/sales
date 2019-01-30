import React from 'react';

import Paper from '@material-ui/core/Paper';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';
import MenuItem from '@material-ui/core/MenuItem';
import FormControl from '@material-ui/core/FormControl';
import Select from '@material-ui/core/Select';
import FormHelperText from '@material-ui/core/FormHelperText';
import Table from '@material-ui/core/Table';
import TableHead from '@material-ui/core/TableHead';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableRow from '@material-ui/core/TableRow';
import TextField from '@material-ui/core/TextField';
import NumericInput from '../common/NumericInput';
import FlexRightAligned from '../common/FlexRightAligned';

export default class PayrollScopeView extends React.Component {
    handleCountryBlur = (countryId, field) => {
        this.props.onBlur(`countries.${countryId}.${field}`);
    }
    render() {
        const {
            name, 
            payrollLevels, 
            values,
            errors, 
            isSubmitting, 
            canEdit, 
            onCountryLevelChange,
            onCountryPopulationChange,
            onCountrySelectedChange,
            onSubmit,
            onClose,
        } = this.props;
        return (
            <Paper>
                <form onSubmit={onSubmit} autoComplete="off">
                    <Toolbar>
                        <Typography variant='h5'>{name} Payroll Scope</Typography>
                        <FlexRightAligned>
                            <Button color='primary' disabled={isSubmitting} onClick={onClose}>{canEdit ? 'Cancel' : 'Close'}</Button>
                            {canEdit && <Button color='primary' variant='outlined' type='submit' disabled={isSubmitting}>Save</Button>}
                        </FlexRightAligned>
                    </Toolbar>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell />
                                <TableCell>Country</TableCell>
                                <TableCell>Level</TableCell>
                                <TableCell>Monthly</TableCell>
                                <TableCell>Semi-monthly</TableCell>
                                <TableCell>Bi-weekly</TableCell>
                                <TableCell>Weekly</TableCell>
                                <TableCell>Total</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {values.countries.map(country => (
                                <PayrollCountryScopeView
                                    key={country.id}
                                    country={country}
                                    errors={errors.countries && errors.countries[country.id]}
                                    levels={payrollLevels}
                                    onSelectedChange={onCountrySelectedChange}
                                    onLevelChange={onCountryLevelChange}
                                    onPopulationChange={onCountryPopulationChange}
                                    onBlur={this.handleCountryBlur}
                                />
                            ))}
                        </TableBody>
                    </Table>
                </form>
            </Paper>
        );
    }
    static defaultProps = {
        errors: {}
    }
}

class PayrollCountryScopeView extends React.PureComponent {
    handleLevelChange = event => {
        const { country, onLevelChange } = this.props;
        onLevelChange(country.id, event.target.value);
    }
    handlePopulationChange = event => {
        const { country, onPopulationChange } = this.props;
        onPopulationChange(country.id, event.target.name, event.target.value);
    }
    handleSelectedChange = event => {
        const { country, onSelectedChange } = this.props;
        onSelectedChange(country.id, event.target.checked);
    }
    handleBlur = event => {
        const { country, onBlur } = this.props;
        onBlur(country.id, event.target.name);
    }
    render() {
        const { country, levels, errors = {}} = this.props;
        const fieldProps = name => ({
            name,
            value: country[name],
            error: !!errors[name],
            helperText: errors[name],
            onBlur: this.handleBlur,
        })
        return (
            <TableRow key={country.id}>
                <TableCell>
                    <input type='checkbox' name='selected' checked={country.selected} onChange={this.handleSelectedChange} />
                </TableCell>
                <TableCell>{country.name}</TableCell>
                <TableCell>
                    <FormControl error={!!errors.levelId}>
                        <Select
                            value={country.levelId}
                            onChange={this.handleLevelChange}
                            onBlur={this.handleBlur}
                            name='levelId'
                        >
                            <MenuItem key="" value=""><em>Select</em></MenuItem>
                            <MenuItem key="0" value={0}>None</MenuItem>
                            {levels.map(level => <MenuItem key={level.id} value={level.id}>{level.name}</MenuItem>)}
                        </Select>
                        {errors.levelId && <FormHelperText>{errors.levelId}</FormHelperText>}
                    </FormControl>
                </TableCell>
                <TableCell>
                    <NumericInput {...fieldProps('monthlyPayees')} onChange={this.handlePopulationChange} disabled={!country.isInScope()} />
                </TableCell>
                <TableCell>
                    <NumericInput {...fieldProps('semiMonthlyPayees')} onChange={this.handlePopulationChange} disabled={!country.isInScope()} />
                </TableCell>
                <TableCell>
                    <NumericInput {...fieldProps('biWeeklyPayees')} onChange={this.handlePopulationChange} disabled={!country.isInScope()} />
                </TableCell>
                <TableCell>
                    <NumericInput {...fieldProps('weeklyPayees')} onChange={this.handlePopulationChange} disabled={!country.isInScope()} />
                </TableCell>
                <TableCell>
                    <TextField {...fieldProps('totalPayees')} value={country.totalPayees()} disabled={true} />
                </TableCell>
            </TableRow>
        );
    }
}