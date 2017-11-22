using Cosella.Framework.Core.ApiClient;
using System.Threading.Tasks;

namespace Cosella.Framework.Core.ServiceDiscovery
{
    public interface IServiceDiscovery
    {
        Task<IServiceRegistration> RegisterService();

        Task<IServiceRegistration> RegisterService(Task<ApiClientResponse<string>> registrationTask);

        Task<Task<ApiClientResponse<string>>> RegisterServiceDeferred();

        void DeregisterService(IServiceRegistration registration);

        Task<IServiceInstanceInfo> FindServiceByName(string serviceName);

        Task<IServiceInstanceInfo> FindServiceByInstanceName(string instanceName);

        Task<IServiceInfo[]> ListServices();
    }
}