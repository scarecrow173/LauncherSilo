using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Reflection;
using System.IO;

using LauncherSilo.Core.Models;
using LauncherSilo.Views;
using LauncherSilo.ViewModels;
using LauncherSilo.Models;
using LauncherSilo.Utility;
using Input;
using Misc;

namespace LauncherSilo
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        public const string MutexName = "LauncherSilo";
        static private Mutex AppMutex = null;


        static public Core.LauncherSiloPluginManager PluginManagerInstance = null;
        static public PluginProviderHost PluginHost = null;


        static public string AppConfigFilePath { get; set; } = "config.xml";
        static public Config AppConfig { get; set; } = null;
        static public Hotkey ShortcutKey { get; set; } = null;

        static public MainViewModel MainVM { get; set; } = new MainViewModel();

        static public MainNotifyIconView NnotifyIcon = null;
        static public ContextOverlay Overlay = null;
        static public ConfigView ConfigMenu = null;

        static public ObservableCollection<SystemMenuTreeNode> SystemMenuNodes { get; set; } = new ObservableCollection<SystemMenuTreeNode>();

        static public LauncherCommandExecuter Executer { get; set; } = new LauncherCommandExecuter();



        public App()
        {
            // 未処理例外の処理
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Initialize();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            StartupValidation();
            base.OnStartup(e);
            PluginHost.RaiseStartupEvent();
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            NnotifyIcon = new MainNotifyIconView();
            ConfigMenu = new ConfigView();
            Overlay = new ContextOverlay();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            PluginHost.RaiseShutdownEvent();
            PluginManagerInstance.UnloadAllPlugins();
            if (AppMutex != null)
            {
                AppMutex.ReleaseMutex();
                AppMutex.Close();
            }
            base.OnExit(e);
            NnotifyIcon.Dispose();
        }
        private void StartupValidation()
        {
            // 多重起動チェック
            bool hasHandle = false;
            AppMutex = new Mutex(true, MutexName, out hasHandle);
            try
            {
                hasHandle = AppMutex.WaitOne(0, false);
            }
            catch (AbandonedMutexException)
            {
                hasHandle = true;
            }
            if (hasHandle == false)
            {
                MessageBox.Show("既に起動しています");
                AppMutex.Close();
                AppMutex = null;
                this.Shutdown();
            }
        }

        static private bool Initialize()
        {
            // 初期化処理系
            InitializePlugins();
            ApplyConfig(LoadConfig(AppConfigFilePath));
            RegisterSystemMenu();
            return true;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogStatics.Fatal(e.ExceptionObject as Exception, "CurrentDomain_UnhandledException");
        }

        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            LogStatics.Fatal(e.Exception, "TaskScheduler_UnobservedTaskException");
        }

        private static void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            LogStatics.Fatal(e.Exception, "App_DispatcherUnhandledException");
        }

        static private bool InitializePlugins()
        {
            PluginHost = new PluginProviderHost();
            PluginManagerInstance = new Core.LauncherSiloPluginManager(PluginHost);

            string DefaultPluginDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "plugins");
            PluginManagerInstance.RegisterPluginSearchPath(DefaultPluginDir);
            PluginManagerInstance.LoadAllPlugins();
            
            return true;
        }

        static public Config LoadConfig(string file_path)
        {
            if (FileHelper.XmlFileHelper.LoadXmlFile<Models.Config>(AppConfigFilePath, out Models.Config ResultConfig, App.PluginHost.GetPluginConfigTypes()))
            {
                return ResultConfig;
            }
            else
            {
                return new Config();
            }
        }
        static public void ApplyConfig(Config config)
        {
            AppConfig = config;
            if (ShortcutKey != null)
            {
                ShortcutKey.Dispose();
            }
            ApplyShortcutKey(config.ShortcutKey.IsUseAltKey, config.ShortcutKey.IsUseShiftKey, config.ShortcutKey.IsUseCtrlKey, config.ShortcutKey.Key);
            ApplyStartupLink(config.IsStartup);
            if (PluginHost != null)
            {
                foreach (var pluginConfig in PluginHost.PluginConfigs.Values)
                {
                    if (!AppConfig.PluginConfigs.Any(x => (x as PluginSystem.PluginConfig).Name == pluginConfig.Name))
                    {
                        AppConfig.PluginConfigs.Add(pluginConfig);
                    }
                }
                foreach (var pluginConfig in AppConfig.PluginConfigs)
                {
                    PluginHost.ApplyConfigObject(pluginConfig as PluginSystem.PluginConfig);
                }
            }

        }
        static public void RegisterSystemMenu()
        {
            foreach (var MenuExtentionModule in PluginManagerInstance.MenuExtentionModules)
            {
                SystemMenuNodes.Add(MenuExtentionModule.GenerateExtentionMenuNode());
            }

            SystemMenuNodes.Add(new SystemMenuTreeNode(new EventHandler(OnConfigMenuExecute))
            {
                Name = "設定",
                IsVisible = true,
                Type = MenuType.System,
                CommandInfoID = string.Empty,
                Children = new ObservableCollection<MenuTreeNode>(),
                IconSource = MaterialDesignIconSourceStorage.FindPackIconImage(MaterialDesignThemes.Wpf.PackIconKind.Settings, new SolidColorBrush(Colors.White), 0)
            });

            SystemMenuNodes.Add(new SystemMenuTreeNode(new EventHandler(OnExitMenuExecute))
            {
                Name = "終了",
                IsVisible = true,
                Type = MenuType.System,
                CommandInfoID = string.Empty,
                Children = new ObservableCollection<MenuTreeNode>(),
                IconSource = MaterialDesignIconSourceStorage.FindPackIconImage(MaterialDesignThemes.Wpf.PackIconKind.ExitToApp, new SolidColorBrush(Colors.White), 0)
            });
        }

        static public void ApplyStartupLink(bool IsStartup)
        {
            string executablePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string exeFileName = System.IO.Path.GetFileName(executablePath);
            string startupDir = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string startupLinkFilePath = System.IO.Path.Combine(startupDir, exeFileName);
            Shortcut StartupLink = new Shortcut(startupLinkFilePath);
            
            if (IsStartup)
            {
                StartupLink.TargetPath = executablePath;
            }
            else
            {
                StartupLink.Delete();

            }
        }
        static public void ApplyShortcutKey(bool IsUseAltKey, bool IsUseShiftKey, bool IsUseCtrlKey, System.Windows.Forms.Keys Key)
        {
            MOD_KEY Modkey = MOD_KEY.MOD_NONE;
            if (IsUseAltKey)
            {
                Modkey |= MOD_KEY.MOD_ALT;
            }
            if (IsUseShiftKey)
            {
                Modkey |= MOD_KEY.MOD_SHIFT;
            }
            if (IsUseCtrlKey)
            {
                Modkey |= MOD_KEY.MOD_CONTROL;
            }
            if (ShortcutKey != null)
            {
                ShortcutKey.Dispose();
            }
            ShortcutKey = new Hotkey(Modkey, Key);
            ShortcutKey.HotkeyPush += ShortcutKey_HotkeyPush;
        }
        static public void ShowContextOverlay()
        {
            Overlay.ShowCurrentDisplayOverlay();
        }
        static public void ShowConfigView()
        {
            ConfigMenu.Show();
        }

        private static void ShortcutKey_HotkeyPush(object sender, EventArgs e)
        {
            System.Drawing.Point dp = System.Windows.Forms.Cursor.Position;
            System.Windows.Point wp = new System.Windows.Point(dp.X, dp.Y);

            switch(AppConfig.Style)
            {
                case LayoutStyles.ContextMenu:
                    NnotifyIcon.ShowMainContextMenu(wp);
                    break;
                case LayoutStyles.TileButton:
                    ShowContextOverlay();
                    break;
                default:
                    break;
            }
        }
        private static void OnExitMenuExecute(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
        private static void OnConfigMenuExecute(object sender, EventArgs e)
        {
            App.ShowConfigView();
        }


    }
}
