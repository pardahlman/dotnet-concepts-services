using System.Threading;
using System.Threading.Tasks;
using Concept.Service.HostBuilder;
using Concept.Service.TopshelfHost.Logging;
using Topshelf;

namespace Concept.Service.TopshelfHost
{
  public class TopshelfHostEntryPoint : IServiceEntryPoint
  {
    private readonly ServiceBootstrap _bootstrap;
    private readonly ServiceMetadata _metadata;

    public TopshelfHostEntryPoint(ServiceBootstrap bootstrap, ServiceMetadata metadata)
    {
      _bootstrap = bootstrap;
      _metadata = metadata;
    }

    public Task StartAsync(CancellationToken ct = default(CancellationToken))
    {
      _bootstrap.PreRegisterDependencies();
      _bootstrap.RegisterDependencies();
      _bootstrap.PostRegisterDependencies();

      HostFactory.Run(host =>
      {
        host.UseLibLog();
        host.SetDescription(_metadata.Description);
        host.SetServiceName(_metadata.Name);
        
        host.Service<Service>(service =>
        {
          service.WhenStarted(s => s.StartAsync(ct).ConfigureAwait(false).GetAwaiter().GetResult());
          service.WhenStopped(s => s.StopAsync(ct).ConfigureAwait(false).GetAwaiter().GetResult());
          service.ConstructUsing(() => _bootstrap.GetServiceAsync(ct).ConfigureAwait(false).GetAwaiter().GetResult());
        });
        host.StartAutomatically();
      });

      return Task.CompletedTask;
    }
  }
}
