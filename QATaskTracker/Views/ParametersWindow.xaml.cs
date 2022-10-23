using MaterialDesignThemes.Wpf;
using QATaskTracker.Models;
using QATaskTracker.ViewModels;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Media = System.Windows.Media;
using Drawing = System.Drawing;

namespace QATaskTracker.Views
{
    /// <summary>
    /// Interaction logic for ParametersWindow.xaml
    /// </summary>
    public partial class ParametersWindow : System.Windows.Controls.UserControl
    {
        private AppParameters originalParameters;
        private readonly ParametersWindowViewModel _viewModel;

        public ParametersWindow()
        {
            InitializeComponent();
            _viewModel = (ParametersWindowViewModel)DataContext;
        }

        public ParametersWindow(AppParameters parameters)
        {
            InitializeComponent();
            _viewModel = new ParametersWindowViewModel(parameters);
            DataContext = _viewModel;

            // Save original parameters so they can be restored on Cancel
            SaveOriginalParameters(parameters);
        }

        private void SaveOriginalParameters(AppParameters parameters)
        {
            originalParameters = new AppParameters();
            foreach (PropertyInfo propertyInfo in typeof(AppParameters).GetProperties())
            {
                propertyInfo.SetValue(originalParameters, propertyInfo.GetValue(parameters));
            }
            originalParameters.PGVersions = new ObservableCollection<string>();
            originalParameters.PMVersions = new ObservableCollection<string>();
            foreach (string version in parameters.PGVersions)
            {
                originalParameters.PGVersions.Add(version);
            }
            foreach (string version in parameters.PMVersions)
            {
                originalParameters.PMVersions.Add(version);
            }
        }

        private void RestoreOriginalParameters()
        {
            foreach (PropertyInfo propertyInfo in typeof(AppParameters).GetProperties())
            {
                propertyInfo.SetValue(_viewModel.Parameters, propertyInfo.GetValue(originalParameters));
            }
            _viewModel.Parameters.PGVersions = new ObservableCollection<string>();
            _viewModel.Parameters.PMVersions = new ObservableCollection<string>();
            foreach (string version in originalParameters.PGVersions)
            {
                _viewModel.Parameters.PGVersions.Add(version);
            }
            foreach (string version in originalParameters.PMVersions)
            {
                _viewModel.Parameters.PMVersions.Add(version);
            }
        }

        private void PGMoveUpButton_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = PGVersionsList.SelectedIndex;

            if (currentIndex > 0)
            {
                int newIndex = currentIndex - 1;
                _viewModel.Parameters.PGVersions.Move(currentIndex, newIndex);

                PGVersionsList.SelectedIndex = newIndex;
            }
        }

        private void PGMoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = PGVersionsList.SelectedIndex;

