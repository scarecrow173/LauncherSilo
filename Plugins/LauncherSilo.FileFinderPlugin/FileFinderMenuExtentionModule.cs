using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.IO;

using LauncherSilo.PluginSystem;
using LauncherSilo.Core;
using LauncherSilo.Core.Models;

namespace LauncherSilo.FileFinderPlugin
{
    public class FileFinderMenuExtentionModule : MenuExtentionModuleBase
    {
        public override string Name { get; }
        public override string Description { get; }
        public override IPlugin Owner { get { return _Owner; } }
        private IPlugin _Owner = null;
        private FileFinderPlugin OwnerFileFinderPlugin { get { return _Owner as FileFinderPlugin; } }

        private FileFinderView _FinderView = null;
        private FileFinderViewModel _FileFinderVM = null;


        public override bool Initialize(IPlugin OwnerPlugin)
        {
            _Owner = OwnerPlugin as FileFinderPlugin;
            if (_Owner == null)
            {
                return false;
            }
            _FileFinderVM = new FileFinderViewModel();
            _FinderView = new FileFinderView(_FileFinderVM);
            OwnerFileFinderPlugin.OnConfigPropertyChanged += OwnerFileFinderPlugin_OnConfigPropertyChanged;
            return true;
        }

        private void OwnerFileFinderPlugin_OnConfigPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsEnableFileFinder")
            {
                if (OwnerFileFinderPlugin.PluginConfigVM.IsEnableFileFinder)
                {
                    _FileFinderVM.BeginInitialize();
                }
            }
        }

        public override bool Finalize()
        {
            return true;
        }
        public override SystemMenuTreeNode GenerateExtentionMenuNode()
        {
            return new SystemMenuTreeNode(new EventHandler(OnFileFinderMenu))
            {
                Name = "ファイル検索",
                IsVisible = true,
                Type = MenuType.System,
                CommandInfoID = string.Empty,
                Children = new ObservableCollection<MenuTreeNode>(),
                ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F
            };
        }
        private void OnFileFinderMenu(object sender, EventArgs e)
        {
            _FinderView?.Show();
        }
    }
}
