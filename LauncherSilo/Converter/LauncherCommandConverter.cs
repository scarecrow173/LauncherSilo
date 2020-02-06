using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

using LauncherSilo.Core.Models;
using LauncherSilo.Models;


namespace LauncherSilo.Converter
{
    [ValueConversion(typeof(string), typeof(LauncherCommandInfo))]
    public class LauncherCommandConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new LauncherCommandInfo
            {
                Name = (string)values[0],
                Command = (string)values[1],
                Description = (string)values[2],
                IsImmediately = (bool)values[3],
                Type = (CommandExecuteType)values[4]
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("no implement");
        }
    }
}
