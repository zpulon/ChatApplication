using ApiCore.Basic;
using ApiCore.Utils;
using CSRedis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PluginCore.Basic;
using PluginCore.Plugin;
using System;
using System.Reflection;
using WebSocketPlugins.Model;
using WebSocketTest.Util;
using Xunit;

namespace WebSocketTest
{
    public class TestBase<TContext> where TContext : DbContext
    {
        public readonly IServiceProvider ServiceProvider;
        public readonly TContext Context;
        public TestBase()
        {
            var services = new ServiceCollection();
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            var config = builder.Build();
            services.AddSingleton<IConfigurationRoot>(config);

            services.AddMvc()
                    .AddViewComponentsAsServices();
            services.AddDbContextPool<TContext>(options =>
            {
                options.UseSqlServer(config["ConnectionStrings:DefaultConnection"]);
#if DEBUG
                options.UseLoggerFactory(new EFLoggerFactory());
#endif
            });
            services.AddUserDefined(config);
            services.AddUserDefined();
            string redisConnectionString = $@"{config["Redis:Connection"]},password={config["Redis:Password"]},defaultDatabase ={config["Redis:DefaultDB"]} ";
            //初始化 RedisHelper
            RedisHelper.Initialization(new CSRedisClient(redisConnectionString));
            services.AddSingleton<IDistributedCache>(new Microsoft.Extensions.Caching.Redis.CSRedisCache(RedisHelper.Instance));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            ServiceProvider = services.BuildServiceProvider();
            Context = ServiceProvider.GetRequiredService<TContext>();
            var application = new PluginCoreContext(services)
            {
                ServiceProvider = ServiceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider,
                PluginFactory = new DefaultPluginFactory(),
                ConnectionString = config["ConnectionStrings:DefaultConnection"],
                AuthUrl = config["AuthUrl"],

            };
            ICoreServiceCollectionExtensions.ServiceProvider = ServiceProvider;
        }
        public void Dispose()
        {
        }
    }
    /// <summary>
    /// 定义Collection名称，标明使用的Fixture
    /// </summary>
    [CollectionDefinition("UnitTestCollection")]
    public class TestBaseCollection : ICollectionFixture<TestBase<WebSocketDbContext>>
    {

    }
}
