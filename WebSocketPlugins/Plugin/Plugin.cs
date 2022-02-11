using PluginCore.Basic;
using PluginCore.Plugin;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WebSocketPlugins.Basic;
using WebSocketPlugins.Stores;
using ApiCore.Basic;
using WebSocketPlugins.Model;
using Microsoft.EntityFrameworkCore;

namespace WebSocketPlugins.Plugin
{
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
            context.Services.AddScoped<IChatSessionService, ChatSessionService>();
            context.Services.AddScoped<IUserStores, UserStores>();

            return base.Init(context);
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
