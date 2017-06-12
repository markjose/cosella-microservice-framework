import serviceData from './data/services.json';

const listServices = () => {
    return serviceData;
};

export const services = {
    list: () => listServices()
};
