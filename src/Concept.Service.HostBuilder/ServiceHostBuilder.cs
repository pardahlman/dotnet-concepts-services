using System;
using System.Data;
using Concept.Service.HostBuilder.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Concept.Service.HostBuilder
{
  public class ServiceHostBuilder : IServiceHostBuilder
  {
    private readonly IServiceCollection _serviceCollection;

    public ServiceHostBuilder(IServiceBootstrap bootstrap)
    {
      bootstrap.PreConfigureLogger();
      bootstrap.ConfigureLogger();
      bootstrap.PostConfigureLogger();

      _serviceCollection = new ServiceCollection();
      _serviceCollection
        .AddSingleton(new ServiceLifetime())
        .AddSingleton<IServiceLifetime>(provider => provider.GetService<ServiceLifetime>())
        .AddSingleton(bootstrap)
        .AddSingleton(bootstrap.CreateMetadata());
    }

    public IServiceHostBuilder ConfigureServices(Action<IServiceCollection> configure)
    {
      if (configure == null)
      {
        throw new NoNullAllowedException(nameof(configure));
      }
      configure(_serviceCollection);
      return this;
    }

    public IServiceHost Build()
    {
      return new ServiceHost(_serviceCollection.BuildServiceProvider());
    }
  }
  
}
