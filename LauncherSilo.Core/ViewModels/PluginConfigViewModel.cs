using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Controls;

using LauncherSilo.Core;

namespace LauncherSilo.Core.ViewModels
{
    public class PluginConfigViewModel : INotifyPropertyChanged
    {

        public string Name
        {
            get
            {
                if (_Plugin == null)
                {
                    return string.Empty;
                }
                return _Plugin.Name;
            }
        }
        public string Description
        {
            get
            {
                if (_Plugin == null)
                {
                    return string.Empty;
                }
                return _Plugin.Description;
            }
        }
        public string Version
        {
            get
            {
                if (_Plugin == null)
                {
                    return string.Empty;
                }
                return _Plugin.Version;
            }
        }
        public string Copyright
        {
            get
            {
                if (_Plugin == null)
                {
                    return string.Empty;
                }
                return _Plugin.Copyright;
            }
        }
        public Uri IconPath
        {
            get
            {
                if (_Plugin == null)
                {
                    return new Uri("");
                }
                return _Plugin.IconPath;
            }
        }
        public bool IsEnable
        {
            get
            {
                if (_Plugin == null)
                {
                    return false;
                }
                return _Plugin.IsEnable;
            }
            set
            {
                if (_Plugin == null)
                {
                    if (value != _Plugin.IsEnable)
                    {
                        _Plugin.IsEnable = value;
                        OnPropertyChanged("IsEnable");
                    }
                }
            }
        }
        public ControlTemplate PluginConfigControlTemplate { get; set; }


        protected PluginSystem.IPlugin _Plugin = null;
        protected PluginSystem.PluginConfig _PluginConfig = null;

        public PluginConfigViewModel(PluginSystem.IPlugin Plugin, PluginSystem.PluginConfig PluginConfig)
        {
            _Plugin = Plugin;
            _PluginConfig = PluginConfig;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(String propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
