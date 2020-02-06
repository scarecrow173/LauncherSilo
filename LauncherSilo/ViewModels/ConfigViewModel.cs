using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Data;
using System.Globalization;
using System.Collections.Specialized;
using System.Windows.Controls;

using GongSolutions.Wpf.DragDrop;

using LauncherSilo.Core.Models;
using LauncherSilo.Models;
using LauncherSilo.Commands;

namespace LauncherSilo.ViewModels
{

    public class ConfigViewModel : INotifyPropertyChanged, IDropTarget
    {
        
        public SaveConfigCommand SaveConfig { get; private set; } = new SaveConfigCommand();
        public CancelConfigCommand CancelConfig { get; private set; } = new CancelConfigCommand();
        public MenuNodeChangedCommand MenuNodeChanged { get; private set; } = new MenuNodeChangedCommand();
        public AddMenuNodeCommand AddMenuNode { get; private set; } = new AddMenuNodeCommand();
        public AddMenuCategoryNodeCommand AddMenuCategoryNode { get; private set; } = new AddMenuCategoryNodeCommand();
        public DeleteMenuNodeCommand DeleteMenuNode { get; private set; } = new DeleteMenuNodeCommand();
        public RegisterNewLauncherCommandCommand RegisterNewLauncherCommand { get; private set; } = new RegisterNewLauncherCommandCommand();
        public DeleteLauncherCommandCommand DeleteLauncherCommand { get; private set; } = new DeleteLauncherCommandCommand();

        public Config AppConfig
        {
            get
            {
                return App.AppConfig;
            }
            set
            {
                if (value != App.AppConfig)
                {
                    App.ApplyConfig(value);

                    BuildMenuTree();
                    OnPropertyChanged("RegistedMenuTreeNodes");

                    SelectedMenuNodes.Clear();
                    OnPropertyChanged("SelectedMenuNodes");

                    BuildLauncherCommandInfos();
                    OnPropertyChanged("RegistedLauncherCommandInfos");

                    SelectedLauncherCommand = null;
                    OnPropertyChanged("SelectedLauncherCommand");

                    OnPropertyChanged("IsLayoutStyleContextMenu");
                    OnPropertyChanged("IsLayoutStyleTileButton");
                    OnPropertyChanged("VerticalTileSize");
                    OnPropertyChanged("HorizonalTileSize");

                    
                    OnPropertyChanged("IsStartup");
                    OnPropertyChanged("IsGeneralImmediately");
                    OnPropertyChanged("IsCopyToClipboard");
                    OnPropertyChanged("IsUseAltKey");
                    OnPropertyChanged("IsUseShiftKey");
                    OnPropertyChanged("IsUseCtrlKey");
                    OnPropertyChanged("ShortcutKeyName");
                    OnPropertyChanged("ConfigTabItems");

                }
            }
        }
        public bool IsLayoutStyleContextMenu
        {
            get
            {
                if (AppConfig == null)
                {
                    return false;
                }
                return AppConfig.Style == LayoutStyles.ContextMenu;
            }
            set
            {
                if (AppConfig != null)
                {
                    if (value != (App.AppConfig.Style == LayoutStyles.ContextMenu))
                    {
                        App.AppConfig.Style = LayoutStyles.ContextMenu;
                        OnPropertyChanged("IsLayoutStyleContextMenu");
                        OnPropertyChanged("IsLayoutStyleTileButton");
                    }
                }
            }
        }
        public bool IsLayoutStyleTileButton
        {
            get
            {
                if (AppConfig == null)
                {
                    return false;
                }
                return AppConfig.Style == LayoutStyles.TileButton;
            }
            set
            {
                if (AppConfig != null)
                {
                    if (value != (App.AppConfig.Style == LayoutStyles.TileButton))
                    {
                        App.AppConfig.Style = LayoutStyles.TileButton;
                        OnPropertyChanged("IsLayoutStyleTileButton");
                        OnPropertyChanged("IsLayoutStyleContextMenu");
                    }
                }
            }
        }

