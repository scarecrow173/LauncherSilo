using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LauncherSilo.PluginSystem;

namespace LauncherSilo.Core
{
    public abstract class LauncherSiloPluginBase : IPlugin
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract string Version { get; }
        public abstract string Copyright { get; }
        public abstract Uri IconPath { get; }
        public abstract bool IsEnable { get; set; }
        public abstract IPluginModule[] PluginModules { get; set; }
        public abstract PluginConfig Config { get; }



        public abstract bool Initialize(IPluginHost Host);
        public abstract bool Finalize();
        public abstract void Startup(object sender, PluginStartupEventArgs args);
        public abstract void Shutdown(object sender, PluginShutdownEventArgs args);
        public abstract void ConfigChenged(object sender, PluginConfigChengedEventArgs args);

        public abstract void OnEventReceived(string EventName, object Parameter);

        public abstract AssemblyMetadata<IPluginModule>[] CollectModuleMetadatas();

        public abstract ViewModels.PluginConfigViewModel CreatePluginConfigViewModel();
    }
}
