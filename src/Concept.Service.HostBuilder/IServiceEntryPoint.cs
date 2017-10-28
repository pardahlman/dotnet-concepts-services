using System.Threading;
using System.Threading.Tasks;

namespace Concept.Service.HostBuilder
{
  public interface IServiceEntryPoint
  {
    Task StartAsync(CancellationToken ct = default(CancellationToken));
  }
}
