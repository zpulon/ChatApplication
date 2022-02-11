using LogCore.Log;
using Microsoft.Extensions.Logging;
using System;

namespace LogCore.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class LoggerProvider : ILoggerProvider, IDisposable
	{
		public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
		{
			return new LoggerAdapter(categoryName);
		}

		public void Dispose()
		{
		}
	}
	internal class LoggerAdapter : Microsoft.Extensions.Logging.ILogger
	{
		private Log.ILogger _logger;

		public LoggerAdapter(string name)
		{
			_logger = LoggerManager.GetLogger(name ?? "global");
		}

		public IDisposable BeginScope<TState>(TState state)
		{
			return null;
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return true;
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			_logger.Log(logLevel switch
			{
				LogLevel.Trace => LogLevels.Trace,
				LogLevel.Critical => LogLevels.Fatal,
				LogLevel.Debug => LogLevels.Debug,
				LogLevel.Error => LogLevels.Error,
				LogLevel.Information => LogLevels.Info,
				LogLevel.Warning => LogLevels.Warn,
				_ => LogLevels.Trace,
			}, "{0} {1}\r\n{2}", eventId.Id, eventId.Name ?? "", formatter(state, exception));
		}
	}
}
