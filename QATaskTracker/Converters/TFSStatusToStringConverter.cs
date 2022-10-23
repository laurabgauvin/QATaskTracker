using QATaskTracker.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace QATaskTracker.Converters
{
    /// <summary>
    /// Convert TFSStatus enum to formatted string
    /// </summary>
    public class TFSStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tfsStatus = (TFSStatus)value;
            return GetValue(tfsStatus);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tfsStatus = (string)value;

            switch (tfsStatus)
            {
                case "Not Ready":
                    return TFSStatus.NotReady;
                case "Ready":
                    return TFSStatus.Ready;
                case "Done":
                    return TFSStatus.Done;
                case "In Progress":
                    return TFSStatus.InProgress;
                case "Waiting":
                    return TFSStatus.Waiting;
                default:
                    return TFSStatus.Ready;
            }
        }

        public static string[] Values => GetValues();

        public static string GetValue(TFSStatus tfsStatus)
        {
            switch (tfsStatus)
            {
                case TFSStatus.NotReady:
                    return "Not Ready";
                case TFSStatus.Ready:
                    return "Ready";
                case TFSStatus.InProgress:
                    return "In Progress";
                case TFSStatus.Waiting:
                    return "Waiting";
                case TFSStatus.Done:
                    return "Done";
                default:
                    return "";
            }
        }

        public static string[] GetValues()
        {
            List<string> list = new()
            {
                GetValue(TFSStatus.NotReady),
                GetValue(TFSStatus.Ready),
                GetValue(TFSStatus.InProgress),
                GetValue(TFSStatus.Waiting),
                GetValue(TFSStatus.Done)
            };

            return list.ToArray();
        }
    }
}
