import * as Yup from 'yup';
import Location from 'react-app-location';

const isNullableDate = Yup.string().test('is-date', '${path}:${value} is not a valid date', date => !date || !isNaN(Date.parse(date))); 
const string = Yup.string();
const integer = Yup.number().integer();
const naturalNbr = integer.moreThan(-1);
const wholeNbr = integer.positive();
const identity = wholeNbr.required();
const order = Yup.string().oneOf(['asc', 'desc']).default('asc');

export default {
    Home: new Location('/'),
    Proposals: new Location('/proposals', null, {
        page: naturalNbr.default(1),
        rowsPerPage: Yup.number().oneOf([25, 50, 75, 100]).default(25),
        orderBy: Yup.string().oneOf(['name', 'clientName']).default('clientName'), 
        order: order
    }),
    Proposal: new Location('/proposals/:id', {id: identity}),
};
