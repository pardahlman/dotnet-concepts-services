using Concept.Service.WindowsService.Logging;
using Topshelf.HostConfigurators;
using Topshelf.Logging;

namespace Concept.Service.WindowsService
{
  public static class HostConfiguratorExtensions
  {
    public static void UseLibLog(this HostConfigurator configurator)
    {
      HostLogger.UseLogger(new LibLogConfigurer());
    }
  }
}
