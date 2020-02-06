using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using LauncherSilo.Core;
using LauncherSilo.PluginSystem;



namespace LauncherSilo.AudioControllerPlugin
{
    public class AudioControllerPlugin : LauncherSiloPluginBase
    {
        public override string Name { get { return "AudioController"; } }
        public override string Description { get { return "AudioControllerPlugin"; } }
        public override string Version { get { return "1.0.0.0"; } }
        public override string Copyright { get { return "© 2019 - 2020"; } }
        public override Uri IconPath { get { return new Uri("pack://application:,,,/LauncherSilo.AudioControllerPlugin;component/AudioControllerIcon.ico"); } }
        public override bool IsEnable { get { return _IsEnable; } set { if (value != _IsEnable) { _IsEnable = value; } } }
        private bool _IsEnable = true;
        public override IPluginModule[] PluginModules { get; set; }
        private IPluginHost _Host = null;

        public override PluginConfig Config { get { return _Config; } }
        private PluginConfig _Config = null;
        public AudioControllerPluginConfig PluginConfig { get { return _Config as AudioControllerPluginConfig; } }

        public override bool Initialize(IPluginHost Host)
        {
            if (Host == null)
            {
                return false;
            }
            _Host = Host;
            _Host?.RegisterPluginConfig<AudioControllerPluginConfig>("AudioControllerPluginConfig");

            return true;
        }
        public override bool Finalize()
        {
            _Host?.UnregisterPluginConfig("AudioControllerPluginConfig");
            return true;
        }
        public override void Startup(object sender, PluginStartupEventArgs args)
        { }
        public override void Shutdown(object sender, PluginShutdownEventArgs args)
        { }
        public override void ConfigChenged(object sender, PluginConfigChengedEventArgs args)
        {
            var NewConfig = args.Config as AudioControllerPluginConfig;
            if (NewConfig != null)
            {
                _Config = NewConfig;
            }
        }

        public override void OnEventReceived(string EventName, object Parameter)
        { }
        public override AssemblyMetadata<IPluginModule>[] CollectModuleMetadatas()
        {
            return AssemblyMetadata<IPluginModule>.GetMetadatas(Assembly.GetExecutingAssembly(), _Host.Logger);
        }
        public override Core.ViewModels.PluginConfigViewModel CreatePluginConfigViewModel()
        {
            return new AudioControllerPluginConfigViewModel(this, Config);
        }
    }
}
