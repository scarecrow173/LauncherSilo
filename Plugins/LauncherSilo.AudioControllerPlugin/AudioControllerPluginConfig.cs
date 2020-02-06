using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LauncherSilo.Core;
using LauncherSilo.PluginSystem;

namespace LauncherSilo.AudioControllerPlugin
{
    [Serializable]
    public class AudioControllerPluginConfig : PluginConfig
    {
        public override string Name { get; set; } = "AudioControllerPluginConfig";

    }
}
