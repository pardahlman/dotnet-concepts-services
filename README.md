# Concept: Services

> Conceptual classes for handling lifetime events of hosting agnostic services

* Targets .NET Standard 2.0, runs on .NET Core
* Clear separation between service, bootstrapper and host
* No-dependency base libraries for custom implementations, add on libraries with opinionated defaults
* Host hybrid service with ASP.NET Core

:blue_book: Full documentation at [`dotnet-concepts-services.readthedocs.io`](http://dotnet-concepts-services.readthedocs.io/en/latest/).

## Create Service

A service is defined by implementing a class derived from `Service`. The method `StartAsync` will be called when the service is started, and can be considered as the entry point to the service. Optionally, `StopAsync` can be overridden to implement clean up activities, like disposing services.

```csharp
public class TimeService : Service
{
  private readonly IWorldClock _clock;
  private Timer _timer;

  public TimeService(IWorldClock clock)
  {
    _clock = clock;
  }

  public override async Task StartAsync(CancellationToken ct = default(CancellationToken))
  {
    _timer = new Timer(time =>
    {
      Log.Information("It is {timeOfDay}, and all is well", _clock.GetTime());
    }, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
  }
}
```

## Create Bootstrap

The `IServiceBootstrap` is responsible for configuring the applicatoin logger and wire-up the dependency injection container. It is not primed to any specific frameworks, as the interface [only contains hooks](https://github.com/pardahlman/dotnet-concepts-services/blob/master/src/Concept.Service/ServiceBootstrap.cs#L7). For convinience, there are implementations that wire up different populare libraries.

The `OpinionatedServiceBootstrap` configures a [Serilog](https://serilog.net/) logger and creates an [Autofac](https://autofac.org/) container to register services in.

```csharp
public class TimeBootstrap : OpinionatedServiceBootstrap<TimeService>
{
  public override ServiceMetadata CreateMetadata()
  {
    return new ServiceMetadata
    {
      Type = typeof(TimeService),
      Name = nameof(TimeService),
      Description = "Tells the time"
    };
  }

  protected override void RegisterDependencies(ContainerBuilder builder)
  {
    builder
      .RegisterType<WorldClock>()
      .AsImplementedInterfaces();
    builder
      .RegisterType<TimeService>()
      .AsSelf();
  }
}
```

## Run the service

The service can be run in a few different ways. The most straight forward option is to use the `ConsoleRuner`

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        MainAsync(args).GetAwaiter().GetResult();
    }

    public static async Task MainAsync(string[] args)
    {
        await ConsoleRunner.StartAsync(new TimeBootstrap());
    }
}
```