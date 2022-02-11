using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using Microsoft.Extensions.Logging;
using ApiCore.Basic;
using ApiCore.JsonFilter;

namespace ApiCore.Utils {
    /// <summary>
    /// 异常处理
    /// </summary>
    public class ExceptionHandlerMiddleWare {
        private readonly ILogger _logger;
        private readonly RequestDelegate next;
        private readonly IJsonHelper ijsonHelper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public ExceptionHandlerMiddleWare(RequestDelegate next, ILoggerFactory logger, IJsonHelper jsonHelper) {
            this.next = next;
            _logger = logger?.CreateLogger("exceptionlog");
            ijsonHelper = jsonHelper;
        }
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context) {
            try {
                await next(context);
            } catch (Exception ex) {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception) {
            if (exception == null) return;
            await WriteExceptionAsync(context, exception).ConfigureAwait(false);
        }
        /// <summary>
        /// 发生异常重新组织内容
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        private async Task WriteExceptionAsync(HttpContext context, Exception exception) {

            WriteException(exception);
            //返回友好的提示
            var response = new ResponseMessage();

            if (exception is CustomException) {
                var ex = exception as CustomException;
                response.Code = ex.Code.ToString();
                response.Message = ex.ErrorMsg;
            } else if (exception is DbException) {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = "系统错误";
            } else if (exception is Exception) {
                response.Code = ResponseCodeDefines.ServiceError;
                response.Message = exception.Message;
            }

            var body = ijsonHelper.ToJson(response);
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";
            context.Response.ContentLength = Encoding.Default.GetByteCount(body);
            await context.Response.WriteAsync(body);
        }
        private void WriteException(Exception exception) {
            // 自定义日志不输出
            if (exception is CustomException) {
                return;
            }
            var message = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]错误:{exception.ToString()}";
            // 如果日志对象为空 则只输出到控制台
            if (_logger == null) {
                Console.WriteLine(message);
            } else {
                // 记录异常日志到文件中
                _logger.LogError(message);
            }
            
        }
    }
}
