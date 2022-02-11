using LogCore.Log;
using NLog;
using NLog.Filters;

namespace LogCore.Filters
{
    public class ExceptLoggerNameFilter : Filter
	{
		private readonly string loggerName = "";

		private LogLevels logLevels;

		public string LoggerName => loggerName;

		public LogLevels LogLevels
		{
			get
			{
				return logLevels;
			}
			set
			{
				logLevels = value;
			}
		}

		public ExceptLoggerNameFilter(string loggerName, LogLevels logLevels)
		{
			this.loggerName = loggerName;
			this.logLevels = logLevels;
		}

		protected override FilterResult Check(LogEventInfo logEvent)
		{
			if (string.IsNullOrEmpty(loggerName))
			{
				return FilterResult.Log;
			}
			LogLevels logLevels = LogLevelConverter.ToLogLevels(logEvent.Level);
			if ((this.logLevels & logLevels) != logLevels)
			{
				return FilterResult.Log;
			}
			if (loggerName == "*")
			{
				return FilterResult.Ignore;
			}
			if (loggerName.StartsWith("*") && loggerName.EndsWith("*"))
			{
				if (logEvent.LoggerName.Contains(loggerName.Substring(1, loggerName.Length - 2)))
				{
					return FilterResult.Ignore;
				}
			}
			else if (loggerName.StartsWith("*"))
			{
				if (logEvent.LoggerName.EndsWith(loggerName.Substring(1)))
				{
					return FilterResult.Ignore;
				}
			}
			else if (loggerName.EndsWith("*"))
			{
				if (logEvent.LoggerName.StartsWith(loggerName.Substring(0, loggerName.Length - 1)))
				{
					return FilterResult.Ignore;
				}
			}
			else if (logEvent.LoggerName == loggerName)
			{
				return FilterResult.Ignore;
			}
			return FilterResult.Log;
		}
	}
}
