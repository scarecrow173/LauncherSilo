using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace LauncherSilo.ViewModels
{
    public class ContextOverlayViewModel : INotifyPropertyChanged
    {



        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
