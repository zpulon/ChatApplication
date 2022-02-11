using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace ApiCore.Basic
{
    public class EFLogger : ILogger
    {
        protected string categoryName;

        public EFLogger(string categoryName)
        {
            this.categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var sqlLog = formatter(state, exception);
            if (sqlLog.StartsWith("Executed"))
            {
                //通过Debugger.Log方法来将EF Core生成的Log信息输出到Visual Studio的输出窗口
                Debugger.Log(0, categoryName, "=============================== EF Core log started ===============================\r\n");
                Debugger.Log(0, categoryName, sqlLog + "\r\n");
                Debugger.Log(0, categoryName, "=============================== EF Core log finished ===============================\r\n");
            }
        }
    }

    public class EFLoggerFactory : ILoggerFactory
    {
        public void AddProvider(ILoggerProvider provider)
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new EFLogger(categoryName);//创建EFLogger类的实例
        }

        public void Dispose()
        {

        }
    }
}
