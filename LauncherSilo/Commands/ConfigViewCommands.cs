using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;

using LauncherSilo.Core.Models;
using LauncherSilo.Models;
using LauncherSilo.ViewModels;

namespace LauncherSilo.Commands
{
    public class SaveConfigCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            FileHelper.XmlFileHelper.SaveXmlFile<Config>(App.AppConfigFilePath, App.MainVM.ConfigVM.AppConfig, App.MainVM.ConfigVM.AppConfig.GetPluginConfigTypes());

        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, null);
        }
    }
    public class CancelConfigCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            FileHelper.XmlFileHelper.LoadXmlFile<Config>(App.AppConfigFilePath, out Config ResultConfig, App.PluginHost.GetPluginConfigTypes());
            App.MainVM.ConfigVM.AppConfig = ResultConfig;
        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, null);
        }
    }

    public class MenuNodeChangedCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {

        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, null);
        }
    }
    public class AddMenuNodeCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            var SelectedNodes = App.MainVM.ConfigVM.SelectedMenuNodes;
            if (SelectedNodes.Count == 0)
            {
                App.MainVM.ConfigVM.RegistedMenuTreeNodes.Add(new MenuTreeNodeViewModel(
                    new MenuTreeNode()
                    {
                        Name = "New",
                        IsVisible = true,
                        Type = MenuType.Menu,
                        CommandInfoID = string.Empty,
                        Children = new ObservableCollection<MenuTreeNode>()
                    }));
            }
            else
            {
                foreach (var Selected in SelectedNodes)
                {
                    if (!Selected.HasExecutableNode)
                    {
                        Selected.Children.Add(new MenuTreeNodeViewModel(
                            new MenuTreeNode()
                            {
                                Name = "New",
                                IsVisible = true,
                                Type = MenuType.Menu,
                                CommandInfoID = string.Empty,
                                Children = new ObservableCollection<MenuTreeNode>()
                            }));
                    }
                }
            }
        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, null);
        }
    }
    public class AddMenuCategoryNodeCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            var SelectedNodes = App.MainVM.ConfigVM.SelectedMenuNodes;
            if (SelectedNodes.Count == 0)
            {
                App.MainVM.ConfigVM.RegistedMenuTreeNodes.Add(new MenuTreeNodeViewModel(
                    new MenuTreeNode()
                    {
                        Name = "NewCategory",
                        IsVisible = true,
                        Type = MenuType.Category,
                        CommandInfoID = string.Empty,
                        Children = new ObservableCollection<MenuTreeNode>()
                    }));
            }
            else
            {
                foreach (var Selected in SelectedNodes)
                {
                    if (!Selected.HasExecutableNode)
                    {
                        Selected.Children.Add(new MenuTreeNodeViewModel(
                        new MenuTreeNode()
                        {
                            Name = "NewCategory",
                            IsVisible = true,
                            Type = MenuType.Category,
                            CommandInfoID = string.Empty,
                            Children = new ObservableCollection<MenuTreeNode>()
                        }));
                    }
                }
            }
        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, null);
        }
    }

    public class DeleteMenuNodeCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            var SelectedNodes = App.MainVM.ConfigVM.SelectedMenuNodes;
            foreach (var DeleteTarget in SelectedNodes)
            {
                foreach (var Node in App.MainVM.ConfigVM.RegistedMenuTreeNodes)
                {
                    App.MainVM.ConfigVM.RemoveMenuNodeReclusive(Node, DeleteTarget);
                }
                App.MainVM.ConfigVM.RegistedMenuTreeNodes.Remove(DeleteTarget);

            }
        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, null);
        }
    }
    public class RegisterNewLauncherCommandCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            App.MainVM.ConfigVM.RegistedLauncherCommandInfos.Insert(0, new LauncherCommandInfoViewModel((LauncherCommandInfo)parameter));
        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, null);
        }
    }
    public class DeleteLauncherCommandCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            App.MainVM.ConfigVM.RegistedLauncherCommandInfos.Remove((LauncherCommandInfoViewModel)parameter);
        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, null);
        }
    }

}
