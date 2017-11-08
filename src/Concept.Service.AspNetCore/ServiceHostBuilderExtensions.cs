using System;
using System.IO;
using System.Reflection;
using Concept.Logging;
using Concept.Service.AspNetCore.Abstractions;
using Concept.Service.HostBuilder.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Concept.Service.AspNetCore
{
  public static class ServiceHostBuilderExtensions
  {
    private static readonly ILog Logger = LogProvider.GetLogger(typeof(ServiceHostBuilderExtensions));

    public static IServiceHostBuilder UseWebHost(this IServiceHostBuilder builder, IWebHostBuilder existingWebHost = null)
    {
      Logger.Debug("Registering WebHost services.");
      builder.ConfigureServices(collection =>
      {
        collection.AddSingleton(provider =>
        {
          var serviceMetadata = provider.GetService<ServiceMetadata>();
          if (!(provider.GetService<IServiceBootstrap>() is IAspNetCoreBootstrap aspNetBootstrap))
          {
            Logger.Info("The service bootstrap {serviceBootstrap} does not implement IAspNetCoreBootstrap. WebHost will not be created", serviceMetadata.GetType().Name);
            throw new Exception("Bootstrap is not of type IAspNetCoreBootstrap.");
          }

          var webHostBuilder = existingWebHost ?? new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseSetting(WebHostDefaults.ApplicationKey, serviceMetadata.Type.GetTypeInfo().Assembly.FullName)
            .UseStartup(aspNetBootstrap.GetType());
          aspNetBootstrap.ConfigureHost(webHostBuilder);
          webHostBuilder
            .ConfigureAppConfiguration(aspNetBootstrap.ConfigureAppConfiguration)
            .ConfigureLogging(aspNetBootstrap.ConfigureLogging);
          var webHost = webHostBuilder.Build();

          aspNetBootstrap.CreateServiceFactory(() => webHost.Services.GetService(serviceMetadata.Type) as Service);
          return webHost;
        });

        collection.AddSingleton<IServiceEntryPoint, WebHostEntryPoint>();
      });
      return builder;
    }
  }
}

