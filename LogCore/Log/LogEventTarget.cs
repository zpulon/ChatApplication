using NLog;
using NLog.Targets;
using System;

namespace LogCore.Log
{
    public class LogEventTarget : Target
	{
		public event EventHandler<LoggingEventArgs> Logging;

		protected override void Write(LogEventInfo logEvent)
		{
			LogEntity logEntity = new LogEntity
			{
				Logger = logEvent.LoggerName,
				LogLevel = logEvent.Level.Name,
				LogTime = logEvent.TimeStamp,
				Message = logEvent.FormattedMessage
			};
			if (this.Logging != null)
			{
				this.Logging(this, new LoggingEventArgs(logEntity));
			}
		}
	}
}
