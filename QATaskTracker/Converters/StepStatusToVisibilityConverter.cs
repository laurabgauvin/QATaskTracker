using QATaskTracker.Helpers;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace QATaskTracker.Converters
{
    /// <summary>
    /// Converts StepStatus enum to visibility.
    /// Required & Done = visible.
    /// NotRequired = collapsed.
    /// </summary>
    public class StepStatusToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stepStatus = (StepStatus)value;
            switch (stepStatus)
            {
                case StepStatus.Required:
                case StepStatus.Done:
                    return Visibility.Visible;
                case StepStatus.NotRequired:
                default:
                    return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
