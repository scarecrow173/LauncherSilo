using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;

using LauncherSilo.Core;
using LauncherSilo.PluginSystem;

namespace LauncherSilo.FileFinderPlugin
{
    public class FileFinderPlugin : LauncherSiloPluginBase
    {
        public override string Name { get { return "FileFinder"; } }
        public override string Description { get { return "FileFinderPlugin"; } }
        public override string Version { get { return "1.0.0.0"; } }
        public override string Copyright { get { return "© "; } }
        public override Uri IconPath { get { return new Uri("pack://application:,,,/LauncherSilo.FileFinderPlugin;component/file-find-outline.png"); } }
        public override bool IsEnable { get { return _IsEnable; } set { if (value != _IsEnable) { _IsEnable = value; } } }
        private bool _IsEnable = true;
        public override IPluginModule[] PluginModules { get; set; }
        private IPluginHost _Host = null;

        public override PluginConfig Config { get { return _Config; } }
        private PluginConfig _Config = null;
        public FileFinderPluginConfigViewModel PluginConfigVM { get; set; } = null;

        public event PropertyChangedEventHandler OnConfigPropertyChanged;

        public override bool Initialize(IPluginHost Host)
        {
            if (Host == null)
            {
                return false;
            }
            _Host = Host;
            _Host?.RegisterPluginConfig<FileFinderPluginConfig>("FileFinderPluginConfig");
            return true;
        }
        public override bool Finalize()
        {
            _Host?.UnregisterPluginConfig("FileFinderPluginConfig");
            return true;
        }
        public override void Startup(object sender, PluginStartupEventArgs args)
        { }
        public override void Shutdown(object sender, PluginShutdownEventArgs args)
        { }
        public override void ConfigChenged(object sender, PluginConfigChengedEventArgs args)
        {
            var NewConfig = args.Config as FileFinderPluginConfig;
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
            PluginConfigVM = new FileFinderPluginConfigViewModel(this, Config);
            PluginConfigVM.PropertyChanged += PluginConfigVM_PropertyChanged;
            PluginConfigVM.ApplyConfigProperty();
            return PluginConfigVM;
        }

        private void PluginConfigVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnConfigPropertyChanged?.Invoke(sender, e);
        }
    }
}
