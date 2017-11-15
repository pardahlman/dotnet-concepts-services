using System;
using System.Diagnostics;
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
  }

  public abstract class ServiceBootstrap<TService> : ServiceBootstrap where TService : Service
  {
    private TService _service;

    public override ServiceMetadata CreateMetadata()
    {
      var serviceType = typeof(TService);
      var versionInfo = FileVersionInfo.GetVersionInfo(serviceType.Assembly.Location);
      return new ServiceMetadata
      {
         Name = serviceType.Name,
         Type = serviceType,
         Version = versionInfo.ProductVersion,
         Commit = versionInfo.FileVersion,
         Description = serviceType.Name
      };
    }

    public override async Task<Service> GetServiceAsync(CancellationToken ct = default(CancellationToken))
    {
      _service = _service ?? await CreateServiceAsync(ct);
      return _service;
    }

    protected abstract Task<TService> CreateServiceAsync(CancellationToken ct = default (CancellationToken));
  }
}
