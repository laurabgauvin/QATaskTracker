using QATaskTracker.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace QATaskTracker.Models
{
    /// <summary>
    /// Class that contains the properties and methods for the task information
    /// </summary>
    public class TFSTask : INotifyPropertyChanged
    {
        #region Fields

        private string _tfs;
        private Project _project;
        private TFSType _type;
        private TFSStatus _status;
        private string _priority;
        private string _iteration;
        private string _ticket;
        private string _userStory;
        private string _devTask;
        private string _developer;
        private string _customer;
        private string _description;
        private string _notes;
        private DateTime _createdDate;
        private DateTime _closedDate;

        public bool typeModified;
        public bool statusModified;
        public bool priorityModified;
        public bool iterationModified;
        public bool ticketModified;
        public bool userStoryModified;
        public bool devTaskModified;
        public bool developerModified;
        public bool customerModified;
        public bool descriptionModified;
        public bool notesModified;
        public bool closedDateModified;

        #endregion

        #region Constructors

        public TFSTask(string tfs, Project project)
        {
            Initialize();
            TFS = tfs;
            Project = project;
        }

        public TFSTask(string tfs, Project project, TFSType type, TFSStatus status, string priority, string iteration, string ticket, string userStory,
            string devTask, string developer, string customer, string description, string notes, DateTime createdDate, DateTime closedDate)
        {
            Initialize();
            TFS = tfs;
            Project = project;
            Type = type;
            Status = status;
            Priority = priority;
            Iteration = iteration;
            Ticket = ticket;
            UserStory = userStory;
            DevTask = devTask;
            Developer = developer;
            Customer = customer;
            Description = description;
            Notes = notes;
            CreatedDate = createdDate;
            ClosedDate = closedDate;
        }

        private void Initialize()
        {
            Type = TFSType.Task;
            Status = TFSStatus.Ready;
            Priority = string.Empty;
            Iteration = string.Empty;
            Ticket = string.Empty;
            UserStory = string.Empty;
            DevTask = string.Empty;
            Developer = string.Empty;
            Customer = string.Empty;
            Description = string.Empty;
            Notes = string.Empty;
            CreatedDate = DateTime.Now;
            ClosedDate = DateTime.MinValue;
            TaskVersionDetails = new ObservableCollection<TaskVersionDetail>();
            Reset();
        }

        public void Reset(bool resetTaskDetail = true)
        {
            typeModified = false;
            statusModified = false;
            priorityModified = false;
            iterationModified = false;
            ticketModified = false;
            userStoryModified = false;
            devTaskModified = false;
            developerModified = false;
            customerModified = false;
            descriptionModified = false;
            notesModified = false;
            closedDateModified = false;

            if (resetTaskDetail)
            {
                foreach (TaskVersionDetail item in TaskVersionDetails)
                {
                    item.Reset();
                }
            }
        }

        #endregion

        #region Properties

        public string TFS
        {
            get => _tfs;
            private set
            {
                _tfs = value;
            }
        }

        public Project Project
        {
            get => _project;
            private set
            {
                _project = value;
            }
        }

        public TFSType Type
        {
            get => _type;
            set
            {
                if (value != _type)
                {
                    typeModified = true;
                    _type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        public TFSStatus Status
        {
            get => _status;
            set
            {
                if (value != _status)
                {
                    statusModified = true;
                    _status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        public string Priority
        {
            get => _priority;
            set
            {
                if (value != _priority)
                {
                    priorityModified = true;
                    _priority = value;
                    OnPropertyChanged("Priority");
                }
            }
        }

        public string Iteration
        {
            get => _iteration;
            set
            {
                if (value != _iteration)
                {
                    iterationModified = true;
                    _iteration = value;
                    OnPropertyChanged("Iteration");
                }
            }
        }

        public string Ticket
        {
            get => _ticket;
            set
            {
                if (value != _ticket)
                {
                    ticketModified = true;
                    _ticket = value;
                    OnPropertyChanged("Ticket");
                }
            }
        }

        public string UserStory
        {
            get => _userStory;
            set
            {
                if (value != _userStory)
                {
                    userStoryModified = true;
                    _userStory = value;
                    OnPropertyChanged("UserStory");
                }
            }
        }

        public string DevTask
        {
            get => _devTask;
            set
            {
                if (value != _devTask)
                {
                    devTaskModified = true;
                    _devTask = value;
                    OnPropertyChanged("DevTask");
                }
            }
        }

        public string Developer
        {
            get => _developer;
            set
            {
                if (value != _developer)
                {
                    developerModified = true;
                    _developer = value;
                    OnPropertyChanged("Developer");
                }
            }
        }

        public string Customer
        {
            get => _customer;
            set
            {
                if (value != _customer)
                {
                    customerModified = true;
                    _customer = value;
                    OnPropertyChanged("Customer");
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    descriptionModified = true;
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        public string Notes
        {
            get => _notes;
            set
            {
                if (value != _notes)
                {
                    notesModified = true;
                    _notes = value;
                    OnPropertyChanged("Notes");
                }
            }
        }

        public DateTime CreatedDate
        {
            get => _createdDate;
            private set => _createdDate = value;
        }

        public DateTime ClosedDate
        {
            get => _closedDate;
            private set
            {
                if (value != _closedDate)
                {
                    closedDateModified = true;
                    _closedDate = value;
                }
            }
        }

        public ObservableCollection<TaskVersionDetail> TaskVersionDetails { get; set; }

        /// <summary>
        /// Returns if either the header or details have been modified
        /// </summary>
        public bool Modified
        {
            get => HeaderModified || DetailsModified;
        }

        /// <summary>
        /// Returns if the task is modified (only fields from TFSTask class)
        /// </summary>
        public bool HeaderModified
        {
            get => typeModified || statusModified || priorityModified ||
                iterationModified || ticketModified || userStoryModified || devTaskModified || customerModified ||
                developerModified || descriptionModified || notesModified || closedDateModified;
        }

        /// <summary>
        /// Returns if any of the TaskVersionDetails for this task are modified (only TaskVersionDetail)
        /// </summary>
        public bool DetailsModified
        {
            get => TaskVersionDetails.Where(x => x.Modified).ToList().Count > 0;
        }

        /// <summary>
        /// Returns the working version for task
        /// </summary>
        public string WorkingVersion
        {
            get
            {
                if (TaskVersionDetails.Count > 0)
                {
                    // The first version that is In Progress
                    var taskList = TaskVersionDetails.Where(x => x.Status == TFSStatus.InProgress).ToList();
                    if (taskList.Count > 0)
                        return taskList.First().Version;

                    // The first version that is Ready
                    taskList = TaskVersionDetails.Where(x => x.Status == TFSStatus.Ready).ToList();
                    if (taskList.Count > 0)
                        return taskList.First().Version;

                    // The first version that is Waiting
                    taskList = TaskVersionDetails.Where(x => x.Status == TFSStatus.Waiting).ToList();
                    if (taskList.Count > 0)
                        return taskList.First().Version;

                    // The first version
                    return TaskVersionDetails.First().Version;
                }

                return "";
            }
        }

        #endregion

        #region Methods

        public void CloseTask()
        {
            ClosedDate = DateTime.Now;
        }

        #endregion

        #region INotify interface

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
