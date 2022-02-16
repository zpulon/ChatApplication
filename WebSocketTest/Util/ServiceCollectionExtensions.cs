using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebSocketPlugins.Basic;
using WebSocketPlugins.Handlers;
using WebSocketPlugins.Manager;
using WebSocketPlugins.Model;
using WebSocketPlugins.SocketsManager;
using WebSocketPlugins.Stores;

namespace WebSocketTest.Util
{
    public static class ServiceCollectionExtensions
    {
        //复制对应项目 Plugin Init()
        public static void AddUserDefined(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddScoped<WebSocketDbContext>();
            services.AddTransient<ConnectionManager>();
            services.AddSingleton<WebSocketMessageHandler>();
            services.AddSingleton<SocketHandler>();
            services.AddScoped<IChatSessionService, ChatSessionService>();
            services.AddScoped<UserManager>();
            services.AddScoped<IUserStores, UserStores>();
        }
    }
}
