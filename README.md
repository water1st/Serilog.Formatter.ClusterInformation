# Serilog.Formatter.ClusterInformation

### Getting started
To use the cluster information formatter, first install the [NuGet package](https://www.nuget.org/packages/Serilog.Formatter.ClusterInformation/):

```powershell
Install-Package Serilog.Formatter.ClusterInformation
``` 
Next, install http and console provider or you can install other provider if you need

```powershell
Install-Package Serilog.Sinks.Http.Formatters.Extension
Install-Package Serilog.Sinks.Console
```

Next, register to IServiceCollection
```csharp
services.AddSerilog(builder => {
    builder.LoggerConfiguration.WriteTo.Http(
    requestUri: "http://localhost:8080",
    batchPostingLimit: 1,
    textFormatter: Serilog.Formatter.ClusterInformation.Formatters.ClusterInformationFormatter.Instance, 
    batchFormatter: Serilog.Sinks.Http.BatchFormatters.SingleLogBatchFormatter.Instance);
    builder.LoggerConfiguration.WriteTo.Console();

    builder.LogOptions.ApplicationName="app1";
    builder.LogOptions.ContainerId="foo";
    builder.LogOptions.Host="localhost";
    builder.LogOptions.Environment="Development";
    builder.LogOptions.Version="1.0";
});
```

#### Serilog.Settings.Configuration support

Register to IServiceCollection
```csharp
services.AddSerilog(configuration);
```

Add the cluster information to appsettings.json

```json
{
  "Serilog": {
    "MinimumLevel": "Verbose",
    "WriteTo": [
      {
        "Name": "Http",
        "Args": {
          "requestUri": "http://localhost:8080",
          "batchPostingLimit": "1",
          "textFormatter": "Serilog.Formatter.ClusterInformation.Formatters.ClusterInformationFormatter::Instance, Serilog.Formatter.ClusterInformation",
          "batchFormatter": "Serilog.Sinks.Http.BatchFormatters.SingleLogBatchFormatter::Instance, Serilog.Sinks.Http.Formatters.Extension"
        }
      },
      { "Name": "Console" }
    ],
    "ClusterInformation":{
        "ApplicationName":"app1",
        "ContainerId":"foo",
        "Host":"localhost",
        "Environment":"Development",
        "Version":"1.0"
    }
  }
}
```
 or environment variables
```
Serilog__ClusterInformation__ApplicationName=app1
Serilog__ClusterInformation__ContainerId=foo
Serilog__ClusterInformation__Host=localhost
Serilog__ClusterInformation__Environment=Development
Serilog__ClusterInformation__Version=1.0
```
or code
```csharp
services.AddSerilog(configuration,(options,hostInfo)=>{
    options.ApplicationName = configuration["app1"];
    options.ContainerId = hostInfo.GetContainerID();
    options.Host = hostInfo.GetCurrentHost();
    options.Environment = configuration["ASPNETCORE_ENVIRONMENT"];
    options.Version = configuration["Version"];
});
```
