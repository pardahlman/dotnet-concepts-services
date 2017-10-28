using Microsoft.Extensions.DependencyInjection;

namespace Concept.Service.HostBuilder
{
  public static class ServiceHostBuilderExtension
  {
    public static IServiceHostBuilder UseBootstrap<TService>(this IServiceHostBuilder builder, ServiceBootstrap<TService> bootstrap) where TService : Service
    {
      builder.ConfigureServices(collection => collection
        .AddSingleton<ServiceBootstrap>(bootstrap)
        .AddSingleton(bootstrap.CreateMetadata())
      );
      return builder;
    }
  }
}
