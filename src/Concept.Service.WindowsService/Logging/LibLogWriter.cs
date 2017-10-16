using System;
using Concept.Logging;
using Topshelf.Logging;

namespace Concept.Service.WindowsService.Logging
{
  public class LibLogWriter : LogWriter
  {
    private readonly ILog _logger;

    public LibLogWriter(string name)
    {
      _logger = LogProvider.GetLogger(name);
    }
    public void Log(LoggingLevel level, object obj)
    {
      var logLevel = MapLogLevel(level);
      if (logLevel == null) return;
      _logger.Log(logLevel.Value, () => FormatObject(obj));
    }

    public void Log(LoggingLevel level, object obj, Exception exception)
    {
      if (exception == null)
      {
        Log(level, obj);
        return;
      }

      var logLevel = MapLogLevel(level);
      if (logLevel == null)
      {
        return;
      }
      _logger.Log(logLevel.Value, () => FormatObject(obj), exception);
    }

    public void Log(LoggingLevel level, LogWriterOutputProvider messageProvider)
    {
      Log(level, messageProvider());
    }

    public void LogFormat(LoggingLevel level, IFormatProvider formatProvider, string format, params object[] args)
    {
      Log(level, string.Format(formatProvider, format, args));
    }

    public void LogFormat(LoggingLevel level, string format, params object[] args)
    {
      Log(level, string.Format(format, args));
    }

    public void Debug(object obj)
    {
      Log(LoggingLevel.Debug, obj);
    }

    public void Debug(object obj, Exception exception)
    {
      Log(LoggingLevel.Debug, obj, exception);
    }

    public void Debug(LogWriterOutputProvider messageProvider)
    {
      Log(LoggingLevel.Debug, messageProvider);
    }

    public void DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
    {
      LogFormat(LoggingLevel.Debug, formatProvider, format, args);
    }

    public void DebugFormat(string format, params object[] args)
    {
      LogFormat(LoggingLevel.Debug, format, args);
    }

    public void Info(object obj)
    {
      Log(LoggingLevel.Info, obj);
    }

    public void Info(object obj, Exception exception)
    {
      Log(LoggingLevel.Info, obj, exception);
    }

    public void Info(LogWriterOutputProvider messageProvider)
    {
      Log(LoggingLevel.Info, messageProvider);
    }

    public void InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
    {
      LogFormat(LoggingLevel.Info, formatProvider, format, args);
    }

    public void InfoFormat(string format, params object[] args)
    {
      LogFormat(LoggingLevel.Info, format, args);
    }

    public void Warn(object obj)
    {
      Log(LoggingLevel.Warn, obj);
    }

    public void Warn(object obj, Exception exception)
    {
      Log(LoggingLevel.Warn, obj, exception);
    }

    public void Warn(LogWriterOutputProvider messageProvider)
    {
      Log(LoggingLevel.Warn, messageProvider);
    }

    public void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
    {
      LogFormat(LoggingLevel.Warn, formatProvider, format, args);
    }

    public void WarnFormat(string format, params object[] args)
    {
      LogFormat(LoggingLevel.Warn, format, args);
    }

    public void Error(object obj)
    {
      Log(LoggingLevel.Error, obj);
    }

    public void Error(object obj, Exception exception)
    {
      Log(LoggingLevel.Error, obj, exception);
    }

    public void Error(LogWriterOutputProvider messageProvider)
    {
      Log(LoggingLevel.Error, messageProvider);
    }

    public void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
    {
      LogFormat(LoggingLevel.Error, formatProvider, format, args);
    }

    public void ErrorFormat(string format, params object[] args)
    {
      LogFormat(LoggingLevel.Error, format, args);
    }

    public void Fatal(object obj)
    {
      Log(LoggingLevel.Fatal, obj);
    }

    public void Fatal(object obj, Exception exception)
    {
      Log(LoggingLevel.Fatal, obj, exception);
    }

    public void Fatal(LogWriterOutputProvider messageProvider)
    {
      Log(LoggingLevel.Fatal, messageProvider);
    }

    public void FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
    {
      LogFormat(LoggingLevel.Fatal, formatProvider, format, args);
    }

    public void FatalFormat(string format, params object[] args)
    {
      LogFormat(LoggingLevel.Fatal, format, args);
    }

    public bool IsDebugEnabled => _logger.IsDebugEnabled();
    public bool IsInfoEnabled => _logger.IsInfoEnabled();
    public bool IsWarnEnabled => _logger.IsWarnEnabled();
    public bool IsErrorEnabled => _logger.IsErrorEnabled();
    public bool IsFatalEnabled => _logger.IsFatalEnabled();

    private static string FormatObject(object obj)
    {
      return obj?.ToString() ?? string.Empty;
    }

    private static LogLevel? MapLogLevel(LoggingLevel loglevel)
    {
      if (loglevel == LoggingLevel.Fatal)
        return LogLevel.Fatal;
      if (loglevel == LoggingLevel.Error)
        return LogLevel.Error;
      if (loglevel == LoggingLevel.Warn)
        return LogLevel.Warn;
      if (loglevel == LoggingLevel.Info)
        return LogLevel.Info;
      if (loglevel == LoggingLevel.Debug)
        return LogLevel.Debug;
      if (loglevel == LoggingLevel.All)
        return LogLevel.Trace;

      return null;
    }
  }
}