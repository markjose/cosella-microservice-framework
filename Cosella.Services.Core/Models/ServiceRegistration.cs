using Cosella.Services.Core.Interfaces;

namespace Cosella.Services.Core.Models
{
    internal class ServiceRegistration : IServiceRegistration
    {
        public string InstanceName { get; set; }
        public string ApiUrl { get; set; }
    }
}