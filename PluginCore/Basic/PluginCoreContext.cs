using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PluginCore.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace PluginCore.Basic
{
    public class PluginCoreContext
    {

        public List<Assembly> AdditionalAssembly { get; set; }

        public IServiceCollection Services { get; protected set; }

        public static PluginCoreContext Current { get; private set; }

        public IServiceProvider ServiceProvider { get; set; }

        public IApplicationBuilder ApplicationBuilder { get; set; }

        public IPluginConfigStorage PluginConfigStorage { get; set; }

        public IPluginFactory PluginFactory { get; set; }

        public PluginCoreConfig Config { get; protected set; }


        public string ConnectionString { get; set; }

        public string NWFUrl { get; set; }
        public string NWFExamineCallbackUrl { get; set; }
        public string ExamineCenterUrl { get; set; }

        public string AuthUrl { get; set; }
        public string AppGatewayUrl { get; set; }

        public string ClientID { get; set; }
        public string ClientSecret { get; set; }

        public string FileServerRoot { get; set; }

        public string MessageServerUrl { get; set; }



        public PluginCoreContext(IServiceCollection serviceContainer)
        {
            Current = this;
            Services = serviceContainer;

        }
        public PluginCoreContext(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            Current = this;
            ApplicationBuilder = app;
            ServiceProvider = serviceProvider;
        }



        public async virtual Task<bool> Init()
        {
            return true;
        }
        public async virtual Task<bool> InitApp()
        {
            return true;
        }
        public async virtual Task<bool> Start()
        {
            return true;
        }

        public async virtual Task<bool> Stop()
        {
            return true;
        }
    }
}
