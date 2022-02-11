using NLog;
using System;
using System.Collections.Generic;

namespace LogCore.Log
{
    public static class LogLevelConverter
	{
		public static LogLevels ToLogLevels(LogLevel logLevel)
		{
			LogLevels result = LogLevels.Trace;
			if (logLevel == LogLevel.Trace)
			{
				result = LogLevels.Trace;
			}
			else if (logLevel == LogLevel.Debug)
			{
				result = LogLevels.Debug;
			}
			else if (logLevel == LogLevel.Info)
			{
				result = LogLevels.Info;
			}
			else if (logLevel == LogLevel.Warn)
			{
				result = LogLevels.Warn;
			}
			else if (logLevel == LogLevel.Error)
			{
				result = LogLevels.Error;
			}
			else if (logLevel == LogLevel.Fatal)
			{
				result = LogLevels.Fatal;
			}
			return result;
		}

		public static IList<LogLevel> GetLogLevels(LogLevels logLevel)
		{
			IList<LogLevel> list = new List<LogLevel>();
			Action<LogLevels, IList<LogLevel>, LogLevels, LogLevel> obj = delegate (LogLevels sourceLogLevel, IList<LogLevel> targetLogLevelList, LogLevels checkLogLevel, LogLevel targetLogLevel)
			{
				if ((sourceLogLevel & checkLogLevel) == checkLogLevel)
				{
					targetLogLevelList.Add(targetLogLevel);
				}
			};
			obj(logLevel, list, LogLevels.Trace, LogLevel.Trace);
			obj(logLevel, list, LogLevels.Debug, LogLevel.Debug);
			obj(logLevel, list, LogLevels.Info, LogLevel.Info);
			obj(logLevel, list, LogLevels.Warn, LogLevel.Warn);
			obj(logLevel, list, LogLevels.Error, LogLevel.Error);
			obj(logLevel, list, LogLevels.Fatal, LogLevel.Fatal);
			return list;
		}
	}

}
