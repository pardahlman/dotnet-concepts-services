using System;
using System.Threading;
using System.Threading.Tasks;
using Concept.Service.HostBuilder.Abstractions;

namespace Concept.Service.ConsoleHost
{
  public class ConsoleHostEntryPoint : IServiceEntryPoint
  {
    private readonly IServiceBootstrap _bootstrap;

    public ConsoleHostEntryPoint(IServiceBootstrap bootstrap)
    {
      _bootstrap = bootstrap;
    }

    public async Task StartAsync(CancellationToken ct = default(CancellationToken))
    {
      try
      {
        await _bootstrap.StartServiceAsync(ct);
      }
      catch (Exception)
      {
        _bootstrap.RegisterDependencies();
        await _bootstrap.StartServiceAsync(ct);
      }
    }
  }
}
