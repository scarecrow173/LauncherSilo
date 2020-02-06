using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LauncherSilo.PluginSystem
{
    public abstract class PluginConfig
    {
        public abstract string Name { get; set; }

    }
}
