using QATaskTracker.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;

namespace QATaskTracker.Models
{
    /// <summary>
    /// Class that contains the properties for the application parameters
    /// </summary>
    public class AppParameters : INotifyPropertyChanged
    {
        #region Private Fields

        private bool _displayTFS;
        private bool _displayProject;
        private bool _displayType;
        private bool _displayStatus;
        private bool _displayPriority;
        private bool _displayIteration;
        private bool _displayTicket;
        private bool _displayUserStory;
        private bool _displayDevTask;
        private bool _displayDeveloper;
        private bool _displayCustomer;
        private bool _displayDescription;
        private bool _displayNotes;
        private bool _displayCreatedDate;
        private bool _displayClosedDate;
        private bool _displayVersion;

        private Color _waitingColour;
        private Color _inProgressColour;
        private Color _notReadyColour;
        private Color _readyColour;
        private Color _doneColour;

        #endregion

        #region Constructors

        public AppParameters()
        {
            DatabaseVersion = "3.0";
            Initialize();
        }

        public void Initialize()
        {
            DefaultProject = Project.ProjectGreen;
            PGVersions = new ObservableCollection<string>();
            PGDevVersion = "";
            PGBetaVersion = "";
            PGReleasedVersion = "";
            PGIteration = "";

            PMVersions = new ObservableCollection<string>();
            PMDevVersion = "";
            PMBetaVersion = "";
            PMReleasedVersion = "";
            PMIteration = "";

            DisplayTFS = true;
            DisplayProject = true;
            DisplayType = true;
            DisplayStatus = true;
            DisplayPriority = true;
            DisplayIteration = true;
            DisplayTicket = true;
            DisplayUserStory = true;
            DisplayDevTask = true;
            DisplayDeveloper = true;
            DisplayCustomer = true;
            DisplayDescription = true;
            DisplayNotes = true;
            DisplayCreatedDate = true;
            DisplayClosedDate = false;
            DisplayVersion = true;

            WaitingColour = Colors.Transparent;
            InProgressColour = Colors.Transparent;
            DoneColour = Colors.Transparent;
            ReadyColour = Colors.Transparent;
            NotReadyColour = Colors.Transparent;
        }

        #endregion

        #region Properties

        public string DatabaseVersion { get; set; }
        public Project DefaultProject { get; set; }

        public string PGDevVersion { get; set; }
        public string PGReleasedVersion { get; set; }
        public string PGBetaVersion { get; set; }
        public string PGIteration { get; set; }
        public ObservableCollection<string> PGVersions { get; set; }

        public string PMDevVersion { get; set; }
        public string PMReleasedVersion { get; set; }
        public string PMBetaVersion { get; set; }
        public string PMIteration { get; set; }
        public ObservableCollection<string> PMVersions { get; set; }

        public bool DisplayTFS
        {
            get { return _displayTFS; }
            set
            {
                if (_displayTFS != value)
                {
                    _displayTFS = value;
                    OnPropertyChanged("DisplayTFS");
                }
            }
        }

        public bool DisplayProject
        {
            get { return _displayProject; }
            set
            {
                if (_displayProject != value)
                {
                    _displayProject = value;
                    OnPropertyChanged("DisplayProject");
                }
            }
        }

        public bool DisplayType
        {
            get { return _displayType; }
            set
            {
                if (_displayType != value)
                {
                    _displayType = value;
                    OnPropertyChanged("DisplayType");
                }
            }
        }

        public bool DisplayStatus
        {
            get { return _displayStatus; }
            set
            {
                if (_displayStatus != value)
                {
                    _displayStatus = value;
                    OnPropertyChanged("DisplayStatus");
                }
            }
        }

        public bool DisplayPriority
        {
            get { return _displayPriority; }
            set
            {
                if (_displayPriority != value)
                {
                    _displayPriority = value;
                    OnPropertyChanged("DisplayPriority");
                }
            }
        }

        public bool DisplayIteration
        {
            get { return _displayIteration; }
            set
            {
                if (_displayIteration != value)
                {
                    _displayIteration = value;
                    OnPropertyChanged("DisplayIteration");
                }
            }
        }

        public bool DisplayTicket
        {
            get { return _displayTicket; }
            set
            {
                if (_displayTicket != value)
                {
                    _displayTicket = value;
                    OnPropertyChanged("DisplayTicket");
                }
            }
        }

        public bool DisplayUserStory
        {
            get { return _displayUserStory; }
            set
            {
                if (_displayUserStory != value)
                {
                    _displayUserStory = value;
                    OnPropertyChanged("DisplayUserStory");
                }
            }
        }

        public bool DisplayDevTask
        {
            get { return _displayDevTask; }
            set
            {
                if (_displayDevTask != value)
                {
                    _displayDevTask = value;
                    OnPropertyChanged("DisplayDevTask");
                }
            }
        }

        public bool DisplayDeveloper
        {
            get { return _displayDeveloper; }
            set
            {
                if (_displayDeveloper != value)
                {
                    _displayDeveloper = value;
                    OnPropertyChanged("DisplayDeveloper");
                }
            }
        }

        public bool DisplayCustomer
        {
            get { return _displayCustomer; }
            set
            {
                if (_displayCustomer != value)
                {
                    _displayCustomer = value;
                    OnPropertyChanged("DisplayCustomer");
                }
            }
        }

        public bool DisplayDescription
        {
            get { return _displayDescription; }
            set
            {
                if (_displayDescription != value)
                {
                    _displayDescription = value;
                    OnPropertyChanged("DisplayDescription");
                }
            }
        }

        public bool DisplayNotes
        {
            get { return _displayNotes; }
            set
            {
                if (_displayNotes != value)
                {
                    _displayNotes = value;
                    OnPropertyChanged("DisplayNotes");
                }
            }
        }

        public bool DisplayCreatedDate
        {
            get { return _displayCreatedDate; }
            set
            {
                if (_displayCreatedDate != value)
                {
                    _displayCreatedDate = value;
                    OnPropertyChanged("DisplayCreatedDate");
                }
            }
        }

        public bool DisplayClosedDate
        {
            get { return _displayClosedDate; }
            set
            {
                if (_displayClosedDate != value)
                {
                    _displayClosedDate = value;
                    OnPropertyChanged("DisplayClosedDate");
                }
            }
        }

        public bool DisplayVersion
        {
            get { return _displayVersion; }
            set
            {
                if (_displayVersion != value)
                {
                    _displayVersion = value;
                    OnPropertyChanged("DisplayVersion");
                }
            }
        }


        public Color WaitingColour
        {
            get { return _waitingColour; }
            set
            {
                if (_waitingColour != value)
                {
                    _waitingColour = value;
                    OnPropertyChanged("WaitingColour");
                }
            }
        }

        public Color InProgressColour
        {
            get { return _inProgressColour; }
            set
            { if (_inProgressColour != value)
                {
                    _inProgressColour = value;
                    OnPropertyChanged("InProgressColour");
                }
            }
        }
        
        public Color DoneColour
        {
            get { return _doneColour; }
            set
            {
                if (_doneColour != value)
                {
                    _doneColour = value;
                    OnPropertyChanged("DoneColour");
                }
            }
        }

        public Color ReadyColour
        {
            get { return _readyColour; }
            set
            {
                if (_readyColour != value)
                {
                    _readyColour = value;
                    OnPropertyChanged("ReadyColour");
                }
            }
        }

        public Color NotReadyColour
        {
            get { return _notReadyColour; }
            set
            {
                if (_notReadyColour != value)
                {
                    _notReadyColour = value;
                    OnPropertyChanged("NotReadyColour");
                }
            }
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
