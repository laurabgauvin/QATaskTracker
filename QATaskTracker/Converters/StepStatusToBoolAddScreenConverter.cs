using QATaskTracker.Helpers;
using System;
using System.Globalization;
using System.Windows.Data;

namespace QATaskTracker.Converters
{
    /// <summary>
    /// Converts StepStatus enum to boolean for the Add screen.
    /// Done & Required = true.
    /// NotRequired = false.
    /// </summary>
    public class StepStatusToBoolAddScreenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stepStatus = (StepStatus)value;

            switch (stepStatus)
            {
                case StepStatus.Done:
                case StepStatus.Required:
                    return true;
                case StepStatus.NotRequired:
                default:
                    return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stepStatus = (bool)value;

            if (stepStatus)
                return StepStatus.Required;
            else
                return StepStatus.NotRequired;
        }
    }
}
