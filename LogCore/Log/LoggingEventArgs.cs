using System;

namespace LogCore.Log
{
    public class LoggingEventArgs : EventArgs
	{
		public LogEntity LogEntity { get; set; }

		public LoggingEventArgs(LogEntity logEntity)
		{
			LogEntity = logEntity;
		}
	}
}
