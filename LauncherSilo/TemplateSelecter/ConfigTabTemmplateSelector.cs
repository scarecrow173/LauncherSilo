using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using LauncherSilo.ViewModels;

namespace LauncherSilo.TemplateSelecter
{
    public class ConfigTabTemmplateSelector : DataTemplateSelector
    {
        public DataTemplate ErrorTabTemplate { get; set; }

        public DataTemplate GeneralTabTemplate { get; set; }
        public DataTemplate DisplayTabTemplate { get; set; }
        public DataTemplate RegisterLauncherCommandTabTemplate { get; set; }
        public DataTemplate PluginConfigTabTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ConfigTabViewModel ConfigTaVM = (ConfigTabViewModel)item;
            switch(ConfigTaVM.TabType)
            {
                case EConfigTabType.None:
                    return ErrorTabTemplate;
                case EConfigTabType.General:
                    return GeneralTabTemplate;
                case EConfigTabType.Display:
                    return DisplayTabTemplate;
                case EConfigTabType.RegisterLauncherCommand:
                    return RegisterLauncherCommandTabTemplate;
                case EConfigTabType.Plugin:
                    return PluginConfigTabTemplate;
                default:
                    return ErrorTabTemplate;

            }
        }
    }
}
