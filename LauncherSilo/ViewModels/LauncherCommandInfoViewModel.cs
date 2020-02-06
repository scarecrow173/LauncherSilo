using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using LauncherSilo.Core.Models;
using LauncherSilo.Models;
using LauncherSilo.Commands;

namespace LauncherSilo.ViewModels
{
    public class LauncherCommandInfoViewModel : INotifyPropertyChanged
    {
        public LauncherCommandInfoViewModel(LauncherCommandInfo CommandInfo)
        {
            _CommandInfo = CommandInfo;
        }

        private LauncherCommandInfo _CommandInfo = null;
        public LauncherCommandInfo CommandInfo
        {
            get
            {
                return _CommandInfo;
            }
            set
            {
                if (value != _CommandInfo)
                {
                    _CommandInfo = value;
                    OnPropertyChanged("CommandInfo");
                }
            }
        }
        public string Name
        {
            get
            {
                if (_CommandInfo == null)
                {
                    return string.Empty;
                }
                return _CommandInfo.Name;
            }
            set
            {
                if (value != _CommandInfo.Name)
                {
                    _CommandInfo.Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public string Command
        {
            get
            {
                if (_CommandInfo == null)
                {
                    return string.Empty;
                }
                return _CommandInfo.Command;
            }
            set
            {
                if (value != _CommandInfo.Command)
                {
                    _CommandInfo.Command = value;
                    OnPropertyChanged("Command");
                }
            }
        }
        public string Description
        {
            get
            {
                if (_CommandInfo == null)
                {
                    return string.Empty;
                }
                return _CommandInfo.Description;
            }
            set
            {
                if (value != _CommandInfo.Description)
                {
                    _CommandInfo.Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        public bool IsImmediately
        {
            get
            {
                if (_CommandInfo == null)
                {
                    return false;
                }
                return _CommandInfo.IsImmediately;
            }
            set
            {
                if (value != _CommandInfo.IsImmediately)
                {
                    _CommandInfo.IsImmediately = value;
                    OnPropertyChanged("IsImmediately");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
    }

}
