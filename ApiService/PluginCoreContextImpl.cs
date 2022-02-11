using ApiService.DefaultService;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PluginCore.Basic;
using PluginCore.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ApiService
{
    public static class PluginCoreConatextCollection
    {
        public static PluginCoreContextImpl AddPlugin(this IServiceCollection services,  Action<PluginCoreContextImpl> action)
        {
            var impl = new PluginCoreContextImpl(services, action);
            return impl;
        }
    }

    public class PluginCoreContextImpl : PluginCoreContext
    {
        public PluginCoreContextImpl(IServiceCollection serviceCollection, Action<PluginCoreContextImpl> action)
            : base(serviceCollection)
        {
            string pluginConfigPath = System.IO.Path.Combine(AppContext.BaseDirectory, "PluginConfig");
            if (!System.IO.Directory.Exists(pluginConfigPath))
            {
                System.IO.Directory.CreateDirectory(pluginConfigPath);
            }
            string pluginPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Plugin");
            if (!System.IO.Directory.Exists(pluginPath))
            {
                System.IO.Directory.CreateDirectory(pluginPath);
            }
            //所有程序集
            DirectoryLoader dl = new DirectoryLoader();
            List<Assembly> assList = new List<Assembly>();
            var psl = dl.LoadFromDirectory(pluginPath);
            assList.AddRange(psl);
            AdditionalAssembly = assList;

            if (action == null) throw new ArgumentNullException(nameof(action));
            action(this);

            Services = serviceCollection;
        }
        public async override Task<bool> Init()
        {
            try
            {

                await base.Init();
                string pluginPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Plugin");
                PluginConfigStorage = new DefaultPluginConfigStorage();
                PluginFactory = new DefaultPluginFactory();

                PluginFactory.Load(pluginPath);
                var result = await PluginFactory.Init(this);
                
                ServiceProvider = this.Services.BuildServiceProvider();
                return result;
            }
            catch (ReflectionTypeLoadException ex)
            {
                Console.WriteLine("初始化失败：\r\n{0}", ex.ToString());
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("初始化失败：\r\n{0}", ex.ToString());
                return false;
            }
        }

        public async override Task<bool> Start()
        {
            await PluginFactory.Start(this);
            return await base.Start();
        }

        public async override Task<bool> Stop()
        {
            await PluginFactory.Stop(this);
            return await base.Stop();
        }
    }
}
