using Topshelf.Logging;

namespace Concept.Service.TopshelfHost.Logging
{
  internal class LibLogWriterFactory : LogWriterFactory
  {
    public LogWriter Get(string name)
    {
      return new LibLogWriter(name);
    }

    public void Shutdown()
    {
    }
  }
}