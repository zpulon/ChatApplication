using PluginCore.Basic;
using System.Threading.Tasks;

namespace PluginCore.Plugin
{
    public abstract class PluginBase : IPlugin
    {
        public static readonly int DefaultOrder = 1000;

        public abstract string Description { get; }

        public virtual int Order
        {
            get
            {
                return DefaultOrder;
            }
        }
        public abstract string PluginID { get; }
        public abstract string PluginName { get; }

        public virtual Task<PluginMessage> Init(PluginCoreContext context)
        {
            return Task.FromResult(new PluginMessage());
        }
        public virtual Task<PluginMessage> Start(PluginCoreContext context)
        {
            return Task.FromResult(new PluginMessage());
        }
        public virtual Task<PluginMessage> Stop(PluginCoreContext context)
        {
            return Task.FromResult(new PluginMessage());
        }


        public virtual Task OnMainConfigChanged(PluginCoreContext context, PluginCoreConfig newConfig)
        {
            return Task.CompletedTask;
        }
    }
}
