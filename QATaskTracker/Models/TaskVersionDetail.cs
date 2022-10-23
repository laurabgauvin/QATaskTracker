using QATaskTracker.Helpers;
using System.Collections.Generic;

namespace QATaskTracker.Models
{
    /// <summary>
    /// Class that contains the properties for the task version detail information
    /// </summary>
    public class TaskVersionDetail
    {
        #region Fields

        private string _tfs;
        private Project _project;
        private string _version;
        private string _notes;
        private TFSStatus _status;
        private StepInfo _tested;
        private StepInfo _patched;
        private StepInfo _emailSent;
        private StepInfo _testCase;
        private StepInfo _releaseTasks;
        private StepInfo _releaseNotes;
        private StepInfo _userGuide;
        private StepInfo _parametersList;
        private StepInfo _sampleReport;
        private StepInfo _internalDocs;
        private StepInfo _tfsUpdated;
        private StepInfo _ticketUpdated;
        private StepInfo _automatedUITest;
        private StepInfo _installInstructions;

        public bool notesModified;
        public bool statusModified;

        #endregion

        #region Constructors

        public TaskVersionDetail(string tfs, Project project, string version)
        {
            Initialize();
            TFS = tfs;
            Project = project;
            Version = version;
        }

        public TaskVersionDetail(string tfs, Project project, string version, TFSStatus status)
        {
            Initialize();
            TFS = tfs;
            Project = project;
            Version = version;
            Status = status;
        }

        private void Initialize()
        {
            Notes = string.Empty;
            Status = TFSStatus.NotReady;
            Tested = new StepInfo("Tested");
            Patched = new StepInfo("Patched");
            EmailSent = new StepInfo("Email Sent");
            TestCase = new StepInfo("Test Case");
            ReleaseTasks = new StepInfo("Release Tasks");
            ReleaseNotes = new StepInfo("Release Notes");
            UserGuide = new StepInfo("User Guide");
            ParametersList = new StepInfo("Parameters List");
            SampleReport = new StepInfo("Sample Report");
            InternalDocs = new StepInfo("Internal Docs");
            TFSUpdated = new StepInfo("TFS Updated");
            TicketUpdated = new StepInfo("Ticket Updated");
            AutomatedUITest = new StepInfo("Automated UI Test");
            InstallInstructions = new StepInfo("Install Instructions");
            Reset();
        }

        public void Reset()
        {
            notesModified = false;
            statusModified = false;
            AllSteps.ForEach(x => x.Reset());
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

        public string Version
        {
            get => _version;
            private set
            {
                _version = value;
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
                }
            }
        }

        public StepInfo Tested
        {
            get => _tested;
            set
            {
                if (value != _tested)
                {
                    _tested = value;
                }
            }
        }
        public StepInfo Patched
        {
            get => _patched;
            set
            {
                if (value != _patched)
                {
                    _patched = value;
                }
            }
        }
        public StepInfo EmailSent
        {
            get => _emailSent;
            set
            {
                if (value != _emailSent)
                {
                    _emailSent = value;
                }
            }
        }
        public StepInfo TestCase
        {
            get => _testCase;
            set
            {
                if (value != _testCase)
                {
                    _testCase = value;
                }
            }
        }
        public StepInfo ReleaseTasks
        {
            get => _releaseTasks;
            set
            {
                if (value != _releaseTasks)
                {
                    _releaseTasks = value;
                }
            }
        }
        public StepInfo ReleaseNotes
        {
            get => _releaseNotes;
            set
            {
                if (value != _releaseNotes)
                {
                    _releaseNotes = value;
                }
            }
        }
        public StepInfo UserGuide
        {
            get => _userGuide;
            set
            {
                if (value != _userGuide)
                {
                    _userGuide = value;
                }
            }
        }
        public StepInfo ParametersList
        {
            get => _parametersList;
            set
            {
                if (value != _parametersList)
                {
                    _parametersList = value;
                }
            }
        }
        public StepInfo SampleReport
        {
            get => _sampleReport;
            set
            {
                if (value != _sampleReport)
                {
                    _sampleReport = value;
                }
            }
        }
        public StepInfo InternalDocs
        {
            get => _internalDocs;
            set
            {
                if (value != _internalDocs)
                {
                    _internalDocs = value;
                }
            }
        }
        public StepInfo TFSUpdated
        {
            get => _tfsUpdated;
            set
            {
                if (value != _tfsUpdated)
                {
                    _tfsUpdated = value;
                }
            }
        }
        public StepInfo TicketUpdated
        {
            get => _ticketUpdated;
            set
            {
                if (value != _ticketUpdated)
                {
                    _ticketUpdated = value;
                }
            }
        }

        public StepInfo AutomatedUITest
        {
            get => _automatedUITest;
            set
            {
                if (value != _automatedUITest)
                {
                    _automatedUITest = value;
                }
            }
        }

        public StepInfo InstallInstructions
        {
            get => _installInstructions;
            set
            {
                if (value != _installInstructions)
                {
                    _installInstructions = value;
                }
            }
        }

        /// <summary>
        /// Returns a list of all steps for this version.
        /// </summary>
        public List<StepInfo> AllSteps
        {
            get
            {
                return new()
                {
                    Tested, Patched, EmailSent, TestCase, AutomatedUITest, ReleaseTasks, ReleaseNotes, UserGuide, 
                    ParametersList, SampleReport, InternalDocs, InstallInstructions, TFSUpdated, TicketUpdated
                };
            }
        }


        /// <summary>
        /// Returns whether any of the steps were modified.
        /// </summary>
        public bool StepsModified
        {
            get => AllSteps.FindAll(x => x.Modified).Count > 0;
        }

        /// <summary>
        /// Returns whether this version detail or any of its steps has been modified.
        /// </summary>
        public bool Modified
        {
            get => statusModified || notesModified || StepsModified;
        }

        #endregion
    }
}
