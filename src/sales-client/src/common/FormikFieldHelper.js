import warning from 'warning';

//Automatically bind requested Mui field props to Formik API props. Reduces boilerplate.

export default (name, fieldPropKeys, formProps, otherProps) => {
    const boundProps = {
        name,
        ...(otherProps || {}),
    };
    for (const propKey of fieldPropKeys) {
        if (['value', 'checked'].includes(propKey)) {
            warning(formProps.values, "Formik 'values' prop not provided");
            boundProps[propKey] = formProps.values[name];
        } else if (propKey === 'error') {
            warning(formProps.touched, "Formik 'touched' prop not provided");
            warning(formProps.errors, "Formik 'errors' prop not provided");
            boundProps.error = formProps.touched[name] && !!formProps.errors[name];
        } else if (propKey === 'helperText') {
            warning(formProps.touched, "Formik 'touched' prop not provided");
            warning(formProps.errors, "Formik 'errors' prop not provided");
            boundProps.helperText = formProps.touched[name] ? formProps.errors[name] : null;
        } else if (propKey === 'onChange') {
            warning(formProps.handleChange, "Formik 'handleChange' prop not provided");
            boundProps.onChange = formProps.handleChange;
        } else if (propKey === 'onBlur') {
            if (formProps.setFieldTouched) {
                boundProps.onBlur = () => formProps.setFieldTouched(name);
            } else if (formProps.onBlur) {
                boundProps.onBlur = formProps.handleBlur;
            } else {
                warning(true, "Neither 'handleBlur' nor 'setFieldTouched' Formik props provided");
            }
        } else if (propKey === 'disabled') {
            warning(formProps.disabled !== undefined, "'disabled' prop not provided")
            boundProps.disabled = formProps.disabled;
        } else {
            warning(true, `'${propKey}' is not a supported fieldPropKey.`);
        }
    }
    return boundProps;
}

