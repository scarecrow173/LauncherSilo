using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LauncherSilo.PluginSystem;

namespace LauncherSilo.Core
{
    public abstract class CommandExecuteModuleBase : LauncherSiloModuleBase
    {
        public abstract bool CanExecute(string Command, bool IsImmediately);
        public abstract void Execute(string Command, bool IsImmediately);


    }
}
