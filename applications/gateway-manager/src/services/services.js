import serviceData from './data/services.json';

const useMocks = false;
const baseUrl = window.cosella.baseUrl || '';

const fetchSettings = {
    headers: new Headers({
        'Content-Type': 'application/json'
    }),
    mode: 'cors',
    credentials: 'omit'
};

const listServices = () => {
    return useMocks ? 
        new Promise(resolve => resolve(serviceData)) : 
        fetch(`${baseUrl}services?includeDescriptors=true`, {...fetchSettings, method: 'GET'})
            .then(response => {
                if(response.ok) {
                    return response.json();
                }
                throw new Error('Failed to fetch services');
            })
            .then(json => json);
};

const proxyRequest = (serviceName, pathKey, requestQuery, body) => {

    const [method, url] = pathKey.split('|');
    let queryString = '';
    if(Object.keys(requestQuery).length > 0) {
        queryString = '?' + Object.keys(requestQuery)
            .map(key => `${key}=${requestQuery[key]}`)
            .join('&');
    }

    const proxyRequest = {
        serviceName,
        method,
        url,
        queryString,
        body
    };

    return useMocks ? 
        new Promise(resolve => resolve({ statusCode: 200, body: "Mock Data" })) : 
        fetch(`${baseUrl}services/proxy`, {...fetchSettings, method: 'POST', body: JSON.stringify(proxyRequest)})
            .then(response => {
                if(response.ok) {
                    return response.json();
                }
                return {
                    ...response
                };
            });
};

export const services = {
    list: () => listServices(),
    proxyRequest
};
