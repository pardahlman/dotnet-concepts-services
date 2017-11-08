using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Concept.Logging;
using Concept.Service.HostBuilder.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Concept.Service.HostBuilder
{

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
    private readonly ServiceLifetime _lifetime;
    public IServiceProvider Services { get; }
    private readonly ILog _logger = LogProvider.For<ServiceHost>();

    public ServiceHost(IServiceProvider serviceProvider)
    {
      Services = serviceProvider;
      _lifetime = serviceProvider.GetService<ServiceLifetime>();
    }

    public async Task StartAsync(CancellationToken ct = default(CancellationToken))
    {
      _logger.DebugFormat("Preparing to start services in the service host.");
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
      _lifetime.NotifyStopped();
      return Task.CompletedTask;
    }

    public void Dispose()
    {
      (Services as IDisposable)?.Dispose();
    }
  }
}
