using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace LauncherSilo.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainNotifyIconViewModel MainNotifyIconVM { get; set; } = new MainNotifyIconViewModel();
        public ConfigViewModel ConfigVM { get; set; } = new ConfigViewModel();
        public ContextOverlayViewModel ContextOverlayVM { get; set; } = new ContextOverlayViewModel();


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
