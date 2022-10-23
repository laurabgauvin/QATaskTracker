using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace QATaskTracker.Converters
{
    /// <summary>
    /// Converts boolean to visibility.
    /// False = collapsed.
    /// True = visible.
    /// </summary>
    class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Collapsed;
            bool isVisible = (bool)value;

            if (isVisible)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility isVisible = (Visibility)value;

            switch (isVisible)
            {
                case Visibility.Visible:
                    return true;
                case Visibility.Hidden:
                    return false;
                case Visibility.Collapsed:
                    return false;
                default:
                    return true;
            }
        }
    }
}
