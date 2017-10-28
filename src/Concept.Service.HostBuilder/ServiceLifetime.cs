using System.Threading;

namespace Concept.Service.HostBuilder
{
  public interface IServiceLifetime
  {
    CancellationToken ApplicationStarted { get; }

    CancellationToken ApplicationStopping { get; }

    CancellationToken ApplicationStopped { get; }

    void StopApplication();
  }

  public class ServiceLifetime : IServiceLifetime
  {
    private readonly CancellationTokenSource _applicationStartedSource;
    private readonly CancellationTokenSource _applicationStoppingSource;
    private readonly CancellationTokenSource _applicationStoppedSource;

    public CancellationToken ApplicationStarted => _applicationStartedSource.Token;
    public CancellationToken ApplicationStopping => _applicationStoppingSource.Token;
    public CancellationToken ApplicationStopped => _applicationStoppedSource.Token;

    public ServiceLifetime()
    {
      _applicationStartedSource = new CancellationTokenSource();
      _applicationStoppingSource = new CancellationTokenSource();
      _applicationStoppedSource = new CancellationTokenSource();
    }

    public void StopApplication()
    {
      _applicationStoppingSource.Cancel();
    }

    public void NotifyStarted()
    {
      _applicationStartedSource.Cancel();
    }

    public void NotifyStopping()
    {
      _applicationStoppingSource.Cancel();
    }

    public void NotifyStopped()
    {
      _applicationStoppingSource.Cancel();
    }
  }
}
