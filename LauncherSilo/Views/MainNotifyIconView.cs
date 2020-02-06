using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using LauncherSilo.Core.Models;
using LauncherSilo.Models;

namespace LauncherSilo.Views
{
    public partial class MainNotifyIconView : Component
    {
        private Dictionary<System.Windows.Forms.ToolStripItem, MenuTreeNode> MenuNodeDictionary { get; set; } = new Dictionary<System.Windows.Forms.ToolStripItem, MenuTreeNode>();

        public MainNotifyIconView()
        {
            InitializeComponent();
        }

        public MainNotifyIconView(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void ShowMainContextMenu(Point pos)
        {
            MainContextMenuStrip.Show((int)pos.X, (int)pos.Y);
        }
        private void MenuNode_Click(object sender, EventArgs e)
        {
            MenuTreeNode ClickedNode = null;
            MenuNodeDictionary.TryGetValue((System.Windows.Forms.ToolStripItem)sender, out ClickedNode);
            if (ClickedNode != null)
            {
                LauncherCommandInfo Command = App.AppConfig.FindCommandInfo(ClickedNode.CommandInfoID);
                if (Command != null)
                {
                    App.Executer.Execute(Command);
                }
            }
        }
        private void SystemMenuItem_Click(object sender, EventArgs e)
        {
            MenuTreeNode ClickedNode = null;
            MenuNodeDictionary.TryGetValue((System.Windows.Forms.ToolStripItem)sender, out ClickedNode);
            SystemMenuTreeNode SystemMenuNode = ClickedNode as SystemMenuTreeNode;
            if (SystemMenuNode != null)
            {
                SystemMenuNode.Execute();
            }
        }
        private void MainContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            BuildContextMenu();
        }
        public void BuildContextMenu()
        {
            MainContextMenuStrip.Items.Clear();
            MenuNodeDictionary.Clear();
            foreach (var MenuNode in App.AppConfig.ContextMenuNodes)
            {
                var NodeItem = BuildContextMenuItem(MenuNode);
                if (NodeItem != null)
                {
                    MainContextMenuStrip.Items.AddRange(NodeItem);
                }
            }
            bool LastChildIsSeparator = MainContextMenuStrip.Items.Count == 0 ? false :  MainContextMenuStrip.Items[MainContextMenuStrip.Items.Count - 1] is System.Windows.Forms.ToolStripSeparator;
            if (!LastChildIsSeparator)
            {
                MainContextMenuStrip.Items.Add(new System.Windows.Forms.ToolStripSeparator());
            }

            foreach (var SystemMenuNode in App.SystemMenuNodes)
            {
                System.Windows.Forms.ToolStripMenuItem NewItem = new System.Windows.Forms.ToolStripMenuItem(SystemMenuNode.Name);
                NewItem.Click += SystemMenuItem_Click;
                NewItem.ShortcutKeys = SystemMenuNode.ShortcutKeys;
                MainContextMenuStrip.Items.Add(NewItem);
                MenuNodeDictionary.Add(NewItem, SystemMenuNode);
            }

        }
        private System.Windows.Forms.ToolStripItem[] BuildContextMenuItem( MenuTreeNode Node)
        {
            if (!Node.IsVisible)
            {
                return null;
            }
            List<System.Windows.Forms.ToolStripItem> NewItems = new List<System.Windows.Forms.ToolStripItem>();
            if (Node.Type == MenuType.Category)
            {
                foreach (var Child in Node.Children)
                {
                    var ChildItem = BuildContextMenuItem(Child);
                    if (ChildItem != null)
                    {
                        NewItems.AddRange(ChildItem);
                    }
                }
                System.Windows.Forms.ToolStripSeparator separator = new System.Windows.Forms.ToolStripSeparator();
                NewItems.Add(separator);
            }
            else
            {
                System.Windows.Forms.ToolStripMenuItem NewItem = new System.Windows.Forms.ToolStripMenuItem(Node.Name);
                NewItems.Add(NewItem);
                LauncherCommandInfo Command = App.AppConfig.FindCommandInfo(Node.CommandInfoID);
                if (Command != null && Command.Type != CommandExecuteType.None)
                {
                    NewItem.ToolTipText = Command.Description;
                    NewItem.Click += MenuNode_Click;
                    MenuNodeDictionary.Add(NewItem, Node);
                }
                foreach (var Child in Node.Children)
                {
                    var ChildItem = BuildContextMenuItem(Child);
                    if (ChildItem != null)
                    {
                        NewItem.DropDownItems.AddRange(ChildItem);
                    }
                }
                
                bool LastChildIsSeparator = NewItem.DropDownItems.Count == 0 ? false : NewItem.DropDownItems[NewItem.DropDownItems.Count - 1] is System.Windows.Forms.ToolStripSeparator;
                if (LastChildIsSeparator)
                {
                    NewItem.DropDownItems.RemoveAt(NewItem.DropDownItems.Count - 1);
                }
            }
            
            return NewItems.ToArray();

        }
    }
}
