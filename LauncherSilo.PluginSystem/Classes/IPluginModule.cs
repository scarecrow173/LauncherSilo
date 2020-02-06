using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherSilo.PluginSystem
{
    public interface IPluginModule
    {
        string Name { get; }
        string Description { get; }

        bool Initialize(IPlugin OwnerPlugin);
        bool Finalize();
    }
}
