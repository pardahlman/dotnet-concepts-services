using System;
using System.Diagnostics;
using Concept.Logging;

namespace Concept.Service.ConsoleHost
{
  public class ConsoleRunner
  {
    public static void Start<TService>(ServiceBootstrap<TService> bootstrap) where TService : Service
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

      logger.Debug("Creating instance of service {serviceName}", metadata.Name);
      var service = bootstrap.CreateService();
      var configureDuration = new TimeSpan(Stopwatch.GetTimestamp() - startTick);
      logger.Info("Service configured and instanciated in {configurationDuration:g} seconds.",configureDuration);

      logger.Info("Starting service {serviceName}. Press any key to stop", metadata.Name);
      service.StartAsync().GetAwaiter().GetResult();
      Console.ReadKey();
      service.StopAsync().GetAwaiter().GetResult();
      (bootstrap as IDisposable)?.Dispose();
    }
  }
}
