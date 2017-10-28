using Topshelf.Logging;

namespace Concept.Service.TopshelfHost.Logging
{
  internal class LibLogConfigurer : HostLoggerConfigurator
  {
    public LogWriterFactory CreateLogWriterFactory()
    {
      return new LibLogWriterFactory();
    }
  }
}
