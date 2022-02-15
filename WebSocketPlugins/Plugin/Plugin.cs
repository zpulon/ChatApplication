using PluginCore.Basic;
using PluginCore.Plugin;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WebSocketPlugins.Basic;
using WebSocketPlugins.Stores;
using ApiCore.Basic;
using WebSocketPlugins.Model;
using Microsoft.EntityFrameworkCore;
using WebSocketPlugins.SocketsManager;
using WebSocketPlugins.Handlers;
using WebSocketPlugins.Manager;

namespace WebSocketPlugins.Plugin
{
    /// <summary>
    /// 
    /// </summary>
    public class Plugin : PluginBase
    {
        /// <summary>
        /// 
        /// </summary>
        public override string PluginID
        {
            get
            {
                return "WebSocketPlugins";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override string PluginName
        {
            get
            {
                return "WebSocket";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override string Description
        {
            get
            {
                return "WebSocket插件";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<PluginMessage> Init(PluginCoreContext context)
        {
            context.Services.AddDbContextPool<WebSocketDbContext>(options =>
            {
                options.UseSqlServer(context.ConnectionString);
#if DEBUG
                options.UseLoggerFactory(new EFLoggerFactory());
#endif
            });
            context.Services.AddTransient<ConnectionManager>();
            context.Services.AddSingleton<WebSocketMessageHandler>();
            context.Services.AddSingleton<SocketHandler>();
            context.Services.AddScoped<IChatSessionService, ChatSessionService>();
            context.Services.AddScoped<UserManager>(); 
            context.Services.AddScoped<IUserStores, UserStores>();

            return base.Init(context);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<PluginMessage> InitApp(PluginCoreContext context)
        {
            context.ApplicationBuilder.MapSocket("/websocket/chat", context.ServiceProvider.GetService<WebSocketMessageHandler>());
            return base.InitApp(context);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>

        public override Task<PluginMessage> Start(PluginCoreContext context)
        {

            return base.Start(context);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<PluginMessage> Stop(PluginCoreContext context)
        {
            return base.Stop(context);
        }
    }
}
