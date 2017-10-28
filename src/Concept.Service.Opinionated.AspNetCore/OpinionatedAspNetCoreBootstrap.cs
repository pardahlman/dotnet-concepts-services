using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Concept.Service.AspNetCore.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Concept.Service.Opinionated.AspNetCore
{
  public abstract class OpinionatedAspNetCoreBootstrap<TService> : OpinionatedServiceBootstrap<TService>, IAspNetCoreBootstrap where TService : Service
  {
    private readonly TaskCompletionSource<Func<TService>> _factoryCompletion;

    protected OpinionatedAspNetCoreBootstrap()
    {
      _factoryCompletion = new TaskCompletionSource<Func<TService>>();
    }

    public void ConfigureHost(IWebHostBuilder webHost)
    {
      webHost
        .UseSerilog()
        .ConfigureServices(collection => collection.AddAutofac());
    }

    public void ConfigureLogging(ILoggingBuilder builder) { /* Logging configured through Serilog */ }

    public override void RegisterDependencies() { /* Dependencys registered through WebHost*/ }

    public abstract void ConfigureServices(IServiceCollection collection);

    public void ConfigureContainer(ContainerBuilder builder) { RegisterDependencies(builder); }

    public virtual void ConfigureAppConfiguration(IConfigurationBuilder configuration) { }

    public void CreateServiceFactory(Func<Service> serviceFactory)
    {
      _factoryCompletion.TrySetResult(() => serviceFactory() as TService);
    }

    public abstract void Configure(IApplicationBuilder app, IHostingEnvironment env);

    protected override Task<TService> CreateServiceAsync()
    {
      return _factoryCompletion.Task.ContinueWith(t => t.Result());
    }
  }
}
