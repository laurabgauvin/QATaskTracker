using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using QATaskTracker.Helpers;
using QATaskTracker.Models;
using QATaskTracker.ViewModels;

namespace QATaskTracker.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = (MainWindowViewModel)DataContext;
        }

        private bool GetSelectedTFS()
        {
            // Get the selected task
            viewModel.SelectedTask = (TFSTask)TaskDataGrid.SelectedItem;

            if (viewModel.SelectedTask != null)
            {
                // Order the version details by the user defined order in the Parameters
                if (viewModel.SelectedTask.TaskVersionDetails != null)
                {
                    ObservableCollection<string> versionOrder;
                    if (viewModel.SelectedTask.Project == Project.ProjectGreen)
                        versionOrder = viewModel.Parameters.PGVersions;
                    else
                        versionOrder = viewModel.Parameters.PMVersions;

                    viewModel.SelectedTask.TaskVersionDetails = new(viewModel.SelectedTask.TaskVersionDetails.OrderBy(x => versionOrder.IndexOf(x.Version)));
                }

                return true;
            }

            return false;
        }

        private void DisplayDetails()
        {
            if (GetSelectedTFS())
                TaskVersionDetailItemsControl.ItemsSource = viewModel.SelectedTask.TaskVersionDetails;
        }

        private void TaskDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayDetails();
        }

        private void TaskDataGrid_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            viewModel.OpenExistingTaskDialog();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SaveTasks();
        }

        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (viewModel.AllTasks.Where(x => x.Modified).ToList().Count > 0)
            {
                ConfirmSavingChanges();
            }

            CloseForm();
        }

        private void ConfirmSavingChanges()
        {
            MessageBoxResult result = MessageBox.Show("Would you like to save your changes?", "QA Task Tracker", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                viewModel.SaveTasks();
        }

        private void CloseForm()
        {
            try
            {
                Close();
                Environment.Exit(1);
            }
            catch (Exception) { }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.AllTasks.Where(x => x.Modified).ToList().Count > 0)
            {
                ConfirmSavingChanges();
            }

            // Go through constructor again so it pulls the list from the DB
            DataContext = new MainWindowViewModel();
            viewModel = (MainWindowViewModel)DataContext;
        }

        private void CheckboxNotReady_Checked(object sender, RoutedEventArgs e)
        {
            foreach (TFSTask task in viewModel.AllTasks.Where(x => x.Status == TFSStatus.NotReady))
            {
                viewModel.DisplayedTasks.Add(task);
            }
        }

        private void CheckboxDone_Checked(object sender, RoutedEventArgs e)
        {
            foreach (TFSTask task in viewModel.AllTasks.Where(x => x.Status == TFSStatus.Done))
            {
                viewModel.DisplayedTasks.Add(task);
            }
        }

        private void CheckboxDone_Unchecked(object sender, RoutedEventArgs e)
        {
            List<TFSTask> temp = new(viewModel.DisplayedTasks.Where(x => x.Status == TFSStatus.Done));
            foreach (TFSTask task in temp)
            {
                viewModel.DisplayedTasks.Remove(task);
            }
        }

        private void CheckboxNotReady_Unchecked(object sender, RoutedEventArgs e)
        {
            List<TFSTask> temp = new(viewModel.DisplayedTasks.Where(x => x.Status == TFSStatus.NotReady));
            foreach (TFSTask task in temp)
            {
                viewModel.DisplayedTasks.Remove(task);
            }
        }

        private void TaskDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TaskDataGrid_DoubleClick(sender, null);
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.Search(SearchTextBox.Text);
        }
    }
}
