using System.Threading;
using System.Threading.Tasks;

namespace Concept.Service.HostBuilder.Abstractions
{
  public interface IServiceEntryPoint
  {
    Task StartAsync(CancellationToken ct = default(CancellationToken));
  }
}
