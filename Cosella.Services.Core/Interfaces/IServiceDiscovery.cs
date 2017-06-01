using Cosella.Services.Core.Communications;
using System.Threading.Tasks;

namespace Cosella.Services.Core.Interfaces
{
    public interface IServiceDiscovery
    {
        IServiceRegistration RegisterService();

        IServiceRegistration RegisterService(Task<ApiClientResponse<string>> registrationTask);

        Task<ApiClientResponse<string>> RegisterServiceDeferred();

        void DeregisterService(IServiceRegistration registration);

        IServiceRegistration FindServiceByName(string serviceName);

        IServiceRegistration FindServiceByInstanceId(string serviceName);

        Task<IServiceInfo[]> ListServices();
    }
}