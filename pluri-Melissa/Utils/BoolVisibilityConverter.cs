using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Project.Utils
{
    public class BoolVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) =>
            (bool)value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) =>
            (System.Windows.Visibility)value == System.Windows.Visibility.Visible;
    }
}
