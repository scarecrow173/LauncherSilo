using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace LauncherSilo.Core.Models
{
    public enum MenuType
    {
        Menu,
        Category,
        System
    }
    [Serializable]
    public class MenuTreeNode
    {
        [System.Xml.Serialization.XmlElement("Name")] public string Name { get; set; } = string.Empty;

        [System.Xml.Serialization.XmlElement("Type")] public MenuType Type { get; set; } = MenuType.Menu;

        [System.Xml.Serialization.XmlElement("IsVisible")] public bool IsVisible { get; set; } = true;

        [System.Xml.Serialization.XmlElement("CommandInfoID")] public string CommandInfoID { get; set; } = string.Empty;

        [System.Xml.Serialization.XmlArray("Children")]
        [System.Xml.Serialization.XmlArrayItem("Child")]
        public ObservableCollection<MenuTreeNode> Children { get; set; } = new ObservableCollection<MenuTreeNode>();

    }
    public class SystemMenuTreeNode : MenuTreeNode
    {
        public System.Windows.Forms.Keys ShortcutKeys { get; set; } = System.Windows.Forms.Keys.None;

        private event EventHandler SystemMenuExecute;
        
        public SystemMenuTreeNode(EventHandler Exec)
        {
            SystemMenuExecute += Exec;
        }
        
        public void Execute()
        {
            if (SystemMenuExecute != null)
            {
                SystemMenuExecute(this, EventArgs.Empty);
            }
        }
    }

}
