import { get, setWith, clone, pick, isObject } from 'lodash';

export const immutableSet = (object = {}, path, value) => setWith(clone(object), path, value, (nsValue, key, nsObject) => {
    return nsValue === undefined
        ? {} //override lodash default creation of arrays for numeric keys
        : clone(nsValue);
});

export const immutableUpdate = (object = {}, path, updateFn) => {
    const objectToUpdate = clone(get(object, path));
    updateFn(objectToUpdate);
    return immutableSet(object, path, objectToUpdate);
}

const deepPaths = (object = {}, basePath) => Object.entries(object).reduce(
    (result, [key, value]) => { 
        const path = basePath
            ? [basePath, key].join('.')
            : key;
        return isObject(value) 
            ? result.concat(deepPaths(value, path)) 
            : result.concat(path)}, 
    []
);

export const deepPickBy = (object, pickBy) => pick(object, deepPaths(pickBy));
