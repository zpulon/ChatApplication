using PluginCore.Basic;
using System.Threading.Tasks;

namespace PluginCore.Interface
{
    public interface IPluginConfigStorage
    {

        /// <summary>
        /// 获取插件配置
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <param name="pluginId"></param>
        /// <returns></returns>
        Task<PluginMessage<TConfig>> GetConfig<TConfig>(string pluginId);

        /// <summary>
        /// 保存插件配置
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        Task<PluginMessage> SaveConfig<TConfig>(string pluginId, TConfig config);

        /// <summary>
        /// 删除插件配置
        /// </summary>
        /// <param name="pluginId"></param>
        /// <returns></returns>
        Task<PluginMessage> DeleteConfig(string pluginId);
    }
}
