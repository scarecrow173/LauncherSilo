using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LauncherSilo.PluginSystem;
using LauncherSilo.Core.Models;

namespace LauncherSilo.Core
{
    public abstract class MenuExtentionModuleBase : LauncherSiloModuleBase
    {
        public abstract SystemMenuTreeNode GenerateExtentionMenuNode();
    }
}
