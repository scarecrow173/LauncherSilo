using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherSilo.PluginSystem
{
    public interface IPlugin
    {
        string Name { get; }
        string Description { get; }
        string Version { get; }
        string Copyright { get; }
        Uri IconPath { get; }
        bool IsEnable { get; set; }
        IPluginModule[] PluginModules { get; set; }
        PluginConfig Config { get; }

        bool Initialize(IPluginHost Host);
        bool Finalize();
        
        void Startup(object sender, PluginStartupEventArgs args);
        void Shutdown(object sender, PluginShutdownEventArgs args);
        void ConfigChenged(object sender, PluginConfigChengedEventArgs args);
        void OnEventReceived(string EventName, object Parameter);

        AssemblyMetadata<IPluginModule>[] CollectModuleMetadatas();
    }
}
