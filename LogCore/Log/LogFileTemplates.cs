using System.IO;

namespace LogCore.Log
{
    public static class LogFileTemplates
	{
		private static string PathSeparator = new string(Path.DirectorySeparatorChar, 1);

		public static readonly string PerDay = "${shortdate}.log";

		public static readonly string PerDayAndLevel = "{shortdate}_${level}.log";

		public static readonly string PerDayDir = "${shortdate}" + PathSeparator + "log.log";

		public static readonly string PerDayDirAndLoggerAndLevel = "${shortdate}" + PathSeparator + "${logger}_${level}.log";

		public static readonly string PerDayDirAndLogger = "${shortdate}" + PathSeparator + "${logger}.log";

		public static readonly string LevelDirAndPerDay = "${level}" + PathSeparator + "${shortdate}.log";

		public static readonly string LevelDirAndLogger = "${level}" + PathSeparator + "${logger}.log";

		public static readonly string LevelAndDateDirAndLogger = "${level}" + PathSeparator + "${shortdate}" + PathSeparator + "${logger}.log";

		public static readonly string LevelAndLoggerDirAndDate = "${level}" + PathSeparator + "${logger}" + PathSeparator + "${shortdate}.log";

		public static readonly string LoggerDirAndDate = "${logger}" + PathSeparator + "${shortdate}.log";

		public static readonly string LoggerDirAndDateLevel = "${logger}" + PathSeparator + "${shortdate}_${level}.log";

		public static readonly string PerLogger = "${logger}.log";

		public static readonly string PerDayDirAndLoggerArchive = "${shortdate}" + PathSeparator + "Archive" + PathSeparator + "${logger}.{####}.zip";
	}
}
