const valueCompare = (val1, val2) => {
    const isUncomparable = val => val === undefined || val === null;

    if (isUncomparable(val1) && isUncomparable(val2)) {
        return 0;
    } else if (isUncomparable(val1)) {
        return -1;
    } else if (isUncomparable(val2)) {
        return 1;
    } else {
        return (typeof val1 === 'string' && typeof val2 === 'string')
            ? val1.localeCompare(val2, 'en', { sensitivity: 'base' })
            : val1 - val2;
    }
}

export const objectCompare = (obj1, obj2, compareField, keyField) => {
    const compare = valueCompare(obj1[compareField], obj2[compareField]);
    if (compare === 0) {
        //for sort stability, if the values are equal, use the key as a tie-breaker
        return valueCompare(obj1[keyField].toString(), obj2[keyField].toString());
    }
    return compare;
}
