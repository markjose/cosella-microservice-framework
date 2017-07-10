# Cosella

Cosella is a framework that helps get rid of the boilerplate code required for running a Microservice architecture.

## Getting Started

### Prerequisites

* Consul
* .NET Framework 4.6.2

### Starting the services

1. Open a terminal and start up Consul

```
consul agent -dev
```

2. Check out the repository to your local machine
3. Start up Visual Studio running as administrator
4. Go to File -> Open -> Project/Solution, navigate to the solution file and click "Open"
5. Right-click on solution in Solution Explorer and select properties from the dropdown
6. Change the radio button to "Multiple startup projects"
7. Select either "Start" or "Start without debugging" for all the "Cosella.Services.*" projects and click "Ok"
8. Choose Debug -> Start Debugging or hit F5 to start the services
