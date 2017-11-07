using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Concept.Service.AspNetCore.Abstractions
{
  public interface IAspNetCoreBootstrap
  {
    void ConfigureHost(IWebHostBuilder webHost);
    void ConfigureLogging(ILoggingBuilder logger);
    void ConfigureServices(IServiceCollection collection);
    void ConfigureAppConfiguration(IConfigurationBuilder configuration);
    void CreateServiceFactory(Func<Service> serviceFactory);
    void Configure(IApplicationBuilder app, IHostingEnvironment env);
  }
}