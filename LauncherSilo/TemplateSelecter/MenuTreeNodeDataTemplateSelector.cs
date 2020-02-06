using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using LauncherSilo.Core.Models;
using LauncherSilo.Models;
using LauncherSilo.ViewModels;

namespace LauncherSilo.TemplateSelecter
{
    public class MenuTreeNodeDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MenuTemplate { get; set; }
        public DataTemplate CategoryTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            MenuTreeNodeViewModel MenuTreeNodeVM = (MenuTreeNodeViewModel)item;

            switch (MenuTreeNodeVM.Type)
            {
                case MenuType.Menu:
                    return MenuTemplate;

                case MenuType.Category:
                    return CategoryTemplate;
                default:
                    return MenuTemplate;

            }
        }
    }

}
