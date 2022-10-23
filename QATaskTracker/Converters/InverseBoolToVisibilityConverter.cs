using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace QATaskTracker.Converters
{
    /// <summary>
    /// Converts boolean to visibility.
    /// False = visible.
    /// True = collapsed.
    /// </summary>
    class InverseBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isVisible = !(bool)value;

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
                    return false;
                case Visibility.Hidden:
                    return true;
                case Visibility.Collapsed:
                    return true;
                default:
                    return true;
            }
        }
    }
}
