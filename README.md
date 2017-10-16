# Concept: Services

> Conceptual classes for handling lifetime events of hosting agnostic services

## Example

```csharp
// Inherit from `Service` and override `StartAsync`
public class ExampleServer : Service
{
  private readonly IChecker _checker;

  // dependency inject
  public ExampleServer(IChecker checker)
  {
    _checker = checker;
  }

  // trigger starting actions
  public override async Task StartAsync(CancellationToken ct = default(CancellationToken))
  {
    await _checker.CheckAsync();
  }
}
```

Create a bootstrapper for the service

```csharp
public class ExampleServiceBootstrap : ServiceBootstrap<ExampleServer>, IDisposeable
{
  private IContainer _autofacContainer;

  public override ServiceMetadata CreateMetadata()
  {
    return new ServiceMetadata
    {
      Name = nameof(ExampleServer),
      Description = "Nothing to fancy"
    };
  }

  public override void ConfigureLogger()
  {
    Log.Logger = new LoggerConfiguration()
      .WriteTo.LiterateConsole()
      .Enrich.FromLogContext()
      .CreateLogger();
  }

  public override void RegisterDependencies()
  {
    var builder = new ContainerBuilder();
    builder
      .RegisterType<Checker>()
      .AsImplementedInterfaces();
    _autofacContainer = builder.Build();
  }

  public override ExampleServer CreateService()
  {
    return _autofacContainer.Resolve<ExampleServer>();
  }
}
```

Run it

```csharp
var bootstrap = new ExampleServiceBootstrap();

// run it in as a console app
ConsoleRunner.Start(bootstrap);

// or a topshelf service
TopshelfRunner.Start(bootstrap);
```

## Opinionated service

If you are like me like [Serilog](https://serilog.net/) and [Autofac](https://autofac.org/) you can use the `OpinionatedServiceBootstrap` for a less verbose bootstrapper

```csharp
public class ExampleServiceBootstrap : OpinionatedServiceBootstrap<ExampleServer>
{
  public override ServiceMetadata CreateMetadata()
  {
    return new ServiceMetadata
    {
      Name = nameof(ExampleServer),
      Description = "Nothing to fancy"
    };
  }

  protected override void RegisterDependencies(ContainerBuilder builder)
  {
    builder
      .RegisterType<Checker>()
      .AsImplementedInterfaces();
  }
}
```