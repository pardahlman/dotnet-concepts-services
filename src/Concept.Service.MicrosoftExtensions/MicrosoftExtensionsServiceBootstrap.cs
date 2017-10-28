using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Concept.Service.MicrosoftExtensions
{
  public abstract class MicrosoftExtensionsServiceBootstrap<TService> : ServiceBootstrap<TService>, IDisposable where TService : Service
  {
    public IServiceCollection ServiceCollection { get; private set; }
    protected IServiceProvider ServiceProvider;

    public override void ConfigureLogger()
    {
      ServiceCollection = CreateServiceCollection();
      ServiceCollection.AddLogging(ConfigureLogging);
    }

    public abstract void ConfigureLogging(ILoggingBuilder builder);

    public abstract void ConfigureServices(IServiceCollection services);

    protected virtual IServiceCollection CreateServiceCollection()
    {
      return new ServiceCollection();
    }

    protected virtual IServiceProvider BuildServiceProvider(IServiceCollection collection)
    {
      return collection.BuildServiceProvider();
    }

    public override void RegisterDependencies()
    {
      ConfigureServices(ServiceCollection);
      ServiceProvider = BuildServiceProvider(ServiceCollection);
    }

    protected override Task<TService> CreateServiceAsync()
    {
      return Task.FromResult(ServiceProvider.GetService<TService>());
    }

    public void Dispose()
    {
      (ServiceProvider as IDisposable)?.Dispose();
      (ServiceCollection as IDisposable)?.Dispose();
    }
  }
}
