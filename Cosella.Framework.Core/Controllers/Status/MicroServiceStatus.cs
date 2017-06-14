using System.Reflection;

namespace Cosella.Framework.Core.Controllers.Status
{
    public class MicroServiceStatus
    {
        public string Name { get; internal set; }
        public string Version { get; internal set; }

        public static MicroServiceStatus Latest
        {
            get
            {
                var assemblyName = Assembly.GetEntryAssembly().GetName();

                return new MicroServiceStatus()
                {
                    Name = assemblyName.Name,
                    Version = assemblyName.Version.ToString()
                };
            }
        }
    }
}