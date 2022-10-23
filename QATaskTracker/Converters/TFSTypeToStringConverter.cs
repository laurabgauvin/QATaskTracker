using QATaskTracker.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace QATaskTracker.Converters
{
    /// <summary>
    /// Convert TFSType enum to formatted string
    /// </summary>
    public class TFSTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tfsType = (TFSType)value;
            return GetValue(tfsType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tfsType = (string)value;

            switch (tfsType)
            {
                case "Task":
                    return TFSType.Task;
                case "Bug":
                    return TFSType.Bug;
                case "Issue":
                    return TFSType.Issue;
                default:
                    return TFSType.Task;
            }
        }

        public static string[] Values => GetValues();

        public static string GetValue(TFSType tfsType)
        {
            switch (tfsType)
            {
                case TFSType.Task:
                    return "Task";
                case TFSType.Bug:
                    return "Bug";
                case TFSType.Issue:
                    return "Issue";
                default:
                    return "";
            }
        }

        public static string[] GetValues()
        {
            List<string> list = new();
            foreach (TFSType type in Enum.GetValues(typeof(TFSType)))
            {
                list.Add(GetValue(type));
            }

            return list.ToArray();
        }
    }
}
