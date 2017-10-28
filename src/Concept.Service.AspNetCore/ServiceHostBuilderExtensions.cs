using System;
using System.IO;
using System.Reflection;
using Concept.Service.AspNetCore.Abstractions;
using Concept.Service.HostBuilder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Concept.Service.AspNetCore
{
  public static class ServiceHostBuilderExtensions
  {
    public static IServiceHostBuilder UseWebHost(this IServiceHostBuilder builder, IWebHostBuilder existingWebHost = null)
    {
      builder.ConfigureServices(collection =>
      {
        collection.AddSingleton(provider =>
        {
          var serviceMetadata = provider.GetService<ServiceMetadata>();
          if (!(provider.GetService<ServiceBootstrap>() is IAspNetCoreBootstrap aspNetBootstrap))
          {
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

