using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherSilo.PluginSystem
{
    public class PluginEventArgs : EventArgs
    {
    }
    public class PluginStartupEventArgs : PluginEventArgs
    {
    }
    public class PluginShutdownEventArgs : PluginEventArgs
    {
    }
    public class PluginConfigChengedEventArgs : PluginEventArgs
    {
        public PluginConfig Config { get; set; }

        public PluginConfigChengedEventArgs(PluginConfig InConfig)
        {
            Config = InConfig;
        }
    }
}
