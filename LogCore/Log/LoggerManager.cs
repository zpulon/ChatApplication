using LogCore.Filters;
using NLog;
using NLog.Config;
using NLog.Filters;
using NLog.Layouts;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace LogCore.Log
{
	/// <summary>
	/// 
	/// </summary>
	public static class LoggerManager
	{
		private static readonly string tcpTargetName = "tcpTarget";

		private static readonly string fileTargetName = "fileTarget";

		private static readonly string eventTargetName = "eventTarget";

		private static LogConfig logConfig = null;

		private static LoggingRule fileTargetRule = null;

		public static readonly string ARCHIVEWRITEMODE_ROLLING = "Rolling";

		public static readonly string ARCHIVEWRITEMODE_SEQUENCE = "Sequence";

		private static Timer clearTimer = null;

		private static object clearLocker = new object();

		public static bool IsLoggingEnabled => LogManager.IsLoggingEnabled();

		public static event EventHandler<LoggingEventArgs> Logging;

		public static void InitLogger(LogConfig config)
		{
			LogManager.Configuration = new LoggingConfiguration();
			logConfig = config;
			Target target = null;
			FileTarget fileTarget = getFileTarget(config);
			LogManager.Configuration.AllTargets.ToList().All(delegate (Target x)
			{
				LogManager.Configuration.RemoveTarget(x.Name);
				return true;
			});
			if (config.IsAsync)
			{
				AsyncTargetWrapper asyncTargetWrapper = new AsyncTargetWrapper(fileTarget);
				LogManager.Configuration.AddTarget(fileTargetName, asyncTargetWrapper);
				target = asyncTargetWrapper;
			}
			else
			{
				LogManager.Configuration.AddTarget(fileTargetName, fileTarget);
				target = fileTarget;
			}
			LogManager.Configuration.LoggingRules.Clear();
			LoggingRule rule = new LoggingRule("*", target);
			LogLevelConverter.GetLogLevels(config.LogLevels).All(delegate (LogLevel level)
			{
				rule.EnableLoggingForLevel(level);
				return true;
			});
			rule.Targets.Add(new LogEventTarget());
			LogManager.Configuration.LoggingRules.Add(rule);
			fileTargetRule = rule;
			LogManager.ReconfigExistingLoggers();
		}

		private static FileTarget getFileTarget(LogConfig config, Layout layout = null)
		{
			string text = config.LogBaseDir;
			string text2 = new string(Path.DirectorySeparatorChar, 1);
			if (string.IsNullOrEmpty(config.MaxFileSize))
			{
				config.MaxFileSize = "10MB";
			}
			if (!text.EndsWith(text2))
			{
				text += text2;
			}
			string text3 = config.ArchiveBaseDir;
			if (string.IsNullOrEmpty(text3))
			{
				text3 = config.LogBaseDir;
			}
			if (!text3.EndsWith(text2))
			{
				text3 += text2;
			}
			string text4 = config.ArchiveFileTemplate;
			if (string.IsNullOrEmpty(text4))
			{
				text4 = ((!(config.LogFileTemplate == LogFileTemplates.PerDayDirAndLogger)) ? config.LogFileTemplate.Replace(".log", ".zip") : LogFileTemplates.PerDayDirAndLoggerArchive);
			}
			if (!text4.Contains("#"))
			{
				int num = text4.LastIndexOf(".");
				string text5 = text4.Substring(0, num);
				string text6 = text4.Substring(num);
				text4 = text5 + ".{####}" + text6;
			}
			text3 = Path.Combine(text3, text4);
			text = Path.Combine(text, config.LogFileTemplate);
			FileTarget fileTarget = new FileTarget();
			fileTarget.FileName = text;
			if (layout != null)
			{
				fileTarget.Layout = layout;
			}
			else
			{
				fileTarget.Layout = config.LogContentTemplate;
			}
			fileTarget.Encoding = Encoding.UTF8;
			fileTarget.ArchiveFileName = text3;
			fileTarget.ArchiveAboveSize = ParseSize(config.MaxFileSize);
			fileTarget.MaxArchiveFiles = config.MaxArchiveFiles;
			if (fileTarget.MaxArchiveFiles <= 0)
			{
				fileTarget.MaxArchiveFiles = int.MaxValue;
			}
			if ((config.ArchiveWriteMode ?? "").Equals("Rolling", StringComparison.OrdinalIgnoreCase))
			{
				fileTarget.ArchiveNumbering = ArchiveNumberingMode.Rolling;
			}
			else
			{
				fileTarget.ArchiveNumbering = ArchiveNumberingMode.Sequence;
			}
			fileTarget.EnableArchiveFileCompression = config.ZipArchiveFile ?? false;
			FileArchivePeriod fileArchivePeriod = FileArchivePeriod.None;
			try
			{
				fileArchivePeriod = (fileTarget.ArchiveEvery = (FileArchivePeriod)Enum.Parse(typeof(FileArchivePeriod), config.ArchiveEvery ?? "", ignoreCase: false));
				return fileTarget;
			}
			catch
			{
				return fileTarget;
			}
		}

		private static long ParseSize(string size)
		{
			if (string.IsNullOrEmpty(size))
			{
				return -1L;
			}
			long result = 0L;
			string text = size;
			double num = 1.0;
			if (size.EndsWith("tb", StringComparison.OrdinalIgnoreCase))
			{
				text = size.Substring(0, size.Length - 2);
				num = Math.Pow(1024.0, 4.0);
			}
			else if (size.EndsWith("gb", StringComparison.OrdinalIgnoreCase))
			{
				text = size.Substring(0, size.Length - 2);
				num = Math.Pow(1024.0, 3.0);
			}
			else if (size.EndsWith("mb", StringComparison.OrdinalIgnoreCase))
			{
				text = size.Substring(0, size.Length - 2);
				num = Math.Pow(1024.0, 2.0);
			}
			else if (size.EndsWith("kb", StringComparison.OrdinalIgnoreCase))
			{
				text = size.Substring(0, size.Length - 2);
				num = Math.Pow(1024.0, 1.0);
			}
			else if (size.EndsWith("b", StringComparison.OrdinalIgnoreCase))
			{
				text = size.Substring(0, size.Length - 1);
				num = 1.0;
			}
			else
			{
				text = size;
				num = 1.0;
			}
			if (long.TryParse(text, out result))
			{
				result = (long)((double)result * num);
			}
			return result;
		}

		public static void SetLoggerLevel(LogLevels logLevels)
		{
			if (fileTargetRule != null)
			{
				fileTargetRule.Levels.ToList().All(delegate (LogLevel x)
				{
					fileTargetRule.DisableLoggingForLevel(x);
					return true;
				});
				LogLevelConverter.GetLogLevels(logLevels).All(delegate (LogLevel x)
				{
					fileTargetRule.EnableLoggingForLevel(x);
					return true;
				});
				LogManager.ReconfigExistingLoggers();
			}
		}

		public static void SetLoggerAboveLevels(LogLevels logLevel)
		{
			LogLevels[] array = new LogLevels[6]
			{
			LogLevels.Trace,
			LogLevels.Debug,
			LogLevels.Info,
			LogLevels.Warn,
			LogLevels.Error,
			LogLevels.Fatal
			};
			LogLevels logLevels = LogLevels.All;
			for (int i = 0; i <= array.Length && array[i] != logLevel; i++)
			{
				logLevels &= ~array[i];
			}
			SetLoggerLevel(logLevels);
		}

		public static void DisableLogger(string loggerName, LogLevels logLevel)
		{
			if (fileTargetRule != null)
			{
				if (!(fileTargetRule.Filters.FirstOrDefault((Filter filter) => (filter is ExceptLoggerNameFilter && (filter as ExceptLoggerNameFilter).LoggerName == loggerName) ? true : false) is ExceptLoggerNameFilter exceptLoggerNameFilter))
				{
					ExceptLoggerNameFilter item = new ExceptLoggerNameFilter(loggerName, logLevel);
					fileTargetRule.Filters.Add(item);
				}
				else
				{
					exceptLoggerNameFilter.LogLevels |= logLevel;
				}
			}
			LogManager.ReconfigExistingLoggers();
		}

		public static void EnableLogger(string loggerName, LogLevels logLevel)
		{
			if (fileTargetRule != null && fileTargetRule.Filters.FirstOrDefault((Filter filter) => (filter is ExceptLoggerNameFilter && (filter as ExceptLoggerNameFilter).LoggerName == loggerName) ? true : false) is ExceptLoggerNameFilter exceptLoggerNameFilter)
			{
				if (exceptLoggerNameFilter.LogLevels == logLevel)
				{
					fileTargetRule.Filters.Remove(exceptLoggerNameFilter);
				}
				else
				{
					exceptLoggerNameFilter.LogLevels &= ~logLevel;
				}
			}
		}

		public static void MapLogger(string loggerName, LogLevels logLevel, string logFileName, Layout layout = null)
		{
			if (LogManager.Configuration == null)
			{
				LogManager.Configuration = new LoggingConfiguration();
			}
			string name = loggerName + "_" + logFileName.GetHashCode() + "_MAPTARGET";
			FileTarget fileTarget = getFileTarget(logConfig, layout);
			Target target = null;
			if (!string.IsNullOrEmpty(logFileName))
			{
				fileTarget.FileName = logFileName;
			}
			if (layout != null)
			{
				fileTarget.Layout = layout;
			}
			else
			{
				fileTarget.Layout = logConfig.LogContentTemplate;
			}
			if (logConfig.IsAsync)
			{
				AsyncTargetWrapper asyncTargetWrapper = new AsyncTargetWrapper(fileTarget);
				LogManager.Configuration.AddTarget(name, asyncTargetWrapper);
				target = asyncTargetWrapper;
			}
			else
			{
				LogManager.Configuration.AddTarget(name, fileTarget);
				target = fileTarget;
			}
			LoggingRule rule = new LoggingRule(loggerName, target);
			LogLevelConverter.GetLogLevels(logLevel).All(delegate (LogLevel level)
			{
				rule.EnableLoggingForLevel(level);
				return true;
			});
			if (fileTargetRule != null)
			{
				fileTargetRule.Filters.Add(new ExceptLoggerNameFilter(loggerName, LogLevels.All));
			}
			LogManager.Configuration.LoggingRules.Add(rule);
			LogManager.ReconfigExistingLoggers();
		}

		public static void EnableLogEvent(string loggerName, LogLevels logLevel)
		{
			if (LogManager.Configuration == null)
			{
				LogManager.Configuration = new LoggingConfiguration();
			}
			Target target = LogManager.Configuration.AllTargets.FirstOrDefault((Target x) => x.Name == eventTargetName);
			if (target == null)
			{
				LogEventTarget logEventTarget = new LogEventTarget();
				target = new AsyncTargetWrapper(logEventTarget)
				{
					Name = eventTargetName
				};
				logEventTarget.Logging += delegate (object sender, LoggingEventArgs args)
				{
					if (LoggerManager.Logging != null)
					{
						LoggerManager.Logging(sender, args);
					}
				};
				LogManager.Configuration.AddTarget(eventTargetName, target);
			}
			LoggingRule rule = LogManager.Configuration.LoggingRules.FirstOrDefault((LoggingRule x) => x.Targets != null && x.Targets.Any((Target t) => t.Name == eventTargetName));
			IList<LogLevel> logLevels = LogLevelConverter.GetLogLevels(logLevel);
			if (rule == null)
			{
				rule = new LoggingRule(loggerName, target);
				LogManager.Configuration.LoggingRules.Add(rule);
			}
			logLevels.All(delegate (LogLevel level)
			{
				rule.EnableLoggingForLevel(level);
				return true;
			});
			LogManager.ReconfigExistingLoggers();
		}

		public static void DisableLogEvent(string loggerName, LogLevels logLevel)
		{
			LogLevel.FromString(logLevel.ToString());
			LoggingRule rule = LogManager.Configuration.LoggingRules.FirstOrDefault((LoggingRule x) => x.Targets != null && x.Targets.Any((Target t) => t.Name == eventTargetName));
			if (rule != null)
			{
				LogLevelConverter.GetLogLevels(logLevel).All(delegate (LogLevel level)
				{
					rule.DisableLoggingForLevel(level);
					return true;
				});
				LogManager.ReconfigExistingLoggers();
			}
		}

		public static void EnableLogging()
		{
			LogManager.EnableLogging();
		}

		public static void DisableLogging()
		{
			LogManager.DisableLogging();
		}

		public static ILogger GetLogger(string loggerName)
		{
			return new DefaultLogger(LogManager.GetLogger(loggerName));
		}

		public static void DeleteLogs(int days, string logFolder, ILogger clearLogger)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(logFolder);
			if (!directoryInfo.Exists)
			{
				return;
			}
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			if (directories == null || directories.Length == 0)
			{
				return;
			}
			DateTime dateTime = DateTime.Now.AddDays(-days).Subtract(DateTime.Now.TimeOfDay);
			DirectoryInfo[] array = directories;
			foreach (DirectoryInfo directoryInfo2 in array)
			{
				DateTime result = DateTime.MinValue;
				if (DateTime.TryParse(directoryInfo2.Name, out result) && (result - dateTime).TotalSeconds <= 0.0)
				{
					try
					{
						directoryInfo2.Delete(recursive: true);
						clearLogger?.Debug("清除过期日志：{0}", directoryInfo2.FullName);
					}
					catch (Exception ex)
					{
						clearLogger?.Error("清除过期日志时发生异常：{0} \r\n{1}", directoryInfo2.FullName, ex.ToString());
					}
				}
			}
		}

		public static void StartClear(int days, string logFolder, ILogger clearLogger)
		{
			lock (clearLocker)
			{
				if (clearTimer != null)
				{
					clearTimer.Dispose();
					clearTimer = null;
				}
				clearTimer = new Timer(delegate
				{
					DeleteLogs(days, logFolder, clearLogger);
				}, null, TimeSpan.FromSeconds(2.0), TimeSpan.FromHours(1.0));
			}
		}
	}
}
