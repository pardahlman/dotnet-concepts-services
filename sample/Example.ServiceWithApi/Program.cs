using System.Threading.Tasks;
using Concept.Service.AspNetCore;
using Concept.Service.ConsoleHost;
using Concept.Service.HostBuilder;
using Example.ServiceWithApi.Service;

namespace Example.ServiceWithApi
{
  public class Program
  {
    public static void Main(string[] args)
    {
      MainAsync(args)
        .ConfigureAwait(false)
        .GetAwaiter()
        .GetResult();
    }

    public static async Task MainAsync(string[] args)
    {
      var host = new ServiceHostBuilder(new OpinionatedFooBootstrap())
        .UseConsoleHost()
        .UseWebHost()
        .Build();

      await host.RunAsync();
    }
  }
}