        public int HorizonalTileSize
        {
            get
            {
                if (AppConfig == null)
                {
                    return 16;
                }
                return AppConfig.HorizonalTileSize;
            }
            set
            {
                if (AppConfig != null)
                {
                    if (value != App.AppConfig.HorizonalTileSize)
                    {
                        App.AppConfig.HorizonalTileSize = value;
                        OnPropertyChanged("HorizonalTileSize");
                    }
                }
            }
        }
        public bool IsStartup
        {
            get
            {
                if (AppConfig == null)
                {
                    return false;
                }
                return AppConfig.IsStartup;
            }
            set
            {
                if (AppConfig != null)
                {
                    if (value != App.AppConfig.IsStartup)
                    {
                        App.AppConfig.IsStartup = value;
                        App.ApplyStartupLink(App.AppConfig.IsStartup);
                        OnPropertyChanged("IsStartup");
                    }
                }
            }
        }

        public bool IsGeneralImmediately
        {
            get
            {
                if (AppConfig == null)
                {
                    return false;
                }
                return AppConfig.IsImmediately;
            }
            set
            {
                if (AppConfig != null)
                {
                    if (value != App.AppConfig.IsImmediately)
                    {
                        App.AppConfig.IsImmediately = value;
                        OnPropertyChanged("IsGeneralImmediately");
                    }
                }
            }
        }
        public bool IsCopyToClipboard
        {
            get
            {
                if (AppConfig == null)
                {
                    return false;
                }
                return AppConfig.IsCopyToClipboard;
            }
            set
            {
                if (AppConfig != null)
                {
                    if (value != App.AppConfig.IsCopyToClipboard)
                    {
                        App.AppConfig.IsCopyToClipboard = value;
                        OnPropertyChanged("IsCopyToClipboard");
                    }
                }
            }
        }

        public bool IsUseAltKey
        {
            get
            {
                if (AppConfig == null)
                {
                    return false;
                }
                return AppConfig.ShortcutKey.IsUseAltKey;
            }
            set
            {
                if (AppConfig != null)
                {
                    if (value != AppConfig.ShortcutKey.IsUseAltKey)
                    {
                        AppConfig.ShortcutKey.IsUseAltKey = value;
                        App.ApplyShortcutKey(AppConfig.ShortcutKey.IsUseAltKey, AppConfig.ShortcutKey.IsUseShiftKey, AppConfig.ShortcutKey.IsUseCtrlKey, AppConfig.ShortcutKey.Key);
                        OnPropertyChanged("IsUseAltKey");
                    }
                }
            }
        }
        public bool IsUseShiftKey
        {
            get
            {
                if (AppConfig == null)
                {
                    return false;
                }
                return AppConfig.ShortcutKey.IsUseShiftKey;
            }
            set
            {
                if (AppConfig != null)
                {
                    if (value != AppConfig.ShortcutKey.IsUseShiftKey)
                    {
                        AppConfig.ShortcutKey.IsUseShiftKey = value;
                        App.ApplyShortcutKey(AppConfig.ShortcutKey.IsUseAltKey, AppConfig.ShortcutKey.IsUseShiftKey, AppConfig.ShortcutKey.IsUseCtrlKey, AppConfig.ShortcutKey.Key);
                        OnPropertyChanged("IsUseShiftKey");
                    }
                }
            }
        } 
        public bool IsUseCtrlKey
        {
            get
            {
                if (AppConfig == null)
                {
                    return false;
                }
                return AppConfig.ShortcutKey.IsUseCtrlKey;
            }
            set
            {
                if (AppConfig != null)
                {
                    if (value != AppConfig.ShortcutKey.IsUseCtrlKey)
                    {
                        AppConfig.ShortcutKey.IsUseCtrlKey = value;
                        App.ApplyShortcutKey(AppConfig.ShortcutKey.IsUseAltKey, AppConfig.ShortcutKey.IsUseShiftKey, AppConfig.ShortcutKey.IsUseCtrlKey, AppConfig.ShortcutKey.Key);
                        OnPropertyChanged("IsUseCtrlKey");
                    }
                }
            }
        } 
        public System.Windows.Forms.Keys ShortcutKeyName
        {
            get
            {
                if (AppConfig == null)
                {
                    return System.Windows.Forms.Keys.None;
                }
                return AppConfig.ShortcutKey.Key;
            }
            set
            {
                if (AppConfig != null)
                {
                    if (value != AppConfig.ShortcutKey.Key)
                    {
                        AppConfig.ShortcutKey.Key = value;
                        App.ApplyShortcutKey(AppConfig.ShortcutKey.IsUseAltKey, AppConfig.ShortcutKey.IsUseShiftKey, AppConfig.ShortcutKey.IsUseCtrlKey, AppConfig.ShortcutKey.Key);
                        OnPropertyChanged("ShortcutKeyName");
                    }
                }
            }
        }


