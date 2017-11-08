using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Concept.Service.AspNetCore.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Concept.Service.AspNetCore
{
  public abstract class AspNetCoreBootstrap<TService> : ServiceBootstrap<TService>, IAspNetCoreBootstrap where TService : Service
  {
    private readonly TaskCompletionSource<Func<TService>> _factoryCompletion;

    protected AspNetCoreBootstrap()
    {
      _factoryCompletion = new TaskCompletionSource<Func<TService>>();
    }

    public virtual void ConfigureHost(IWebHostBuilder host) { }

    public virtual void ConfigureLogging(ILoggingBuilder builder) { }

    public abstract void ConfigureServices(IServiceCollection services);

    public virtual void ConfigureAppConfiguration(IConfigurationBuilder configuration) { }

    public abstract void Configure(IApplicationBuilder app, IHostingEnvironment env);

    public void CreateServiceFactory(Func<Service> serviceFactory)
    {
      _factoryCompletion.TrySetResult(() => serviceFactory() as TService);
    }

    protected override Task<TService> CreateServiceAsync(CancellationToken ct = default (CancellationToken))
    {
      return _factoryCompletion.Task.ContinueWith(t => t.Result(), ct);
    }

    public override void ConfigureLogger() { /* Logger is configured by the Webhost */}

    public override void RegisterDependencies() { /* Dependencies are registered by the Webhost */ }
  }
}
