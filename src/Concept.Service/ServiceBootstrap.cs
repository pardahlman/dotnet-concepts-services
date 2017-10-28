using System;
using System.Threading;
using System.Threading.Tasks;

namespace Concept.Service
{
  public interface IServiceBootstrap
  {
    ServiceMetadata CreateMetadata();

    void PreConfigureLogger();
    void ConfigureLogger();
    void PostConfigureLogger();

    void PreRegisterDependencies();
    void RegisterDependencies();
    void PostRegisterDependencies();

    Task<Service> GetServiceAsync(CancellationToken ct = default(CancellationToken));

    Task StartServiceAsync(CancellationToken ct = default(CancellationToken));
    Task StopServiceAsync(CancellationToken ct = default(CancellationToken));
  }

  public abstract class ServiceBootstrap : IServiceBootstrap
  {
    public abstract ServiceMetadata CreateMetadata();

    public virtual void PreConfigureLogger() { }
    public abstract void ConfigureLogger();
    public virtual void PostConfigureLogger() { }

    public virtual void PreRegisterDependencies() { }
    public abstract void RegisterDependencies();
    public virtual void PostRegisterDependencies() { }

    public abstract Task<Service> GetServiceAsync(CancellationToken ct = default(CancellationToken));

    public abstract Task StartServiceAsync(CancellationToken ct = default(CancellationToken));
    public abstract Task StopServiceAsync(CancellationToken ct = default(CancellationToken));
  }

  public abstract class ServiceBootstrap<TService> : ServiceBootstrap where TService : Service
  {
    private TService _service;

    public override async Task StartServiceAsync(CancellationToken ct = default(CancellationToken))
    {
      if (_service != null)
      {
        throw new ApplicationException("Service is already started.");
      }
      _service = await GetServiceAsync(ct) as TService;
      await _service.StartAsync(ct);
    }

    public override Task StopServiceAsync(CancellationToken ct = default(CancellationToken))
    {
      if (_service == null)
      {
        throw new NullReferenceException("Is service started?");
      }
      return _service.StopAsync(ct);
    }

    public override async Task<Service> GetServiceAsync(CancellationToken ct = default(CancellationToken))
    {
      _service = _service ?? await CreateServiceAsync();
      return _service;
    }

    protected abstract Task<TService> CreateServiceAsync();
  }
}
