using PluginCore.Basic;
using PluginCore.Plugin;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace PluginCore.Interface
{
    public interface IPluginFactory
    {
        List<Assembly> LoadedAssembly { get; }

        IPlugin GetPlugin(string pluginId);
        PluginItem GetPluginInfo(string pluginId, bool secret = false);
        List<PluginItem> GetPluginList(bool secret = false);
        void Load(string pluginPath);


        Task<bool> Init(PluginCoreContext context);

        Task<bool> Start(PluginCoreContext context);

        Task<bool> Stop(PluginCoreContext context);
    }
}
