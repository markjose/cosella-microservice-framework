import serviceData from './data/services.json';

const useMocks = false;

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
        fetch('http://localhost:5000/services?includeDescriptors=true', {...fetchSettings, method: 'GET'})
            .then(response => {
                if(response.ok) {
                    return response.json();
                }
                throw new Error('Failed to fetch services');
            })
            .then(json => json);
};

export const services = {
    list: () => listServices()
};
