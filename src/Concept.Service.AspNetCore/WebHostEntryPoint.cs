using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Concept.Logging;
using Concept.Service.HostBuilder.Abstractions;
using Microsoft.AspNetCore.Hosting;

namespace Concept.Service.AspNetCore
{
  public class WebHostEntryPoint : IServiceEntryPoint
  {
    private readonly IWebHost _webHost;
    private readonly ILog _logger = LogProvider.For<WebHostEntryPoint>();

    public WebHostEntryPoint(IWebHost webHost)
    {
      _webHost = webHost;
    }

    public async Task StartAsync(CancellationToken ct = default(CancellationToken))
    {
      var startTime = Stopwatch.GetTimestamp();
      try
      {
        await _webHost.StartAsync(ct);
        var timeElapsed = new TimeSpan(Stopwatch.GetTimestamp() - startTime);
        _logger.Debug("WebHost successfully started ater {elapsedTime}", timeElapsed);
      }
      catch (Exception e)
      {
        _logger.InfoException("An unhandled exception occured when starting the WebHost", e);
        var timeElapsed = new TimeSpan(Stopwatch.GetTimestamp() - startTime);
        _logger.Debug("WebHost failed to start after {elapsedTime}", timeElapsed);
      }
    }
  }
}
