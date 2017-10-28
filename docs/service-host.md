# Service Host

The `ServiceHost` is heavely inspired by the `WebHost` classes, used to configure ASP.NET Core applications.

A service host is created by defining a `ServiceHostBuilder`, configuring it and finally building the host.

```csharp
var serviceHost = new ServiceHostBuilder(new TimeBootstrap())
    .UseConsoleHost()
    .Build();

await serviceHost.RunAsync();
```

## Hosting ASP.NET Core

The package `Concept.Service.AspNetCore` contains classes that makes it possible to host an ASP.NET application together with the service. This can be great in a microservice architecture where each service exposes an API as well as underlying, event based business logic.

Getting up and running is fairly easy. Make sure that the bootstrap inherits from `AspNetCoreBootstrap`. This is an extended bootstrap that, in addition to normal bootstrapping, contains methods similar to the ones in the `Startup` class.

```csharp
public class FooBootstrap : AspNetCoreBootstrap<FooService>
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services
      .AddSingleton<FooService>()
      .AddLogging()
      .AddMvc();
  }

  public override void ConfigureAppConfiguration(IConfigurationBuilder configuration)
  {
    configuration.AddJsonFile("appsettings.json");
  }

  public override void Configure(IApplicationBuilder app, IHostingEnvironment env)
  {
    if (env.IsDevelopment())
      app.UseDeveloperExceptionPage();

    app.UseMvc();
  }
}
```

With the updated bootstrapper, the service host builder can define multiple hosts

```csharp
public static async Task MainAsync(string[] args)
{
  var host = new ServiceHostBuilder(new OpinionatedFooBootstrap())
    .UseConsoleHost()
    .UseWebHost()
    .Build();

  await host.RunAsync();
}
```