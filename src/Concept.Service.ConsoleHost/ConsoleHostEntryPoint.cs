using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Concept.Logging;
using Concept.Service.HostBuilder.Abstractions;

namespace Concept.Service.ConsoleHost
{
  public class ConsoleHostEntryPoint : IServiceEntryPoint
  {
    private readonly IServiceBootstrap _bootstrap;
    private readonly ILog _logger = LogProvider.For<ConsoleHostEntryPoint>();

    public ConsoleHostEntryPoint(IServiceBootstrap bootstrap)
    {
      _bootstrap = bootstrap;
    }

    public async Task StartAsync(CancellationToken ct = default(CancellationToken))
    {
      try
      {
        var startTime = Stopwatch.GetTimestamp();
        _logger.Debug("Preparing to start service");
        var service = await _bootstrap.GetServiceAsync(ct);
        _logger.Debug("Service of type {serviceType} successfully resolved", service.GetType().Name);
        await service.StartAsync(ct);
        var elapsed = new TimeSpan(Stopwatch.GetTimestamp() - startTime);
        _logger.Info("Service {serviceName} started after {elapsedTime}", service.GetType().Name, elapsed);
      }
      catch (Exception e)
      {
        _logger.Error(e, "An unhandled exception was thrown when starting the service");
      }
    }
  }
}
