using System;
using Topshelf;

namespace Concept.Service.WindowsService
{
  public class TopshelfRunner
  {
    public static void Start<TService>(ServiceBootstrap<TService> bootstrap) where TService : Service
    {
      var metadata = bootstrap.CreateMetadata();
      
      bootstrap.PreConfigureLogger();
      bootstrap.ConfigureLogger();
      bootstrap.PostConfigureLogger();

      bootstrap.PreRegisterDependencies();
      bootstrap.RegisterDependencies();
      bootstrap.PostRegisterDependencies();

      HostFactory.Run(host =>
      {
        host.UseLibLog();
        host.SetDescription(metadata.Description);
        host.SetServiceName(metadata.Name);
        host.Service<Service>(service =>
        {
          service.WhenStarted(s => s.StartAsync().ConfigureAwait(false).GetAwaiter().GetResult());
          service.WhenStopped(s => s.StopAsync().ConfigureAwait(false).GetAwaiter().GetResult());
          service.ConstructUsing(() => bootstrap.GetServiceAsync().ConfigureAwait(false).GetAwaiter().GetResult());
        });
        host.StartAutomatically();
      });
    }
  }
}
