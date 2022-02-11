using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiService.DefaultService
{
    public static class WebApiApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseWebApi(this IApplicationBuilder app, params object[] args)
        {
            app = app.UseMiddleware(typeof(HttpRequestLogScopeMiddleware), args);
            return app;
        }
    }
}
