using Cosella.Framework.Client.ApiClient;
using System.Threading.Tasks;

namespace Cosella.Framework.Client.Interfaces
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