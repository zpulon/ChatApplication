using LogCore.Log;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApiService.DefaultService
{
	public class HttpRequestLogScopeMiddleware : IMiddleware
	{
		private readonly ILogger logger = LoggerManager.GetLogger("HttpRequestLogScopeMiddleware");


		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			
			Stopwatch sw = new();
			sw.Start();
			string url = context.Request.Path + "?" + context.Request.QueryString.Value;
	     	try
	     	{
	     		context.Request.EnableBuffering();
	     		string text = await new StreamReader(context.Request.Body).ReadToEndAsync();
	     		logger.Log(LogLevels.Info, "请求：{0} {1} \n {2} \n {3} ", context.Request.Method, url, context.Request.Headers, text);
	     		context.Request.Body.Position = 0L;
	     	}
	     	catch (Exception exception)
	     	{
	     		context.Request.Body.Position = 0L;
	     		logger.Log(LogLevels.Error, "记录请求日志异常 {0}",exception.StackTrace);
	     	}
			Stream bodyStream = context.Response.Body;
            MemoryStream tempResponseBodyStream = new MemoryStream();
            context.Response.Body = tempResponseBodyStream;

			await next(context);
			
				try
				{
					_ = 2;
					try
					{
						string text2 = "";
						tempResponseBodyStream.Seek(0L, SeekOrigin.Begin);
						if (tempResponseBodyStream.Length > 0)
						{
							text2 = await new StreamReader(context.Response.Body).ReadToEndAsync();
						}
						logger.Log(LogLevels.Info, "请求应答：{0} {1} {2} \n {3} \n {4} ", context.Response?.StatusCode, context.Request.Method, url, context.Response.Headers, text2);
						tempResponseBodyStream.Seek(0L, SeekOrigin.Begin);
					}
					catch (Exception exception)
					{
						context.Request.Body.Position = 0L;
					    logger.Log(LogLevels.Error, "记录请求应答日志异常{ 0}", exception.StackTrace);
					}
				}
				finally
				{
					await tempResponseBodyStream.CopyToAsync(bodyStream);
				}
			
			sw.Stop();
			logger.Log(LogLevels.Info, $"请求耗时：{sw.ElapsedMilliseconds}ms {context.Response?.StatusCode} {context.Request.Method} {url}");
		}
	}
}
