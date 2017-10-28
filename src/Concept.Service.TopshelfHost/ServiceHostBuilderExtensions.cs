using System;
using Concept.Service.HostBuilder;
using Microsoft.Extensions.DependencyInjection;

namespace Concept.Service.TopshelfHost
{
  public static class ServiceHostBuilderExtensions
  {
    public static IServiceHostBuilder UseTopshelfHost(this IServiceHostBuilder builder)
    {
      builder.ConfigureServices(collection => collection.AddSingleton<IServiceEntryPoint, TopshelfHostEntryPoint>());
      return builder;
    }
  }
}
