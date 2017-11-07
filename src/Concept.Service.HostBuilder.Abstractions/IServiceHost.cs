using System;
using System.Threading;
using System.Threading.Tasks;

namespace Concept.Service.HostBuilder.Abstractions
{
  public interface IServiceHost
  {
    IServiceProvider Services { get; set; }
    Task StartAsync(CancellationToken ct = default(CancellationToken));
    Task StopAsync(CancellationToken ct = default(CancellationToken));
  }
}
