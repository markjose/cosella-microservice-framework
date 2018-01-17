# Getting Started

### Prerequisites

- [.NET Framework 4.7.1 targeting pack or SDK](https://www.microsoft.com/net/download/visual-studio-sdks)
- [Consul](https://www.consul.io/downloads.html) - start as local development agent
```./consul.exe agent -dev```

### Using the framework

1. Open Visual Studio (not required, you can use VS Code or whichever IDE you normally use for .NET development)
2. Create a new 'Console App (Net Framework)' project.
3. Include the [Cosella.Framework.Core](https://www.nuget.org/packages/Cosella.Framework.Core/) package from the NuGet package manager
4. Include the [Microsoft.Owin.Host.HttpListener](https://www.nuget.org/packages/Microsoft.Owin.Host.HttpListener/) package from the NuGet package manager (this is not pulled in
automagically)
5. In your ```Main()``` method, create your microservice like this:
```
static int Main(string[] args)
{
	return MicroService
		.Configure(config =>
		{            
			config.ServiceName = "MyService";
			config.ServiceDisplayName = "My First Cosella Microservice";

			// If you didn't start consul, you'll need to add a port number
			// config.RestApiPort = 5000;

			// ...and disable service registration and discovery
			// config.DisableRegistration = true;
			// config.DisableServiceDiscovery = true;
		})
		.Run();
}
```
6. Choose Debug -> Start Debugging or hit F5 to start the services
7. Browse to the address shown in the console window (Typically ```http://localhost:5000```)