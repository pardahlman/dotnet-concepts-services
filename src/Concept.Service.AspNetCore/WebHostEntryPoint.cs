using System.Threading;
using System.Threading.Tasks;
using Concept.Service.HostBuilder;
using Microsoft.AspNetCore.Hosting;

namespace Concept.Service.AspNetCore
{
  public class WebHostEntryPoint : IServiceEntryPoint
  {
    private readonly IWebHost _webHost;

    public WebHostEntryPoint(IWebHost webHost)
    {
      _webHost = webHost;
    }

    public Task StartAsync(CancellationToken ct = default(CancellationToken))
    {
      return _webHost.StartAsync(ct);
    }
  }
}
