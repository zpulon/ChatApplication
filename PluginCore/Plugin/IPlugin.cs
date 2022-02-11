using PluginCore.Basic;
using System.Threading.Tasks;

namespace PluginCore.Plugin
{
    public interface IPlugin
    {
        string PluginID { get; }

        string PluginName { get; }

        string Description { get; }

        int Order { get; }


        Task<PluginMessage> Init(PluginCoreContext context);

        Task<PluginMessage> Start(PluginCoreContext context);

        Task<PluginMessage> Stop(PluginCoreContext context);

        Task OnMainConfigChanged(PluginCoreContext context, PluginCoreConfig newConfig);
    }
}
