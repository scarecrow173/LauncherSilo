using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Controls;

using LauncherSilo.Core.ViewModels;

namespace LauncherSilo.ViewModels
{
    public enum EConfigTabType
    {
        None,
        General,
        Display,
        RegisterLauncherCommand,
        Plugin
    }
    public class ConfigTabViewModel : INotifyPropertyChanged
    {
        public string Header
        {
            get
            {
                switch(TabType)
                {
                    case EConfigTabType.None:
                        return string.Empty;
                    case EConfigTabType.General:
                        return "全般";
                    case EConfigTabType.Display:
                        return "表示";
                    case EConfigTabType.RegisterLauncherCommand:
                        return "コマンド登録";
                    case EConfigTabType.Plugin:
                        return PluginConfigVM?.Name;
                    default:
                        return string.Empty;

                }
            }
        }

        public EConfigTabType TabType
        {
            get { return _TabType; }
            set { if (value != _TabType) { _TabType = value; } }
        }
        public EConfigTabType _TabType = EConfigTabType.None;

        public ConfigViewModel ConfigVM
        {
            get { return App.MainVM.ConfigVM; }
        }
        public PluginConfigViewModel PluginConfigVM
        {
            get { return _PluginConfigVM; }
            set
            {
                if (value != _PluginConfigVM)
                {
                    _PluginConfigVM = value;
                    OnPropertyChanged("PluginConfigVM");
                }
            }
        }
        private PluginConfigViewModel _PluginConfigVM = null;


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
