using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Concept.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Concept.Service.HostBuilder
{
  public interface IServiceHost
  {
    IServiceProvider Services { get; set; }
    Task StartAsync(CancellationToken ct = default(CancellationToken));
    Task StopAsync(CancellationToken ct = default(CancellationToken));
  }

  public static class ServiceHostExtensions
  {
    public static async Task RunAsync(this IServiceHost host, CancellationToken ct = default(CancellationToken))
    {
      await host.StartAsync(ct);

      var cancelCompletionSource = new TaskCompletionSource<ConsoleCancelEventArgs>();
      Console.CancelKeyPress += (sender, args) =>
      {
        cancelCompletionSource.TrySetResult(args);
      };
      await cancelCompletionSource.Task;
    }
  }

  public class ServiceHost : IServiceHost, IDisposable
  {
    private ServiceLifetime _lifetime;
    public IServiceProvider Services { get; set; }
    private readonly ILog _logger = LogProvider.For<ServiceHost>();

    public async Task StartAsync(CancellationToken ct = default(CancellationToken))
    {
      _logger.DebugFormat("Preparing to start services in the service host.");
      _lifetime = new ServiceLifetime();
      var entryPoints = Services.GetServices<IServiceEntryPoint>();
      var entryTasks = entryPoints
        .Select(e =>
        {
          var entryPointName = e.GetType().Name;
          _logger.Debug("Invoking entry point {entryPoint}.", entryPointName);
          return e.StartAsync(ct).ContinueWith(t =>
          {
            if (t.IsFaulted)
              _logger.InfoException("An unhandled exception occured when invoking entry point {entryPoint}", t.Exception, entryPointName);
            else
              _logger.Debug("Entry point {entryPoint} successfully invoked.", entryPointName);
          }, ct);
        })
        .ToList();
      await Task.WhenAll(entryTasks);
      _lifetime.NotifyStarted();
    }

    public Task StopAsync(CancellationToken ct = default(CancellationToken))
    {
      _lifetime.NotifyStopping();
      Dispose();
      return Task.CompletedTask;
    }

    public void Dispose()
    {
      (Services as IDisposable)?.Dispose();
    }
  }
}
