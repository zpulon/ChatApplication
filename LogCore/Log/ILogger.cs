namespace LogCore.Log
{
    public interface ILogger
	{
		string LoggerName { get; set; }

		void Trace(string message);

		void Trace(string formatString, params object[] args);

		void Debug(string message);

		void Debug(string formatString, params object[] args);

		void Info(string message);

		void Info(string formatString, params object[] args);

		void Warn(string message);

		void Warn(string formatString, params object[] args);

		void Error(string message);

		void Error(string formatString, params object[] args);

		void Fatal(string message);

		void Fatal(string formatString, params object[] args);

		void Log(LogLevels logLevel, string message);

		void Log(LogLevels logLevel, string formatString, params object[] args);
	}

}
