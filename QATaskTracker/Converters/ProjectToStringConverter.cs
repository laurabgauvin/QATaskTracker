using QATaskTracker.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace QATaskTracker.Converters
{
    /// <summary>
    /// Converts Project enum to formatted string
    /// </summary>
    public class ProjectToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var project = (Project)value;

            return GetValue(project);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var project = value.ToString();

            if (project == "Project Green")
                return Project.ProjectGreen;
            else if (project == "Project Magenta")
                return Project.ProjectMagenta;

            return Project.ProjectGreen;
        }

        public static string[] Values => GetValues();

        public static string GetValue(Project project)
        {
            if (project == Project.ProjectGreen)
                return "Project Green";
            else if (project == Project.ProjectMagenta)
                return "Project Magenta";

            return "";
        }

        public static string[] GetValues()
        {
            List<string> list = new();
            foreach (Project project in Enum.GetValues(typeof(Project)))
            {
                list.Add(GetValue(project));
            }

            return list.ToArray();
        }
    }
}
