using LogCore.Log;
using PluginCore;
using PluginCore.Basic;
using PluginCore.Interface;
using System;
using System.Threading.Tasks;

namespace ApiService.DefaultService
{
    public class DefaultPluginConfigStorage : IPluginConfigStorage
    {
        protected ILogger Logger = LoggerManager.GetLogger("PluginConfigStorage");
        public Task<PluginMessage> DeleteConfig(string pluginId)
        {
            PluginMessage r = new();
            string cfgFile = GetConfigPath(pluginId);
            if (System.IO.File.Exists(cfgFile))
            {
                try
                {
                    System.IO.File.Delete(cfgFile);
                }
                catch (Exception e)
                {
                    r.Code = "500";
                    r.Message = e.Message;
                    Logger.Error("delete plugin config fail:\r\n{0}", e.ToString());
                }
            }
            return Task.FromResult(r);
        }

        public Task<PluginMessage<TConfig>> GetConfig<TConfig>(string pluginId)
        {
            PluginMessage<TConfig> r = new();
            string cfgFile = GetConfigPath(pluginId);
            if (System.IO.File.Exists(cfgFile))
            {
                try
                {
                    string json = System.IO.File.ReadAllText(cfgFile);
                    TConfig cfg = (TConfig)PluginJsonHelper.ToObject(json, typeof(TConfig));
                    r.Extension = cfg;
                }
                catch (Exception e)
                {
                    r.Code = "500";
                    r.Message = e.Message;
                    Logger.Error("get plugin config fail:\r\n{0}", e.ToString());
                }
            }
            else
            {
                r.Code = "404";
            }
            return Task.FromResult(r);
        }

        public Task<PluginMessage> SaveConfig<TConfig>(string pluginId, TConfig config)
        {
            PluginMessage r = new();
            string cfgFile = GetConfigPath(pluginId);
            if (config == null)
                return Task.FromResult(r);
            try
            {
                string json = "";
                if (config != null)
                {
                    json = PluginJsonHelper.ToJson(config);
                }

                System.IO.File.WriteAllText(cfgFile, json);
            }
            catch (Exception e)
            {
                r.Code = "500";
                r.Message = e.Message;
                Logger.Error("save plugin config fail:\r\n{0}", e.ToString());
            }

            return Task.FromResult(r);
        }
        protected virtual string GetConfigPath(string pluginId)
        {
            string path = System.IO.Path.Combine(AppContext.BaseDirectory, "PluginConfig");
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            return System.IO.Path.Combine(path, (pluginId ?? "none") + ".json");
        }
    }
}
