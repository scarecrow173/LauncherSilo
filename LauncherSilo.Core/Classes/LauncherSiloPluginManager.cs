using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LauncherSilo.PluginSystem;

namespace LauncherSilo.Core
{
    public class LauncherSiloPluginManager : PluginManager
    {
        public LauncherSiloPluginBase[] LauncherSiloPlugins { get { return _LauncherSiloPlugins; } }
        private LauncherSiloPluginBase[] _LauncherSiloPlugins = null;

        public CommandExecuteModuleBase[] CommandExecuteModules { get { return _CommandExecuteModules; } }
        private CommandExecuteModuleBase[] _CommandExecuteModules = null;

        public MenuExtentionModuleBase[] MenuExtentionModules { get { return _MenuExtentionModules; } }
        private MenuExtentionModuleBase[] _MenuExtentionModules = null;

        public LauncherSiloPluginManager(IPluginHost Host) : base(Host)
        {
        }

        protected override void PostLoadPlugins()
        {
            var EnumerateValidPlugins = _Plugins.Values.Where(x => x.GetType().IsSubclassOf(typeof(LauncherSiloPluginBase))).Select(x => x as LauncherSiloPluginBase);
            _LauncherSiloPlugins = EnumerateValidPlugins.ToArray();

            var EnumerateCommandExecuteModules = EnumerateValidPlugins
                .SelectMany(x => x.PluginModules.Where(y => y.GetType().IsSubclassOf(typeof(CommandExecuteModuleBase))))
                .Select(x => x as CommandExecuteModuleBase);
            _CommandExecuteModules = EnumerateCommandExecuteModules.ToArray();

            var EnumerateMenuExtentionModules = EnumerateValidPlugins
                .SelectMany(x => x.PluginModules.Where(y => y.GetType().IsSubclassOf(typeof(MenuExtentionModuleBase))))
                .Select(x => x as MenuExtentionModuleBase);
            _MenuExtentionModules = EnumerateMenuExtentionModules.ToArray();

        }
        protected override void PostUnloadPlugins()
        {

        }
    }
}
