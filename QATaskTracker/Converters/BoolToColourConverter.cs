using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace QATaskTracker.Converters
{
    /// <summary>
    /// Converts boolean value to background color. 
    /// Unchecked = light gray background.
    /// Checked = transparent background.
    /// </summary>
    public class BoolToColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value;

            if (isChecked)
                return Brushes.Transparent;
            else
                return Brushes.LightGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
