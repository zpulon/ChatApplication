namespace LogCore.Log
{
    public static class CommonLogger
	{
		private static ILogger traceLogger = LoggerManager.GetLogger("Trace");

		private static ILogger debugLogger = LoggerManager.GetLogger("Debug");

		private static ILogger uiLogger = LoggerManager.GetLogger("UI");

		private static ILogger eventLogger = LoggerManager.GetLogger("EventLogger");

		public static ILogger TraceLogger => traceLogger;

		public static ILogger DebugLogger => debugLogger;

		public static ILogger UILogger => uiLogger;

		public static ILogger EventLogger => eventLogger;
	}
}
