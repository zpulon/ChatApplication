using PluginCore.Basic;
using System;
using System.Threading.Tasks;

namespace PluginCore.Plugin
{
    public interface IPluginConfig<TConfig>
    {
        Type ConfigType { get; }

        Task<TConfig> GetConfig(PluginCoreContext context);

        Task<bool> SaveConfig(TConfig cfg);

        TConfig GetDefaultConfig(PluginCoreContext context);

        Task<PluginMessage> ConfigChanged(PluginCoreContext context, TConfig newConfig);


    }
}
