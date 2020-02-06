using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;


namespace LauncherSilo.Converter
{
    [ValueConversion(typeof(string), typeof(System.Windows.Forms.Keys))]
    public class StringKeysConverter : IValueConverter
    {
        private System.Windows.Forms.KeysConverter converter = new System.Windows.Forms.KeysConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return converter.ConvertFromString((string)value);
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("no implement");
        }
    }
}
