using MaterialDesignThemes.Wpf;
using QATaskTracker.Helpers;
using QATaskTracker.Models;
using QATaskTracker.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;

namespace QATaskTracker.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        // Fields
        private DispatcherTimer timer = new DispatcherTimer();

        // Properties
        public List<TFSTask> AllTasks { get; set; }
        public ObservableCollection<TFSTask> DisplayedTasks { get; set; }
        public TFSTask SelectedTask { get; set; }
        public AppParameters Parameters { get; set; }
        public int ModifiedCount { get; set; }

        // Commands
        public ICommand OpenNewTaskDialog => new CommandImplementation(NewTaskDialog);
        public ICommand OpenParametersDialog => new CommandImplementation(ParametersDialog);

        public MainWindowViewModel()
        {
            using (DatabaseManager dbManager = new DatabaseManager())
            {
                Parameters = dbManager.LoadParameters();
                AllTasks = dbManager.GetAllTasks();
            }
            DisplayedTasks = new ObservableCollection<TFSTask>(AllTasks.Where(x => x.Status != TFSStatus.NotReady && x.Status != TFSStatus.Done));

            ModifiedCount = 0;
            timer.Tick += TimerTick;
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Start();
        }

        private async void NewTaskDialog(object o)
        {
            var view = new AddWindow(Parameters, DisplayedTasks);
            var result = await DialogHost.Show(view, "RootDialog");
            if ((bool)result)
            {
                AllTasks.Add(view.FullTask);
            }
        }

        private async void ParametersDialog(object o)
        {
            var view = new ParametersWindow(Parameters);
            var result = await DialogHost.Show(view, "RootDialog");
        }

        public async void OpenExistingTaskDialog()
        {
            var view = new AddWindow(Parameters, SelectedTask, DisplayedTasks);
            var result = await DialogHost.Show(view, "RootDialog");
        }

        public void SaveTasks()
        {
            using (DatabaseManager databaseManager = new DatabaseManager())
            {
                foreach (TFSTask task in AllTasks.Where(x => x.Modified))
                {
                    databaseManager.UpdateTask(task);
                }
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            int newModifiedCount = AllTasks.Where(x => x.Modified).ToList().Count;
            if (ModifiedCount != newModifiedCount)
            {
                ModifiedCount = AllTasks.Where(x => x.Modified).ToList().Count;
                OnPropertyChanged("ModifiedCount");
            }
        }

        public void Search(string text)
        {
            List<TFSTask> temp = new List<TFSTask>(AllTasks);
            if (text == string.Empty)
            {
                DisplayedTasks.Clear();
                foreach(TFSTask task in temp)
                {
                    if (task.Status != TFSStatus.NotReady && task.Status != TFSStatus.Done)
                        DisplayedTasks.Add(task);
                }
            }
            else
            {
                DisplayedTasks.Clear();
                foreach (TFSTask task in temp)
                {
                    if (task.TFS.Contains(text))
                        DisplayedTasks.Add(task);
                }
            }
        }

        #region INotify interface

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
