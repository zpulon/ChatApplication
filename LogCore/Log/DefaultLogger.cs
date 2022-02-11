using NLog;
using System;

namespace LogCore.Log
{
    public  class DefaultLogger:ILogger
    {
		private readonly Logger logger;

		public string LoggerName
		{
			get
			{
				return logger.Name;
			}
			set
			{
				throw new NotSupportedException("LoggerName");
			}
		}

		public DefaultLogger(Logger logger)
		{
			this.logger = logger;
		}

		public void Trace(string message)
		{
			logger.Trace(message);
		}

		public void Trace(string formatString, params object[] args)
		{
			Log(LogLevels.Trace, formatString, args);
		}

		public void Debug(string message)
		{
			logger.Debug(message);
		}

		public void Debug(string formatString, params object[] args)
		{
			Log(LogLevels.Debug, formatString, args);
		}

		public void Info(string message)
		{
			logger.Info(message);
		}

		public void Info(string formatString, params object[] args)
		{
			Log(LogLevels.Info, formatString, args);
		}

		public void Warn(string message)
		{
			logger.Warn(message);
		}

		public void Warn(string formatString, params object[] args)
		{
			Log(LogLevels.Warn, formatString, args);
		}

		public void Error(string message)
		{
			logger.Error(message);
		}

		public void Error(string formatString, params object[] args)
		{
			Log(LogLevels.Error, formatString, args);
		}

		public void Fatal(string message)
		{
			logger.Fatal(message);
		}

		public void Fatal(string formatString, params object[] args)
		{
			Log(LogLevels.Fatal, formatString, args);
		}

		public void Log(LogLevels logLevel, string message)
		{
			logger.Log(LogLevel.FromString(logLevel.ToString()), message);
		}

		public void Log(LogLevels logLevel, string formatString, params object[] args)
		{
			logger.Log(LogLevel.FromString(logLevel.ToString()), string.Format(formatString, args));
		}
	}
}
