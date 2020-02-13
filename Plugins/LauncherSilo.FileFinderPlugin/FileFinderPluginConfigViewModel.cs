using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Resources;
using System.Windows.Markup;

namespace LauncherSilo.FileFinderPlugin
{
    public class FileFinderPluginConfigViewModel : Core.ViewModels.PluginConfigViewModel
    {
        public FileFinderPluginConfigViewModel(PluginSystem.IPlugin Plugin, PluginSystem.PluginConfig PluginConfig) : base(Plugin, PluginConfig)
        {
            Uri uri = new Uri("pack://application:,,,/LauncherSilo.FileFinderPlugin;component/FileFinderPluginResource.xaml");
            StreamResourceInfo info = Application.GetResourceStream(uri);
            XamlReader reader = new XamlReader();
            var dictionary = reader.LoadAsync(info.Stream) as ResourceDictionary;
            PluginConfigControlTemplate = dictionary["FileFinderPluginConfigView"] as ControlTemplate;
        }
        public FileFinderPluginConfig FileFinderPluginConfig { get { return _PluginConfig as FileFinderPluginConfig; } set { _PluginConfig = value; } }

        public void ApplyConfigProperty()
        {
            OnPropertyChanged("IsEnableFileFinder");
        }
        public bool IsEnableFileFinder
        {
            get
            {
                if (FileFinderPluginConfig == null)
                {
                    return false;
                }
                return FileFinderPluginConfig.IsEnableFileFinder;
            }
            set
            {
                if (FileFinderPluginConfig != null)
                {
                    if (value != FileFinderPluginConfig.IsEnableFileFinder)
                    {
                        FileFinderPluginConfig.IsEnableFileFinder = value;
                        OnPropertyChanged("IsEnableFileFinder");
                    }
                }
            }
        }
    }
}
