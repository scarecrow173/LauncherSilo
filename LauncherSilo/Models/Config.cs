using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Security.Permissions;

using LauncherSilo.Core.Models;

namespace LauncherSilo.Models
{
    public enum LayoutStyles
    {
        ContextMenu,
        TileButton
    }
    [Serializable]
    public class Config
    {
        public LayoutStyles Style { get; set; } = LayoutStyles.ContextMenu;
        public int HorizonalTileSize { get; set; } = 16;
        public bool IsStartup { get; set; } = false;
        public bool IsImmediately { get; set; } = false;
        public bool IsCopyToClipboard { get; set; } = false;
        public ShrotcutKeySetting ShortcutKey { get; set; } = new ShrotcutKeySetting();

        public ObservableCollection<LauncherCommandInfo> CommandList { get; set; } = new ObservableCollection<LauncherCommandInfo>();
        public ObservableCollection<MenuTreeNode> ContextMenuNodes { get; set; } = new ObservableCollection<MenuTreeNode>();

        public ObservableCollection<PluginSystem.PluginConfig> PluginConfigs { get; set; } = new ObservableCollection<PluginSystem.PluginConfig>();



        private Dictionary<string, LauncherCommandInfo> CommandDictionary { get; set; } = null;

        public LauncherCommandInfo FindCommandInfo(string UID)
        {
            if (CommandDictionary == null)
            {
                BuildCommandDictionary();
            }
            if (CommandDictionary.ContainsKey(UID))
            {
                return CommandDictionary[UID];
            }
            else
            {
                return null;
            }
        }
        private void CommandList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            BuildCommandDictionary();
        }
        public void BuildCommandDictionary()
        {
            if (CommandDictionary == null)
            {
                CommandDictionary = new Dictionary<string, LauncherCommandInfo>();
            }
            else
            {
                CommandDictionary.Clear();
            }
            foreach(var Command in CommandList)
            {
                CommandDictionary.Add(Command.UID, Command);
            }

        }

        public Type[] GetPluginConfigTypes()
        {
            return PluginConfigs.Select(x => x.GetType()).ToArray();
        }
    }
}
