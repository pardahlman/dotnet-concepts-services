using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Concept.Logging;

namespace Concept.Service
{
  public class ConsoleRunner
  {
    public static void Start<TService>(ServiceBootstrap<TService> bootstrap) where TService : Service
    {
      StartAsync(bootstrap)
        .ConfigureAwait(false)
        .GetAwaiter()
        .GetResult();
    }

    public static async Task StartAsync<TService>(ServiceBootstrap<TService> bootstrap, CancellationToken ct = default (CancellationToken)) where TService : Service
    {
      var startTick = Stopwatch.GetTimestamp();
      var metadata = bootstrap.CreateMetadata();

      bootstrap.PreConfigureLogger();
      bootstrap.ConfigureLogger();
      bootstrap.PostConfigureLogger();

      var logger = LogProvider.For<ConsoleRunner>();
      logger.Info("Logger configured for service {serviceName}.", metadata.Name);

      bootstrap.PreRegisterDependencies();
      bootstrap.RegisterDependencies();
      bootstrap.PostRegisterDependencies();
      logger.Info("Services registered.");

      
      var configureDuration = new TimeSpan(Stopwatch.GetTimestamp() - startTick);
      logger.Info("Service configured and instanciated in {configurationDuration:g} seconds.", configureDuration);

      await bootstrap.StartServiceAsync(ct);

      if (ct == default(CancellationToken))
      {
        logger.Info("Starting service {serviceName}. Press [Ctrl + C] to stop", metadata.Name);

        var cancelCompletionSource = new TaskCompletionSource<ConsoleCancelEventArgs>();
        Console.CancelKeyPress += (sender, args) =>
        {
          bootstrap.StopServiceAsync(ct).GetAwaiter().GetResult();
          (bootstrap as IDisposable)?.Dispose();
          cancelCompletionSource.TrySetResult(args);
        };
        await cancelCompletionSource.Task;
      }
      else
      {
        logger.Info("Starting service {serviceName}. Stop service by calling cancel on provided CancellationToken", metadata.Name);
        ct.Register(() =>
        {
          bootstrap.StopServiceAsync(ct).GetAwaiter().GetResult();
          (bootstrap as IDisposable)?.Dispose();
        });
      }
    }
  }
}