        private ObservableCollection<MenuTreeNodeViewModel> _RegistedMenuTreeNodes = null;

        public ObservableCollection<MenuTreeNodeViewModel> RegistedMenuTreeNodes
        {
            get
            {
                if(_RegistedMenuTreeNodes == null)
                {
                    BuildMenuTree();
                }
                return _RegistedMenuTreeNodes;
            }
            set
            {
                if (value != _RegistedMenuTreeNodes)
                {
                    _RegistedMenuTreeNodes = value;
                    OnPropertyChanged("RegistedMenuTreeNodes");
                }
            }
        }

        public IList<MenuTreeNodeViewModel> _SelectedMenuNodes  = new ObservableCollection<MenuTreeNodeViewModel>();
        public IList<MenuTreeNodeViewModel> SelectedMenuNodes
        {
            get
            {
                return _SelectedMenuNodes;
            }
            set
            {
                if (value != _SelectedMenuNodes)
                {
                    _SelectedMenuNodes = value;
                    OnPropertyChanged("SelectedMenuNodes");
                }
            }
        } 


        private ObservableCollection<LauncherCommandInfoViewModel> _RegistedLauncherCommandInfos = null;

        public ObservableCollection<LauncherCommandInfoViewModel> RegistedLauncherCommandInfos
        {
            get
            {
                if (_RegistedLauncherCommandInfos == null)
                {
                    BuildLauncherCommandInfos();
                }
                return _RegistedLauncherCommandInfos;
            }

            set
            {
                if (value != _RegistedLauncherCommandInfos)
                {
                    _RegistedLauncherCommandInfos = value;
                    OnPropertyChanged("RegistedLauncherCommandInfos");
                }
            }
        }
        private LauncherCommandInfoViewModel _SelectedLauncherCommand = null;
        public LauncherCommandInfoViewModel SelectedLauncherCommand
        {
            get
            {
                return _SelectedLauncherCommand;
            }
            set
            {
                if (_SelectedLauncherCommand != value)
                {
                    _SelectedLauncherCommand = value;
                    OnPropertyChanged("SelectedLauncherCommand");

                }
            }
        }

        public ObservableCollection<ConfigTabViewModel> ConfigTabItems
        {
            get
            {
                if (_ConfigTabItems == null)
                {
                    BuildConfigTabItems();
                }
                return _ConfigTabItems;
            }

            set
            {
                if (value != _ConfigTabItems)
                {
                    _ConfigTabItems = value;
                    OnPropertyChanged("ConfigTabItems");
                }
            }
        }
        private ObservableCollection<ConfigTabViewModel> _ConfigTabItems = null;



