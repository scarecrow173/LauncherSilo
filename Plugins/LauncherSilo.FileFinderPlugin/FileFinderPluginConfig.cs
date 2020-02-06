using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LauncherSilo.Core;
using LauncherSilo.PluginSystem;

namespace LauncherSilo.FileFinderPlugin
{
    [Serializable]
    public class FileFinderPluginConfig : PluginConfig
    {
        public override string Name { get; set; } = "FileFinderPluginConfig";

        public bool IsEnableFileFinder { get; set; } = false;
    }
}
