using Concept.Service.HostBuilder.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Concept.Service.ConsoleHost
{
  public static class ServiceHostBuilderExtension
  {
    public static IServiceHostBuilder UseConsoleHost(this IServiceHostBuilder builder)
    {
      builder.ConfigureServices(
        collection => collection.AddSingleton<IServiceEntryPoint, ConsoleHostEntryPoint>()
      );
      return builder;
    }
  }
}