        public void BuildMenuTree()
        {
            if (_RegistedMenuTreeNodes == null)
            {
                _RegistedMenuTreeNodes = new ObservableCollection<MenuTreeNodeViewModel>();
            }

            _RegistedMenuTreeNodes.CollectionChanged -= RegistedMenuTreeNodes_CollectionChanged;
            _RegistedMenuTreeNodes.Clear();
            if (App.AppConfig != null)
            {
                var ContextMenuNodesResult = App.AppConfig.ContextMenuNodes.Select(x => new MenuTreeNodeViewModel(x));
                foreach (var item in ContextMenuNodesResult)
                {
                    _RegistedMenuTreeNodes.Add(item);
                }
            }

            _RegistedMenuTreeNodes.CollectionChanged += RegistedMenuTreeNodes_CollectionChanged;
        }

        public void OrganizeMenuTree()
        {
            if (_RegistedMenuTreeNodes == null)
            {
                return;
            }

            List<MenuTreeNodeViewModel> RemoveTargets = new List<MenuTreeNodeViewModel>();
            foreach (var MenuNodeChild in _RegistedMenuTreeNodes)
            {
                if (OrganizeMenuTreeChildren(MenuNodeChild, App.AppConfig.CommandList))
                {
                    RemoveTargets.Add(MenuNodeChild);
                }
            }
            foreach (var RemoveTarget in RemoveTargets)
            {
                _RegistedMenuTreeNodes.Remove(RemoveTarget);
            }
        }
        private bool OrganizeMenuTreeChildren(MenuTreeNodeViewModel MenuTreeNodeVM, ObservableCollection<LauncherCommandInfo> SrcList)
        {
            if (MenuTreeNodeVM.MenuNode.CommandInfoID != string.Empty)
            {
                return SrcList.Count(x => { return MenuTreeNodeVM.MenuNode.CommandInfoID == x.UID; }) == 0;
            }
            else
            {
                List<MenuTreeNodeViewModel> RemoveTargets = new List<MenuTreeNodeViewModel>();
                foreach (var MenuNodeChild in MenuTreeNodeVM.Children)
                {
                    if (OrganizeMenuTreeChildren(MenuNodeChild, SrcList))
                    {
                        RemoveTargets.Add(MenuNodeChild);
                    }
                }

                foreach (var RemoveTarget in RemoveTargets)
                {
                    MenuTreeNodeVM.Children.Remove(RemoveTarget);
                }
                return false;
            }

        }
        public void BuildLauncherCommandInfos()
        {
            if (_RegistedLauncherCommandInfos == null)
            {
                _RegistedLauncherCommandInfos = new ObservableCollection<LauncherCommandInfoViewModel>();
            }

            _RegistedLauncherCommandInfos.CollectionChanged -= RegistedLauncherCommandInfos_CollectionChanged;
            _RegistedLauncherCommandInfos.Clear();
            if (App.AppConfig != null)
            {
                var ContextMenuNodesResult = App.AppConfig.CommandList.Select(x => new LauncherCommandInfoViewModel(x));
                foreach (var item in ContextMenuNodesResult)
                {
                    _RegistedLauncherCommandInfos.Add(item);
                }
            }

            _RegistedLauncherCommandInfos.CollectionChanged += RegistedLauncherCommandInfos_CollectionChanged;

        }

        public void ApplyMenuTreeChanged()
        {
            AppConfig.ContextMenuNodes.Clear();
            foreach (var MenuNodeVM in _RegistedMenuTreeNodes)
            {
                AppConfig.ContextMenuNodes.Add(MenuNodeVM.MenuNode);
            }
        }
        public void ApplyLauncherCommandInfosChanged()
        {
            AppConfig.CommandList.Clear();
            foreach (var LauncherCommandVM in _RegistedLauncherCommandInfos)
            {
                AppConfig.CommandList.Add(LauncherCommandVM.CommandInfo);
            }
            AppConfig.BuildCommandDictionary();
            OrganizeMenuTree();
        }


