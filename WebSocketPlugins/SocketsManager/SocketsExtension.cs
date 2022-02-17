using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

namespace WebSocketPlugins.SocketsManager
{
    /// <summary>
    /// 
    /// </summary>
    public static class SocketsExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="path"></param>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static IApplicationBuilder MapSocket(this IApplicationBuilder app, PathString path, SocketHandler socket)
        {
            return app.Map(path, (x) => x.UseMiddleware<SocketsMiddleware>(socket));
        }


    }
}
