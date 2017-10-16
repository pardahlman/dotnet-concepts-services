using Topshelf.Logging;

namespace Concept.Service.WindowsService.Logging
{
  internal class LibLogConfigurer : HostLoggerConfigurator
  {
    public LogWriterFactory CreateLogWriterFactory()
    {
      return new LibLogWriterFactory();
    }
  }
}
