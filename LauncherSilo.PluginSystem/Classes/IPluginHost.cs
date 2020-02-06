using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherSilo.PluginSystem
{
    public interface IPluginHost
    {
        event EventHandler<PluginStartupEventArgs> OnStartupEvent;
        event EventHandler<PluginShutdownEventArgs> OnShutdownEvent;
        event EventHandler<PluginConfigChengedEventArgs> OnConfigChenged;

        IPluginLogger Logger { get; }

        void RegisterPluginConfig<T>(string ConfigName) where T : PluginConfig;
        void UnregisterPluginConfig(string ConfigName);
        T GetConfigObject<T>(string ConfigName) where T : PluginConfig;


    }
}