        private void RegistedMenuTreeNodes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ApplyMenuTreeChanged();
        }
        private void RegistedLauncherCommandInfos_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ApplyLauncherCommandInfosChanged();
        }

        private void BuildConfigTabItems()
        {
            _ConfigTabItems = new ObservableCollection<ConfigTabViewModel>()
            {
                new ConfigTabViewModel() { TabType = EConfigTabType.General },
                new ConfigTabViewModel() { TabType = EConfigTabType.Display },
                new ConfigTabViewModel() { TabType = EConfigTabType.RegisterLauncherCommand },
            };
            if (AppConfig?.PluginConfigs != null)
            {
                foreach(var Plugin in App.PluginManagerInstance.LauncherSiloPlugins)
                {
                    _ConfigTabItems.Add(new ConfigTabViewModel()
                    {
                        TabType = EConfigTabType.Plugin,
                        PluginConfigVM = Plugin.CreatePluginConfigViewModel()
                    });
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }

        public void DragOver(IDropInfo dropInfo)
        {
            LauncherCommandInfoViewModel SourceCommandItem = dropInfo.Data as LauncherCommandInfoViewModel;
            LauncherCommandInfoViewModel TargetCommandItem = dropInfo.TargetItem as LauncherCommandInfoViewModel;
            MenuTreeNodeViewModel SourceMenuItem = dropInfo.Data as MenuTreeNodeViewModel;
            MenuTreeNodeViewModel TargetMenuItem = dropInfo.TargetItem as MenuTreeNodeViewModel;
            ObservableCollection<MenuTreeNodeViewModel> Targets = dropInfo.TargetCollection as ObservableCollection<MenuTreeNodeViewModel>;
            List<object> DataList = dropInfo.Data as List<object>;
            List<LauncherCommandInfoViewModel> SourceCommandItems = null;
            List<MenuTreeNodeViewModel> SourceMenuItems = null;
            if (DataList != null && DataList.Count > 0)
            {
                if (DataList[0] is LauncherCommandInfoViewModel)
                {
                    SourceCommandItems = new List<LauncherCommandInfoViewModel>(DataList.Select(x => (LauncherCommandInfoViewModel)x));

                }
                else if (DataList[0] is MenuTreeNodeViewModel)
                {
                    SourceMenuItems = new List<MenuTreeNodeViewModel>(DataList.Select(x => (MenuTreeNodeViewModel)x));
                }
            }
            if (Targets != null)
            {
                if (SourceCommandItem != null || SourceCommandItems != null)
                {
                    dropInfo.Effects = DragDropEffects.Copy;
                }
                else if (SourceMenuItem != null || SourceMenuItems != null)
                {
                    dropInfo.Effects = DragDropEffects.Move;
                }
                if (dropInfo.InsertPosition == RelativeInsertPosition.None || dropInfo.InsertPosition.HasFlag(RelativeInsertPosition.TargetItemCenter))
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                    if (TargetMenuItem != null && TargetMenuItem.HasExecutableNode)
                    {
                        dropInfo.DropTargetAdorner = null;
                        dropInfo.Effects = DragDropEffects.None;
                    }
                    
                }
                else if (dropInfo.InsertPosition.HasFlag(RelativeInsertPosition.BeforeTargetItem) || dropInfo.InsertPosition.HasFlag(RelativeInsertPosition.AfterTargetItem))
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                }
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            LauncherCommandInfoViewModel SourceCommandItem = dropInfo.Data as LauncherCommandInfoViewModel;
            LauncherCommandInfoViewModel TargetCommandItem = dropInfo.TargetItem as LauncherCommandInfoViewModel;
            MenuTreeNodeViewModel SourceMenuItem = dropInfo.Data as MenuTreeNodeViewModel;
            MenuTreeNodeViewModel TargetMenuItem = dropInfo.TargetItem as MenuTreeNodeViewModel;
            ObservableCollection<MenuTreeNodeViewModel> Targets = dropInfo.TargetCollection as ObservableCollection<MenuTreeNodeViewModel>;
            List<object> DataList = dropInfo.Data as List<object>;
            List<LauncherCommandInfoViewModel> SourceCommandItems = null;
            List<MenuTreeNodeViewModel> SourceMenuItems = null;
            if (DataList != null && DataList.Count > 0)
            {
                if (DataList[0] is LauncherCommandInfoViewModel)
                {
                    SourceCommandItems = new List<LauncherCommandInfoViewModel>(DataList.Select(x => (LauncherCommandInfoViewModel)x));

                }
                if (DataList[0] is MenuTreeNodeViewModel)
                {
                    SourceMenuItems = new List<MenuTreeNodeViewModel>(DataList.Select(x => (MenuTreeNodeViewModel)x));
                }
            }
            else
            {
                if (SourceCommandItem != null)
                {
                    SourceCommandItems = new List<LauncherCommandInfoViewModel>()
                    {
                        SourceCommandItem
                    };
                }
                if (SourceMenuItem != null)
                {
                    SourceMenuItems = new List<MenuTreeNodeViewModel>();
                    if (SelectedMenuNodes.Count > 1)
                    {
                        SourceMenuItems.AddRange(SelectedMenuNodes);
                    }
                    else
                    {
                        SourceMenuItems.Add(SourceMenuItem);
                    }
                }
            }
            if (Targets != null)
            {
                if (SourceCommandItems != null)
                {
                    DropCommandItemsToMenuTree(dropInfo, SourceCommandItems, TargetMenuItem, Targets);
                }
                if (SourceMenuItems != null)
                {
                    DropMenuItemsToMenuTree(dropInfo, SourceMenuItems, TargetMenuItem, Targets);
                }
                SelectedMenuNodes.Clear();
            }
        }
        private void DropCommandItemsToMenuTree(IDropInfo dropInfo, List<LauncherCommandInfoViewModel> SourceCommandItems, MenuTreeNodeViewModel TargetMenuItem, ObservableCollection<MenuTreeNodeViewModel> Targets)
        {
            if (dropInfo.DropTargetAdorner == DropTargetAdorners.Highlight)
            {
                AddCommandItemsToMenuTree(dropInfo, SourceCommandItems, TargetMenuItem, Targets);
            }
            else if(dropInfo.DropTargetAdorner == DropTargetAdorners.Insert)
            {
                InsertCommandItemsToMenuTree(dropInfo, SourceCommandItems, TargetMenuItem, Targets);
            }
        }
        private void AddCommandItemsToMenuTree(IDropInfo dropInfo, List<LauncherCommandInfoViewModel> SourceCommandItems, MenuTreeNodeViewModel TargetMenuItem, ObservableCollection<MenuTreeNodeViewModel> Targets)
        {
            if (TargetMenuItem != null)
            {
                if (!TargetMenuItem.HasExecutableNode)
                {
                    foreach (var SourceItem in SourceCommandItems)
                    {
                        TargetMenuItem.Children.Add(new MenuTreeNodeViewModel(
                            new MenuTreeNode()
                            {
                                Name = SourceItem.Name,
                                IsVisible = true,
                                Type = MenuType.Menu,
                                CommandInfoID = SourceItem.CommandInfo.UID,
                                Children = new ObservableCollection<MenuTreeNode>()
                            }));
                    }
                }
            }
            else
            {
                foreach (var SourceItem in SourceCommandItems)
                {
                    RegistedMenuTreeNodes.Add(new MenuTreeNodeViewModel(
                    new MenuTreeNode()
                    {
                        Name = SourceItem.Name,
                        IsVisible = true,
                        Type = MenuType.Menu,
                        CommandInfoID = SourceItem.CommandInfo.UID,
                        Children = new ObservableCollection<MenuTreeNode>()
                    }));
                }
            }
        }
        private void InsertCommandItemsToMenuTree(IDropInfo dropInfo, List<LauncherCommandInfoViewModel> SourceCommandItems, MenuTreeNodeViewModel TargetMenuItem, ObservableCollection<MenuTreeNodeViewModel> Targets)
        {
            List<MenuTreeNodeViewModel> InsertItems = new List<MenuTreeNodeViewModel>();
            foreach (var SourceItem in SourceCommandItems)
                {
                    InsertItems.Add(new MenuTreeNodeViewModel(
                    new MenuTreeNode()
                    {
                        Name = SourceItem.Name,
                        IsVisible = true,
                        Type = MenuType.Menu,
                        CommandInfoID = SourceItem.CommandInfo.UID,
                        Children = new ObservableCollection<MenuTreeNode>()
                    }));
                }
            if (InsertItems.Count > 0)
            {
                if (dropInfo.InsertIndex >= Targets.Count)
                {
                    foreach (var InsertItem in InsertItems)
                    {
                        Targets.Add(InsertItem);
                    }
                }
                else
                {
                    foreach (var InsertItem in InsertItems.Select((item, index) => new { item, index }))
                    {
                        Targets.Insert(dropInfo.InsertIndex + InsertItem.index, InsertItem.item);
                    }
                }
            }
        }
        private void DropMenuItemsToMenuTree(IDropInfo dropInfo, List<MenuTreeNodeViewModel> SourceMenuItems, MenuTreeNodeViewModel TargetMenuItem, ObservableCollection<MenuTreeNodeViewModel> Targets)
        {
            if (dropInfo.DropTargetAdorner == DropTargetAdorners.Highlight)
            {
                AddMenuItemsToMenuTree(dropInfo, SourceMenuItems, TargetMenuItem, Targets);
            }
            else if (dropInfo.DropTargetAdorner == DropTargetAdorners.Insert)
            {
                InsertMenuItemsToMenuTree(dropInfo, SourceMenuItems, TargetMenuItem, Targets);
            }
        }
        private void AddMenuItemsToMenuTree(IDropInfo dropInfo, List<MenuTreeNodeViewModel> SourceMenuItems, MenuTreeNodeViewModel TargetMenuItem, ObservableCollection<MenuTreeNodeViewModel> Targets)
        {
            foreach (var SourceItem in SourceMenuItems)
            {
                foreach (var Node in RegistedMenuTreeNodes)
                {
                    RemoveMenuNodeReclusive(Node, SourceItem);
                }
                RegistedMenuTreeNodes.Remove(SourceItem);

                if (TargetMenuItem != null)
                {
                    if (!TargetMenuItem.HasExecutableNode)
                    {
                        TargetMenuItem.Children.Add(SourceItem);
                    }
                }
                else
                {
                    RegistedMenuTreeNodes.Add(SourceItem);
                }
            }
        }
        private void InsertMenuItemsToMenuTree(IDropInfo dropInfo, List<MenuTreeNodeViewModel> SourceMenuItems, MenuTreeNodeViewModel TargetMenuItem, ObservableCollection<MenuTreeNodeViewModel> Targets)
        {
            List<MenuTreeNodeViewModel> InsertItems = new List<MenuTreeNodeViewModel>();
            foreach (var SourceItem in SourceMenuItems)
            {
                foreach (var Node in RegistedMenuTreeNodes)
                {
                    RemoveMenuNodeReclusive(Node, SourceItem);
                }
                RegistedMenuTreeNodes.Remove(SourceItem);
                InsertItems.Add(SourceItem);

            }
            if (InsertItems.Count > 0)
            {
                if (dropInfo.InsertIndex >= Targets.Count)
                {
                    foreach (var InsertItem in InsertItems)
                    {
                        Targets.Add(InsertItem);
                    }
                }
                else
                {
                    foreach (var InsertItem in InsertItems.Select((item, index) => new { item, index }))
                    {
                        Targets.Insert(dropInfo.InsertIndex + InsertItem.index, InsertItem.item);
                    }
                }
            }
        }

        public void RemoveMenuNodeReclusive(MenuTreeNodeViewModel Node, MenuTreeNodeViewModel RemoveTarget)
        {
            foreach (var Child in Node.Children)
            {
                RemoveMenuNodeReclusive(Child, RemoveTarget);
            }
            Node.Children.Remove(RemoveTarget);
        }

    }

}
