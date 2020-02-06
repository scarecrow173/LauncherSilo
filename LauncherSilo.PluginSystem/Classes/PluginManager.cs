using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace LauncherSilo.PluginSystem
{
    public class PluginManager
    {
        private string _PluginSearchPath = string.Empty;
        private readonly char _SearchPathSeparator = ';';
        protected Dictionary<string, IPlugin> _Plugins = new Dictionary<string, IPlugin>();
        protected IPluginHost _Host = null;

        public PluginManager(IPluginHost Host)
        {
            _Host = Host;
        }

        public IPlugin FindPlugin(string Name)
        {
            if (_Plugins.ContainsKey(Name))
            {
                return _Plugins[Name];
            }
            else
            {
                _Host?.Logger?.Info("Plugin not found! : {0}", Name);
                return null;
            }
        }
        public string[] GetPluginNames()
        {
            return _Plugins.Keys.ToArray();
        }
        public void RegisterPluginSearchPath(string Path)
        {
            _Host?.Logger?.Debug("Registed dll search path : {0}", Path);
            _PluginSearchPath += Path + _SearchPathSeparator;
        }
        public void UnregisterPluginSearchPath(string Path)
        {
            _Host?.Logger?.Debug("Unregisted dll Search Path : {0}", Path);
            _PluginSearchPath = _PluginSearchPath.Replace(Path + _SearchPathSeparator, string.Empty);
        }
        public void LoadAllPlugins()
        {
            PreLoadPlugins();
            AssemblyMetadata<IPlugin>[] PluginMetadatas = SearchPlugins(); 
            foreach (var Metadata in PluginMetadatas)
            {
                var NewInstance = Metadata.CreateInstance(_Host?.Logger);
                if (_Plugins.ContainsKey(NewInstance.Name)) // 重複不可
                {
                    _Host?.Logger?.Warn("plugin duplicated! : {0}", NewInstance.Name);
                    NewInstance.Finalize(); // 即破棄
                    continue;
                }
                if (NewInstance.Initialize(_Host))
                {
                    _Host?.Logger?.Debug("load plugin succeeded : {0}", NewInstance.Name);
                    RegisterPluginEvents(_Host, NewInstance);
                    AssemblyMetadata<IPluginModule>[] ModuleMetadatas = NewInstance.CollectModuleMetadatas();
                    List<IPluginModule> ModuleLists = new List<IPluginModule>();
                    foreach(var Module in ModuleMetadatas)
                    {
                        var ModuleInstance = Module.CreateInstance(_Host?.Logger);
                        if (ModuleInstance.Initialize(NewInstance))
                        {
                            ModuleLists.Add(ModuleInstance);
                        }
                    }
                    NewInstance.PluginModules = ModuleLists.ToArray();
                    _Plugins.Add(NewInstance.Name, NewInstance);
                }
                else
                {
                    _Host?.Logger?.Warn("load plugin failed! : {0}", NewInstance.Name);
                    NewInstance.Finalize();
                }
            }
            PostLoadPlugins();
        }
        public void UnloadAllPlugins()
        {
            PreUnloadPlugins();
            foreach (var Plugin in _Plugins.Values)
            {
                if (Plugin.PluginModules != null)
                {
                    foreach(var Module in Plugin.PluginModules)
                    {
                        if (Module.Finalize())
                        {
                            _Host?.Logger?.Debug("unload plugin module succeeded : {0}", Module.Name);
                        }
                        else
                        {
                            _Host?.Logger?.Warn("unload plugin module failed! : {0}", Module.Name);
                        }
                    }
                }
                if (Plugin.Finalize())
                {
                    _Host?.Logger?.Debug("unload plugin succeeded : {0}", Plugin.Name);
                }
                else
                {
                    _Host?.Logger?.Warn("unload plugin failed! : {0}", Plugin.Name);
                }
            }
            _Plugins.Clear();
            PostUnloadPlugins();
        }
        public virtual void SendEvent(string EventName, object Parameter)
        {
            _Host?.Logger?.Debug("send event : {0}", EventName);
            foreach (var Plugin in _Plugins.Values)
            {
                Plugin.OnEventReceived(EventName, Parameter);
            }
        }
        protected virtual void RegisterPluginEvents(IPluginHost Host, IPlugin Plugin)
        {
            Host.OnStartupEvent += Plugin.Startup;
            Host.OnShutdownEvent += Plugin.Shutdown;
            Host.OnConfigChenged += Plugin.ConfigChenged;
        }
        protected virtual void PreLoadPlugins()
        {

        }
        protected virtual void PostLoadPlugins()
        {

        }
        protected virtual void PreUnloadPlugins()
        {

        }
        protected virtual void PostUnloadPlugins()
        {

        }
        private AssemblyMetadata<IPlugin>[] SearchPlugins()
        {
            string[] SearchPaths = _PluginSearchPath.Split(new char[] { _SearchPathSeparator }, StringSplitOptions.RemoveEmptyEntries);
            return AssemblyMetadata<IPlugin>.SearchMetadata(SearchPaths, _Host?.Logger);
        }

    }
}
