using Topshelf.HostConfigurators;
using Topshelf.Logging;

namespace Concept.Service.TopshelfHost.Logging
{
  public static class HostConfiguratorExtensions
  {
    public static void UseLibLog(this HostConfigurator configurator)
    {
      HostLogger.UseLogger(new LibLogConfigurer());
    }
  }
}
