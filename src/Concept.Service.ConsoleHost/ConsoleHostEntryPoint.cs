using System;
using System.Threading;
using System.Threading.Tasks;
using Concept.Service.HostBuilder;

namespace Concept.Service.ConsoleHost
{
  public class ConsoleHostEntryPoint : IServiceEntryPoint
  {
    private readonly ServiceBootstrap _bootstrap;

    public ConsoleHostEntryPoint(ServiceBootstrap bootstrap)
    {
      _bootstrap = bootstrap;
    }

    public Task StartAsync(CancellationToken ct = default(CancellationToken))
    {
      try
      {
        return _bootstrap.StartServiceAsync(ct);
      }
      catch (Exception)
      {
        _bootstrap.ConfigureLogger();
        _bootstrap.RegisterDependencies();
        return _bootstrap.StartServiceAsync(ct);
      }
    }
  }
}
