using QATaskTracker.Helpers;
using QATaskTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace QATaskTracker.ViewModels
{
    public class AddWindowViewModel
    {
        #region Private Fields

        private readonly TFSTask originalTask;
        private readonly AppParameters appParameters;
        private readonly ObservableCollection<TFSTask> displayedTasks;

        #endregion

        #region Properties
        
        public bool IsNewTask { get; set; }
        public List<TaskVersionDetail> AddScreenTaskVersionDetails { get; set; }
        public string TFS { get; set; }
        public Project Project { get; set; }
        public TFSType Type { get; set; }
        public TFSStatus Status { get; set; }
        public string Priority { get; set; }
        public string Iteration { get; set; }
        public string Ticket { get; set; }
        public string UserStory { get; set; }
        public string DevTask { get; set; }
        public string Developer { get; set; }
        public string Customer { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }

        /// <summary>
        /// Used in MainWindow to return the task that was just added
        /// </summary>
        public TFSTask FullTask { get; set; }

        #endregion

        #region Constructors

        public AddWindowViewModel() { }

        /// <summary>
        /// Constructor called when adding a new task
        /// </summary>
        /// <param name="parameters">Parameters</param>
        /// <param name="allTasks">Lists of all tasks</param>
        public AddWindowViewModel(AppParameters parameters, ObservableCollection<TFSTask> allTasks)
        {
            appParameters = parameters;
            displayedTasks = allTasks;
            Initialize();
        }

        /// <summary>
        /// Constructor called when modifying an existing task
        /// </summary>
        /// <param name="parameters">Parameters</param>
        /// <param name="task">Task to load</param>
        /// <param name="allTasks">List of all tasks</param>
        public AddWindowViewModel(AppParameters parameters, TFSTask task, ObservableCollection<TFSTask> allTasks)
        {
            appParameters = parameters;
            displayedTasks = allTasks;
            originalTask = task;
            FullTask = task;
            Initialize(task);
        }

        /// <summary>
        /// Initializes a blank Add form
        /// </summary>
        private void Initialize()
        {
            TFS = string.Empty;
            Project = appParameters.DefaultProject;
            Type = TFSType.Task;
            Status = TFSStatus.NotReady;
            Priority = string.Empty;
            Ticket = string.Empty;
            UserStory = string.Empty;
            DevTask = string.Empty;
            Developer = string.Empty;
            Customer = string.Empty;
            Description = string.Empty;
            Notes = string.Empty;
            IsNewTask = true;

            if (Project == Project.ProjectGreen)
            {
                LoadProjectGreenDetails();
                Iteration = appParameters.PGIteration;
            }
            else
            {
                LoadProjectMagentaDetails();
                Iteration = appParameters.PMIteration;
            }
        }

        /// <summary>
        /// Initializes Modify form
        /// </summary>
        /// <param name="task">Task to load</param>
        private void Initialize(TFSTask task)
        {
            TFS = task.TFS;
            Project = task.Project;
            Type = task.Type;
            Status = task.Status;
            Priority = task.Priority;
            Iteration = task.Iteration;
            Ticket = task.Ticket;
            UserStory = task.UserStory;
            DevTask = task.DevTask;
            Developer = task.Developer;
            Customer = task.Customer;
            Description = task.Description;
            Notes = task.Notes;
            IsNewTask = false;

            if (Project == Project.ProjectGreen)
                LoadProjectGreenDetails();
            else
                LoadProjectMagentaDetails();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads all of the Project Green versions in the ItemsControl
        /// </summary>
        public void LoadProjectGreenDetails()
        {
            AddScreenTaskVersionDetails = new List<TaskVersionDetail>();

            // Generate a list containing all the versions with blank parameters
            foreach (string version in appParameters.PGVersions)
            {
                TaskVersionDetail versionDetail = new("", Project.ProjectGreen, version, TFSStatus.NotReady);
                versionDetail.TFSUpdated.Status = StepStatus.Required;
                versionDetail.TicketUpdated.Status = StepStatus.Required;

                // Beta version
                if (version == appParameters.PGBetaVersion)
                {
                    versionDetail.TestCase.Status = StepStatus.Required;
                    versionDetail.ReleaseNotes.Status = StepStatus.Required;
                    versionDetail.UserGuide.Status = StepStatus.Required;
                }

                // Dev version
                if (version == appParameters.PGDevVersion)
                {
                    versionDetail.AutomatedUITest.Status = StepStatus.Required;
                }

                // Released versions
                if (version != appParameters.PGBetaVersion && version != appParameters.PGDevVersion)
                {
                    versionDetail.Patched.Status = StepStatus.Required;
                }

                AddScreenTaskVersionDetails.Add(versionDetail);
            }

            // For modified tasks, use the existing values
            if (!IsNewTask)
            {
                foreach (TaskVersionDetail versionDetail in originalTask.TaskVersionDetails)
                {
                    // Remove existing records (should be only one) for the version
                    AddScreenTaskVersionDetails.RemoveAll(x => x.Version == versionDetail.Version);

                    // Add the data from the modified task
                    AddScreenTaskVersionDetails.Add(versionDetail);
                }
            }

            var versionOrder = appParameters.PGVersions;
            AddScreenTaskVersionDetails = AddScreenTaskVersionDetails.OrderBy(x => versionOrder.IndexOf(x.Version)).ToList();
        }

        /// <summary>
        /// Loads all of the Project Magenta versions in the ItemsControl
        /// </summary>
        public void LoadProjectMagentaDetails()
        {
            AddScreenTaskVersionDetails = new List<TaskVersionDetail>();

            // Generate a list containing all the versions with blank parameters
            foreach (string version in appParameters.PMVersions)
            {
                TaskVersionDetail versionDetail = new("", Project.ProjectMagenta, version, TFSStatus.NotReady);
                versionDetail.TFSUpdated.Status = StepStatus.Required;
                versionDetail.TicketUpdated.Status = StepStatus.Required;

                // Beta version
                if (version == appParameters.PMBetaVersion)
                {
                    versionDetail.TestCase.Status = StepStatus.Required;
                    versionDetail.ReleaseNotes.Status = StepStatus.Required;
                    versionDetail.UserGuide.Status = StepStatus.Required;
                }

                // Dev version
                if (version == appParameters.PMDevVersion)
                {
                    versionDetail.AutomatedUITest.Status = StepStatus.Required;
                }

                // Released versions
                if (version != appParameters.PMBetaVersion && version != appParameters.PMDevVersion)
                {
                    versionDetail.Patched.Status = StepStatus.Required;
                }

                AddScreenTaskVersionDetails.Add(versionDetail);
            }

            // For modified tasks, use the existing values
            if (!IsNewTask)
            {
                foreach (TaskVersionDetail versionDetail in originalTask.TaskVersionDetails)
                {
                    // Remove existing records (should be only one) for the version
                    AddScreenTaskVersionDetails.RemoveAll(x => x.Version == versionDetail.Version);

                    // Add the data from the modified task
                    AddScreenTaskVersionDetails.Add(versionDetail);
                }
            }

            var versionOrder = appParameters.PMVersions;
            AddScreenTaskVersionDetails = AddScreenTaskVersionDetails.OrderBy(x => versionOrder.IndexOf(x.Version)).ToList();
        }

        /// <summary>
        /// Create a new task
        /// </summary>
        public void AddNewTask()
        {
            // Create new task
            FullTask = new TFSTask(TFS, Project, Type, Status, Priority, Iteration, Ticket, UserStory, DevTask, Developer,
                Customer, Description, Notes, DateTime.Now, DateTime.MinValue);

            // Add VersionDetails that have Tested checked
            ObservableCollection<TaskVersionDetail> newVersionDetails = new();
            foreach (TaskVersionDetail uiVersionDetail in AddScreenTaskVersionDetails)
            {
                if (uiVersionDetail.Tested.Status != StepStatus.NotRequired)
                {
                    TaskVersionDetail temp = new(TFS, Project, uiVersionDetail.Version)
                    {
                        Notes = uiVersionDetail.Notes,
                        Status = uiVersionDetail.Status,
                        Tested = new StepInfo(uiVersionDetail.Tested.Name, uiVersionDetail.Tested.Status),
                        Patched = new StepInfo(uiVersionDetail.Patched.Name, uiVersionDetail.Patched.Status),
                        EmailSent = new StepInfo(uiVersionDetail.EmailSent.Name, uiVersionDetail.EmailSent.Status),
                        TestCase = new StepInfo(uiVersionDetail.TestCase.Name, uiVersionDetail.TestCase.Status),
                        ReleaseTasks = new StepInfo(uiVersionDetail.ReleaseTasks.Name, uiVersionDetail.ReleaseTasks.Status),
                        ReleaseNotes = new StepInfo(uiVersionDetail.ReleaseNotes.Name, uiVersionDetail.ReleaseNotes.Status),
                        UserGuide = new StepInfo(uiVersionDetail.UserGuide.Name, uiVersionDetail.UserGuide.Status),
                        ParametersList = new StepInfo(uiVersionDetail.ParametersList.Name, uiVersionDetail.ParametersList.Status),
                        SampleReport = new StepInfo(uiVersionDetail.SampleReport.Name, uiVersionDetail.SampleReport.Status),
                        InternalDocs = new StepInfo(uiVersionDetail.InternalDocs.Name, uiVersionDetail.InternalDocs.Status),
                        TFSUpdated = new StepInfo(uiVersionDetail.TFSUpdated.Name, uiVersionDetail.TFSUpdated.Status),
                        TicketUpdated = new StepInfo(uiVersionDetail.TicketUpdated.Name, uiVersionDetail.TicketUpdated.Status),
                        AutomatedUITest = new StepInfo(uiVersionDetail.AutomatedUITest.Name, uiVersionDetail.AutomatedUITest.Status),
                        InstallInstructions = new StepInfo(uiVersionDetail.InstallInstructions.Name, uiVersionDetail.InstallInstructions.Status)
                    };

                    newVersionDetails.Add(temp);
                }
            }

            FullTask.TaskVersionDetails = new(newVersionDetails);
            FullTask.Reset();

            // Commit changes to the database
            displayedTasks.Add(FullTask);
            using (DatabaseManager databaseManager = new())
            {
                databaseManager.CreateTask(FullTask);
            }
        }

        /// <summary>
        /// Save changes to existing task
        /// </summary>
        public void SaveTaskChanges()
        {
            // Close task if Status changed to Done
            if (originalTask.Status != TFSStatus.Done && Status == TFSStatus.Done)
                originalTask.CloseTask();

            // Save new Task info
            originalTask.Type = Type;
            originalTask.Status = Status;
            originalTask.Priority = Priority;
            originalTask.Iteration = Iteration;
            originalTask.Ticket = Ticket;
            originalTask.UserStory = UserStory;
            originalTask.DevTask = DevTask;
            originalTask.Developer = Developer;
            originalTask.Customer = Customer;
            originalTask.Description = Description;
            originalTask.Notes = Notes;

            // Delete version details that are no longer active
            List<TaskVersionDetail> deletedVersionDetails = new();
            foreach (TaskVersionDetail versionDetail in originalTask.TaskVersionDetails)
            {
                if (AddScreenTaskVersionDetails.Where(x => x.Version == versionDetail.Version).First().Tested.Status == StepStatus.NotRequired)
                    deletedVersionDetails.Add(versionDetail);
            }

            // Add new version details
            foreach (TaskVersionDetail uiVersionDetail in AddScreenTaskVersionDetails)
            {
                // If this version doesn't exist, then add it
                if (uiVersionDetail.Tested.Status != StepStatus.NotRequired &&
                    originalTask.TaskVersionDetails.Where(x => x.Project == Project && x.Version == uiVersionDetail.Version).ToList().Count == 0)
                {
                    TaskVersionDetail temp = new TaskVersionDetail(TFS, Project, uiVersionDetail.Version)
                    {
                        Notes = uiVersionDetail.Notes,
                        Status = uiVersionDetail.Status,
                        Tested = new StepInfo(uiVersionDetail.Tested.Name, uiVersionDetail.Tested.Status),
                        Patched = new StepInfo(uiVersionDetail.Patched.Name, uiVersionDetail.Patched.Status),
                        EmailSent = new StepInfo(uiVersionDetail.EmailSent.Name, uiVersionDetail.EmailSent.Status),
                        TestCase = new StepInfo(uiVersionDetail.TestCase.Name, uiVersionDetail.TestCase.Status),
                        ReleaseTasks = new StepInfo(uiVersionDetail.ReleaseTasks.Name, uiVersionDetail.ReleaseTasks.Status),
                        ReleaseNotes = new StepInfo(uiVersionDetail.ReleaseNotes.Name, uiVersionDetail.ReleaseNotes.Status),
                        UserGuide = new StepInfo(uiVersionDetail.UserGuide.Name, uiVersionDetail.UserGuide.Status),
                        ParametersList = new StepInfo(uiVersionDetail.ParametersList.Name, uiVersionDetail.ParametersList.Status),
                        SampleReport = new StepInfo(uiVersionDetail.SampleReport.Name, uiVersionDetail.SampleReport.Status),
                        InternalDocs = new StepInfo(uiVersionDetail.InternalDocs.Name, uiVersionDetail.InternalDocs.Status),
                        TFSUpdated = new StepInfo(uiVersionDetail.TFSUpdated.Name, uiVersionDetail.TFSUpdated.Status),
                        TicketUpdated = new StepInfo(uiVersionDetail.TicketUpdated.Name, uiVersionDetail.TicketUpdated.Status),
                        AutomatedUITest = new StepInfo(uiVersionDetail.AutomatedUITest.Name, uiVersionDetail.AutomatedUITest.Status),
                        InstallInstructions = new StepInfo(uiVersionDetail.InstallInstructions.Name, uiVersionDetail.InstallInstructions.Status)
                    };

                    originalTask.TaskVersionDetails.Add(temp);
                }
            }

            // Commit changes to the database
            using (DatabaseManager databaseManager = new())
            {
                foreach (TaskVersionDetail versionDetail in deletedVersionDetails)
                {
                    originalTask.TaskVersionDetails.Remove(versionDetail);
                    databaseManager.DeleteVersionDetail(versionDetail);
                }
                databaseManager.UpdateTask(originalTask);
            }
        }

        #endregion
    }
}
