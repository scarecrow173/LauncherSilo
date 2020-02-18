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
    public class MenuTreeNodeViewModel : INotifyPropertyChanged
    {
        public MenuTreeNodeViewModel(MenuTreeNode Node)
        {
            _MenuNode = Node;
            foreach (var ChildNode in Node.Children)
            {
                _Children.Add(new MenuTreeNodeViewModel(ChildNode));
            }
            _Children.CollectionChanged += Children_CollectionChanged;
        }

        private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _MenuNode.Children.Clear();
            foreach (var Child in _Children)
            {
                _MenuNode.Children.Add(Child.MenuNode);
            }
        }
        public AddMenuNodeCommand AddMenuNode { get; private set; } = new AddMenuNodeCommand();
        public AddMenuCategoryNodeCommand AddMenuCategoryNode { get; private set; } = new AddMenuCategoryNodeCommand();
        public DeleteMenuNodeCommand DeleteMenuNode { get; private set; } = new DeleteMenuNodeCommand();


        private MenuTreeNode _MenuNode = null;
        public MenuTreeNode MenuNode
        {
            get
            {
                return _MenuNode;
            }
            set
            {
                if (value != _MenuNode)
                {
                    _MenuNode = value;
                    OnPropertyChanged("MenuNode");
                    OnPropertyChanged("Name");
                    OnPropertyChanged("IsExpanded");
                    OnPropertyChanged("IsItemSelected");
                    OnPropertyChanged("IsVisible");
                    OnPropertyChanged("Type");
                    OnPropertyChanged("Children");
                    OnPropertyChanged("HasExecutableNode");
                }
            }
        }
        public string Name
        {
            get
            {
                if (_MenuNode == null)
                {
                    return string.Empty;
                }
                return _MenuNode.Name;
            }
            set
            {
                if (value != _MenuNode.Name)
                {
                    _MenuNode.Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        private bool _IsExpanded = false;
        public bool IsExpanded
        {
            get
            {
                return _IsExpanded;
            }
            set
            {
                if (value != _IsExpanded)
                {
                    _IsExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }
            }
        }

        private bool _IsItemSelected = false;
        public bool IsItemSelected
        {
            get
            {
                return _IsItemSelected;
            }
            set
            {
                if (value != _IsItemSelected)
                {
                    _IsItemSelected = value;
                    OnPropertyChanged("IsItemSelected");
                }
            }
        }
        public bool IsVisible
        {
            get
            {
                if (_MenuNode == null)
                {
                    return false;
                }
                return _MenuNode.IsVisible;
            }
            set
            {
                if (value != _MenuNode.IsVisible)
                {
                    _MenuNode.IsVisible = value;
                    OnPropertyChanged("IsVisible");
                }
            }
        }
        public MenuType Type
        {
            get
            {
                if (_MenuNode == null)
                {
                    return MenuType.Menu;
                }
                return _MenuNode.Type;
            }
            set
            {
                if (value != _MenuNode.Type)
                {
                    _MenuNode.Type = value;
                    OnPropertyChanged("Type");
                }
            }
        }
        public bool HasExecutableNode
        {
            get
            {
                if (_MenuNode == null)
                {
                    return false;
                }
                // 毎回Findはえぐいけどとりあえずいいや
                LauncherCommandInfo CommandInfo = App.AppConfig.FindCommandInfo(_MenuNode.CommandInfoID);
                if (CommandInfo == null)
                {
                    return false;
                }
                return CommandInfo.Type != CommandExecuteType.None;
            }
        }
        private ObservableCollection<MenuTreeNodeViewModel> _Children = new ObservableCollection<MenuTreeNodeViewModel>();
        public ObservableCollection<MenuTreeNodeViewModel> Children
        {
            get
            {
                return _Children;
            }
            set
            {
                if (value != _Children)
                {
                    _Children = value;
                    OnPropertyChanged("Children");
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
