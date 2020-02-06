using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LauncherSilo.PluginSystem;


namespace LauncherSilo.Core
{
    public abstract class LauncherSiloModuleBase : IPluginModule
    {
        public abstract string Name { get; }
        public abstract string Description { get; }

        public abstract IPlugin Owner { get; }

        public abstract bool Initialize(IPlugin OwnerPlugin);
        public abstract bool Finalize();
    }
}