            if (currentIndex < PGVersionsList.Items.Count - 1 && currentIndex > -1)
            {
                int newIndex = currentIndex + 1;
                _viewModel.Parameters.PGVersions.Move(currentIndex, newIndex);

                PGVersionsList.SelectedIndex = newIndex;
            }
        }

        private void PGAddVersionButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PGNewVersion.Text))
            {
                _viewModel.Parameters.PGVersions.Add(PGNewVersion.Text);
            }

            PGNewVersion.Clear();
            PGNewVersion.Focus();
        }

        private void PGRemoveVersionButton_Click(object sender, RoutedEventArgs e)
        {
            if (PGVersionsList != null && PGVersionsList.SelectedIndex > -1 && PGVersionsList.Items.Count > 1)
            {
                _viewModel.Parameters.PGVersions.Remove(PGVersionsList.SelectedItem.ToString());
            }
        }

        private void PMMoveUpButton_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = PMVersionsList.SelectedIndex;

            if (currentIndex > 0)
            {
                int newIndex = currentIndex - 1;
                _viewModel.Parameters.PMVersions.Move(currentIndex, newIndex);

                PMVersionsList.SelectedIndex = newIndex;
            }
        }

        private void PMMoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = PMVersionsList.SelectedIndex;

            if (currentIndex < PMVersionsList.Items.Count - 1 && currentIndex > -1)
            {
                int newIndex = currentIndex + 1;
                _viewModel.Parameters.PMVersions.Move(currentIndex, newIndex);

                PMVersionsList.SelectedIndex = newIndex;
            }
        }

        private void PMAddVersionButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PMNewVersion.Text))
            {
                _viewModel.Parameters.PMVersions.Add(PMNewVersion.Text);
            }

            PMNewVersion.Clear();
            PMNewVersion.Focus();
        }

        private void PMRemoveVersionButton_Click(object sender, RoutedEventArgs e)
        {
            if (PMVersionsList != null && PMVersionsList.SelectedIndex > -1 && PMVersionsList.Items.Count > 1)
            {
                _viewModel.Parameters.PMVersions.Remove(PMVersionsList.SelectedItem.ToString());
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Commit changes to the database
            _viewModel.SaveParameters();
            DialogHost.CloseDialogCommand.Execute(true, null);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Restore original parameters
            RestoreOriginalParameters();
            DialogHost.CloseDialogCommand.Execute(false, null);
        }

        /// <summary>
        /// Generates the list of custom colours that will be displayed in the ColorPicker from the current colours
        /// </summary>
        /// <returns></returns>
        private int[] GenerateCustomColours()
        {
            Drawing.Color waitingColour = _viewModel.Parameters.WaitingColour.ToDrawingColor();
            Drawing.Color inProgressColour = _viewModel.Parameters.InProgressColour.ToDrawingColor();
            Drawing.Color readyColour = _viewModel.Parameters.ReadyColour.ToDrawingColor();
            Drawing.Color notReadyColour = _viewModel.Parameters.NotReadyColour.ToDrawingColor();
            Drawing.Color doneColour = _viewModel.Parameters.DoneColour.ToDrawingColor();

            int[] customColours = new int[]
            {
                Drawing.ColorTranslator.ToOle(waitingColour),
                Drawing.ColorTranslator.ToOle(inProgressColour),
                Drawing.ColorTranslator.ToOle(readyColour),
                Drawing.ColorTranslator.ToOle(notReadyColour),
                Drawing.ColorTranslator.ToOle(doneColour)
            };

            return customColours;
        }

        private void WaitingColourButton_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorPicker = new()
            {
                AllowFullOpen = true,
                Color = _viewModel.Parameters.WaitingColour.ToDrawingColor(),
                CustomColors = GenerateCustomColours()
            };

            if (colorPicker.ShowDialog() == DialogResult.OK)
            {
                _viewModel.Parameters.WaitingColour = colorPicker.Color.ToMediaColor();
            }
        }

        private void InProgressColourButton_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorPicker = new()
            {
                AllowFullOpen = true,
                Color = _viewModel.Parameters.InProgressColour.ToDrawingColor(),
                CustomColors = GenerateCustomColours()
            };

            if (colorPicker.ShowDialog() == DialogResult.OK)
            {
                _viewModel.Parameters.InProgressColour = colorPicker.Color.ToMediaColor();
            }
        }

        private void DoneColourButton_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorPicker = new()
            {
                AllowFullOpen = true,
                Color = _viewModel.Parameters.DoneColour.ToDrawingColor(),
                CustomColors = GenerateCustomColours()
            };

            if (colorPicker.ShowDialog() == DialogResult.OK)
            {
                _viewModel.Parameters.DoneColour = colorPicker.Color.ToMediaColor();
            }
        }

        private void ReadyColourButton_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorPicker = new()
            {
                AllowFullOpen = true,
                Color = _viewModel.Parameters.ReadyColour.ToDrawingColor(),
                CustomColors = GenerateCustomColours()
            };

            if (colorPicker.ShowDialog() == DialogResult.OK)
            {
                _viewModel.Parameters.ReadyColour = colorPicker.Color.ToMediaColor();
            }
        }

        private void NotReadyColourButton_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorPicker = new()
            {
                AllowFullOpen = true,
                Color = _viewModel.Parameters.NotReadyColour.ToDrawingColor(),
                CustomColors = GenerateCustomColours()
            };

            if (colorPicker.ShowDialog() == DialogResult.OK)
            {
                _viewModel.Parameters.NotReadyColour = colorPicker.Color.ToMediaColor();
            }
        }

        private void ParamsForm_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                CancelButton_Click(sender, e);
            }
        }
    }

    public static class MediaConverters
    {
        /// <summary>
        /// Converts a System.Windows.Media.Color object to a System.Drawing.Color object
        /// </summary>
        /// <param name="colour"></param>
        /// <returns></returns>
        public static Drawing.Color ToDrawingColor(this Media.Color colour)
        {
            return Drawing.Color.FromArgb(colour.A, colour.R, colour.G, colour.B);
        }

        /// <summary>
        /// Converts a System.Drawing.Color object to a System.Windows.Media.Color object
        /// </summary>
        /// <param name="colour"></param>
        /// <returns></returns>
        public static Media.Color ToMediaColor(this Drawing.Color colour)
        {
            return Media.Color.FromArgb(colour.A, colour.R, colour.G, colour.B);
        }
    }
}
