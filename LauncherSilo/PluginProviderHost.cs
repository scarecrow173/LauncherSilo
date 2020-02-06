using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Misc;

using LauncherSilo.PluginSystem;

namespace LauncherSilo
{
    public class PluginLogger : IPluginLogger
    {
        public void Trace(string message)
        {
            LogStatics.Debug(message);
        }
        public void Trace(string message, params object[] args)
        {
            LogStatics.Debug(message, args);
        }

        public void Debug(string message)
        {
            LogStatics.Debug(message);
        }
        public void Debug(string message, params object[] args)
        {
            LogStatics.Debug(message, args);
        }

        public void Info(string message)
        {
            LogStatics.Info(message);
        }
        public void Info(string message, params object[] args)
        {
            LogStatics.Info(message, args);
        }

        public void Warn(string message)
        {
            LogStatics.Warn(message);
        }
        public void Warn(string message, params object[] args)
        {
            LogStatics.Warn(message, args);
        }

        public void Error(string message)
        {
            LogStatics.Error(message);
        }
        public void Error(Exception exception, string message)
        {
            LogStatics.Error(exception, message);
        }
        public void Error(string message, params object[] args)
        {
            LogStatics.Error(message, args);
        }
        public void Error(Exception exception, string message, params object[] args)
        {
            LogStatics.Error(exception, message, args);
        }

        public void Fatal(string message)
        {
            LogStatics.Fatal(message);
        }
        public void Fatal(Exception exception, string message)
        {
            LogStatics.Fatal(exception, message);
        }
        public void Fatal(string message, params object[] args)
        {
            LogStatics.Fatal(message, args);
        }
        public void Fatal(Exception exception, string message, params object[] args)
        {
            LogStatics.Fatal(exception, message, args);
        }

    }

    public class PluginProviderHost : IPluginHost
    {
        public event EventHandler<PluginStartupEventArgs> OnStartupEvent;
        public event EventHandler<PluginShutdownEventArgs> OnShutdownEvent;
        public event EventHandler<PluginConfigChengedEventArgs> OnConfigChenged;


        public IPluginLogger Logger { get { return _Logger; } }
        private PluginLogger _Logger = new PluginLogger();

        public Dictionary<string, PluginConfig> PluginConfigs { get { return _PluginConfigs; } }
        private Dictionary<string, PluginConfig> _PluginConfigs = new Dictionary<string, PluginConfig>();

        public void RegisterPluginConfig<T>(string ConfigName) where T : PluginConfig
        {
            Type ConfigType = typeof(T);
            PluginConfig Obj = ConfigType.Assembly.CreateInstance(ConfigType.FullName) as PluginConfig;
            PluginConfigs.Add(ConfigName, Obj);
        }
        public void UnregisterPluginConfig(string ConfigName)
        {

        }
        public T GetConfigObject<T>(string ConfigName) where T : PluginConfig
        {
            if (PluginConfigs.ContainsKey(ConfigName))
            {
                return PluginConfigs[ConfigName] as T;
            }
            else 
            {
                return null;
            }
        }

        public void ApplyConfigObject(PluginConfig Config)
        {
            if (_PluginConfigs.ContainsKey(Config.Name))
            {
                _PluginConfigs[Config.Name] = Config;
                RaiseConfigChenged(Config);
            }
        }

        public void RaiseStartupEvent()
        {
            OnStartupEvent?.Invoke(this, new PluginStartupEventArgs());
        }
        public void RaiseShutdownEvent()
        {
            OnShutdownEvent?.Invoke(this, new PluginShutdownEventArgs());
        }
        public void RaiseConfigChenged(PluginConfig Config)
        {
            OnConfigChenged?.Invoke(this, new PluginConfigChengedEventArgs(Config));
        }
        public Type[] GetPluginConfigTypes()
        {
            return _PluginConfigs.Values.Select(x => x.GetType()).ToArray();
        }
    }
}
