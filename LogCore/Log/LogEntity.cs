using System;

namespace LogCore.Log
{
    public class LogEntity
	{
		public string Logger { get; set; }

		public DateTime LogTime { get; set; }

		public string Message { get; set; }

		public string LogLevel { get; set; }
	}
}
