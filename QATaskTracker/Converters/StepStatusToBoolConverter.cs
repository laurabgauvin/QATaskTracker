using QATaskTracker.Helpers;
using System;
using System.Globalization;
using System.Windows.Data;

namespace QATaskTracker.Converters
{
    /// <summary>
    /// Converts StepStatus enum to boolean.
    /// Done = true.
    /// NotRequired & Required = false.
    /// </summary>
    public class StepStatusToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stepStatus = (StepStatus)value;

            switch (stepStatus)
            {
                case StepStatus.Done:
                    return true;
                case StepStatus.NotRequired:
                case StepStatus.Required:
                default:
                    return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stepStatus = (bool)value;

            if (stepStatus)
                return StepStatus.Done;
            else
                return StepStatus.Required;
        }
    }
}
