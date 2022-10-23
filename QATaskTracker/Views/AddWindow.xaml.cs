using MaterialDesignThemes.Wpf;
using QATaskTracker.Models;
using QATaskTracker.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QATaskTracker.Views
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : UserControl
    {
        private readonly AddWindowViewModel _viewModel;

        public TFSTask FullTask { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public AddWindow()
        {
            InitializeComponent();
            _viewModel = (AddWindowViewModel)DataContext;
        }

        /// <summary>
        /// When opened from New button on Main form
        /// </summary>
        /// <param name="parameters">Parameters</param>
        /// /// <param name="allTasks">List of all tasks</param>
        public AddWindow(AppParameters parameters, ObservableCollection<TFSTask> allTasks)
        {
            InitializeComponent();
            _viewModel = new AddWindowViewModel(parameters, allTasks);
            DataContext = _viewModel;
        }

        /// <summary>
        /// When opened by double-clicking a task in the grid on Main form
        /// </summary>
        /// <param name="parameters">Parameters</param>
        /// <param name="task">Task to load</param>
        /// <param name="allTasks">List of all tasks</param>
        public AddWindow(AppParameters parameters, TFSTask task, ObservableCollection<TFSTask> allTasks)
        {
            InitializeComponent();
            _viewModel = new AddWindowViewModel(parameters, task, allTasks);
            DataContext = _viewModel;
        }

        /// <summary>
        /// Save changes to existing task.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveTaskChanges();
            FullTask = _viewModel.FullTask;
            DialogHost.CloseDialogCommand.Execute(true, null);
        }

        /// <summary>
        /// Add new task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.TFS == "")
            {
                MessageBox.Show("Please enter a TFS Number.");
            }
            else
            {
                _viewModel.AddNewTask();
                FullTask = _viewModel.FullTask;
                DialogHost.CloseDialogCommand.Execute(true, null);
            }
        }

        /// <summary>
        /// Cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(false, null);
        }

        /// <summary>
        /// Reload version details based on the project selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel != null)
            {
                if (ProjectComboBox.SelectedItem.ToString() == "Project Green")
                    _viewModel.LoadProjectGreenDetails();
                else
                    _viewModel.LoadProjectMagentaDetails();

                // Re-bind ItemsControl to new list
                TaskVersionDetailItemsControl.ItemsSource = _viewModel.AddScreenTaskVersionDetails;
            }
        }

        private void AddForm_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                CancelButton_Click(sender, null);
            }
        }
    }
}
