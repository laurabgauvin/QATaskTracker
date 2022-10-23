using System.Windows.Media;
using QATaskTracker.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using QATaskTracker.Models;
using System.IO;

namespace QATaskTracker.Database
{
    class SQLiteDatabase : IDatabase
    {
        private readonly SQLiteConnection connection;

        /// <summary>
        /// Constructor
        /// </summary>
        public SQLiteDatabase()
        {
            string directory = @"C:\QATaskTracker";
            if (Directory.Exists(directory) == false)
                Directory.CreateDirectory(directory);

            string fileName = Path.Combine(directory, "QATaskTrackerDB.db");
            connection = new SQLiteConnection($"Data Source={fileName};Version=3;");

            try
            {
                connection.Open();
                CreateDatabase();
            }
            catch (Exception e)
            {
                MessageBox.Show("SQLiteDatabase constructor: Exception when opening database: " + e);
            }
        }

        #region Public Methods

        /// <summary>
        /// Adds a new task to the TaskHeader table.
        /// </summary>
        /// <param name="task">Task to add</param>
        /// <returns></returns>
        public bool AddTask(TFSTask task)
        {
            // If task already exists, modify instead
            if (TaskExists(task.TFS, task.Project))
                return UpdateTask(task);

            // Build query
            string query = "INSERT INTO TaskHeader " +
                "VALUES (" +
                $"'{PrepareString(task.TFS)}', " +
                $"'{task.Project}', " +
                $"'{task.Type}', " +
                $"'{task.Status}', " +
                $"'{PrepareString(task.Priority)}', " +
                $"'{PrepareString(task.Iteration)}', " +
                $"'{PrepareString(task.Ticket)}', " +
                $"'{PrepareString(task.UserStory)}', " +
                $"'{PrepareString(task.DevTask)}', " +
                $"'{PrepareString(task.Developer)}', " +
                $"'{PrepareString(task.Customer)}', " +
                $"'{PrepareString(task.Description)}', " +
                $"'{PrepareString(task.Notes)}', " +
                $"'{task.CreatedDate:yyyyMMddHHmmss}', " +
                $"'{task.ClosedDate:yyyyMMddHHmmss}'" +
                ")";

            // Run query
            SQLiteCommand command = new(query.ToString(), connection);
            int rowsModified = command.ExecuteNonQuery();

            task.Reset(false);
            if (rowsModified == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Adds a new task to the TaskVersionDetail table.
        /// </summary>
        /// <param name="versionDetail">Task version detail to add</param>
        /// <returns></returns>
        public bool AddVersionDetail(TaskVersionDetail versionDetail)
        {
            // If task already exists, modify instead
            if (TaskVersionDetailExists(versionDetail.TFS, versionDetail.Project, versionDetail.Version))
                return UpdateVersionDetail(versionDetail);

            // Build query
            string query = "INSERT INTO TaskVersionDetail " +
                "VALUES (" +
                $"'{PrepareString(versionDetail.TFS)}', " +
                $"'{versionDetail.Project}', " +
                $"'{PrepareString(versionDetail.Version)}', " +
                $"'{PrepareString(versionDetail.Notes)}', " +
                $"'{versionDetail.Status}', " +
                $"'{(int)versionDetail.Tested.Status}', " +
                $"'{(int)versionDetail.Patched.Status}', " +
                $"'{(int)versionDetail.EmailSent.Status}', " +
                $"'{(int)versionDetail.TestCase.Status}', " +
                $"'{(int)versionDetail.ReleaseTasks.Status}', " +
                $"'{(int)versionDetail.ReleaseNotes.Status}', " +
                $"'{(int)versionDetail.UserGuide.Status}', " +
                $"'{(int)versionDetail.ParametersList.Status}', " +
                $"'{(int)versionDetail.SampleReport.Status}', " +
                $"'{(int)versionDetail.InternalDocs.Status}', " +
                $"'{(int)versionDetail.TFSUpdated.Status}', " +
                $"'{(int)versionDetail.TicketUpdated.Status}', " +
                $"'{(int)versionDetail.AutomatedUITest.Status}', " +
                $"'{(int)versionDetail.InstallInstructions.Status}'" +
                ")";

            // Run query
            SQLiteCommand command = new(query.ToString(), connection);
            int rowsModified = command.ExecuteNonQuery();

            versionDetail.Reset();
            if (rowsModified == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Close the database
        /// </summary>
        public void CloseDatabase()
        {
            try
            {
                if (connection != null)
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();

                    connection.Dispose();
                }
            }
            catch (ObjectDisposedException)
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Deletes one row out of the TaskVersionDetail table.
        /// </summary>
        /// <param name="versionDetail">Task version detail to delete</param>
        /// <returns></returns>
        public bool DeleteVersionDetail(TaskVersionDetail versionDetail)
        {
            if (!TaskVersionDetailExists(versionDetail.TFS, versionDetail.Project, versionDetail.Version))
                return true;

            string query = $"DELETE from TaskVersionDetail WHERE " +
                $"TFS = '{PrepareString(versionDetail.TFS)}' AND " +
                $"Project = '{versionDetail.Project}' AND " +
                $"Version = '{PrepareString(versionDetail.Version)}'";

            // Run query
            SQLiteCommand command = new(query.ToString(), connection);
            int rowsModified = command.ExecuteNonQuery();

            versionDetail.Reset();
            if (rowsModified == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets all the tasks in the database
        /// </summary>
        /// <returns></returns>
        public List<TFSTask> GetAllTasks()
        {
            List<TFSTask> tasks = new();
            List<TaskVersionDetail> taskVersionDetails = new();
            DataTable taskHeaderTable = new();
            DataTable taskVersionDetailTable = new();

            // Populate list of TFSTask
            string query = "SELECT * FROM TaskHeader";
            SQLiteCommand command = new(query.ToString(), connection);
            SQLiteDataAdapter dataAdapter = new(command);
            dataAdapter.Fill(taskHeaderTable);

            foreach (DataRow row in taskHeaderTable.Rows)
            {
                object?[] rowData = row.ItemArray;
                tasks.Add(ConvertTaskHeader(rowData));
            }

            // Populate list of TaskVersionDetail
            query = "SELECT * FROM TaskVersionDetail";
            command = new SQLiteCommand(query.ToString(), connection);
            dataAdapter = new SQLiteDataAdapter(command);
            dataAdapter.Fill(taskVersionDetailTable);

            foreach (DataRow row in taskVersionDetailTable.Rows)
            {
                object?[] rowData = row.ItemArray;
                taskVersionDetails.Add(ConvertTaskVersionDetail(rowData));
            }

            // Assign details to correct task
            foreach (TFSTask task in tasks)
            {
                foreach (TaskVersionDetail versionDetail in taskVersionDetails.Where(x => x.TFS == task.TFS))
                {
                    task.TaskVersionDetails.Add(versionDetail);
                }
            }

            tasks.ForEach(x => x.Reset());
            return tasks.OrderBy(x => x.TFS).ToList();
        }

        /// <summary>
        /// Reads all records in the AppParameters table.
        /// </summary>
        /// <returns></returns>
        public AppParameters ReadParameters()
        {
            AppParameters parameters = new();
            DataTable table = new();

            // Run query
            string query = "SELECT * FROM AppParameters";
            SQLiteCommand command = new(query.ToString(), connection);
            SQLiteDataAdapter dataAdapter = new(command);
            dataAdapter.Fill(table);

            // Convert data from each row
            foreach (DataRow row in table.Rows)
            {
                object[] paramsRow = row.ItemArray;
                string paramName = (string)paramsRow[0];
                string paramValue = (string)paramsRow[1];
                PropertyInfo? propertyInfo = typeof(AppParameters).GetProperty(paramName);
                if (propertyInfo == null) continue;

                if (propertyInfo.PropertyType == typeof(string))
                {
                    propertyInfo.SetValue(parameters, paramValue);
                }
                else if (propertyInfo.PropertyType == typeof(Project))
                {
                    Project value = (Project)Enum.Parse(typeof(Project), paramValue);
                    propertyInfo.SetValue(parameters, value);
                }
                else if (propertyInfo.PropertyType == typeof(ObservableCollection<string>))
                {
                    ObservableCollection<string> value = new(paramValue.Split(',').ToList());
                    propertyInfo.SetValue(parameters, value);
                }
                else if (propertyInfo.PropertyType == typeof(Color))
                {
                    Color value = (Color)ColorConverter.ConvertFromString(paramValue);
                    propertyInfo.SetValue(parameters, value);
                }
                else if (propertyInfo.PropertyType == typeof(int))
                {
                    if (int.TryParse(paramValue, out int value))
                        propertyInfo.SetValue(parameters, value);
                }
                else if (propertyInfo.PropertyType == typeof(bool))
                {
                    if (bool.TryParse(paramValue, out bool value))
                        propertyInfo.SetValue(parameters, value);
                }
            }

            return parameters;
        }

        /// <summary>
        /// Updates parameters in the AppParameters table.
        /// </summary>
        /// <param name="parameters">Parameters to update</param>
        /// <returns></returns>
        public bool UpdateParameters(AppParameters parameters)
        {
            if (parameters == null)
                return false;

            bool result = true;
            foreach (PropertyInfo? property in typeof(AppParameters).GetProperties())
            {
                if (property == null) continue;
                string paramName = property.Name;
                string paramValue = property.GetValue(parameters).ToString();
                string dateModified = DateTime.Now.ToString("yyyyMMddHHmmss");

                if (paramName == "PGVersions")
                {
                    paramValue = string.Join(",", parameters.PGVersions);
                }

                if (paramName == "PMVersions")
                {
                    paramValue = string.Join(",", parameters.PMVersions);
                }

                // Compare current value to new value
                string selectQuery = $"SELECT ParamValue from AppParameters where ParamName = '{paramName}'";
                SQLiteCommand selectCommand = new(selectQuery.ToString(), connection);
                SQLiteDataAdapter selectData = new(selectCommand);
                DataTable selectTable = new();
                selectData.Fill(selectTable);

                // If value doesn't exist
                if (selectTable.Rows.Count == 0)
                {
                    string insertQuery = $"INSERT INTO AppParameters " +
                        "VALUES (" +
                        $"'{paramName}', " +
                        $"'{PrepareString(paramValue)}', " +
                        $"'{dateModified}' " +
                        ")";
                    SQLiteCommand insertCommand = new(insertQuery.ToString(), connection);
                    if (insertCommand.ExecuteNonQuery() != 1)
                        result = false;
                }
                // If values are different, update
                else if (paramValue != (string)selectTable.Rows[0].ItemArray[0])
                {
                    string updateQuery = $"UPDATE AppParameters " +
                        $"SET ParamValue = '{PrepareString(paramValue)}', LastChanged = '{dateModified}' " +
                        $"WHERE ParamName = '{paramName}'";
                    SQLiteCommand updateCommand = new(updateQuery.ToString(), connection);
                    if (updateCommand.ExecuteNonQuery() != 1)
                        result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// Updates an existing task in the TaskHeader table.
        /// </summary>
        /// <param name="task">Task to update</param>
        /// <returns></returns>
        public bool UpdateTask(TFSTask task)
        {
            // If task doesn't exist, add instead
            if (!TaskExists(task.TFS, task.Project))
                return AddTask(task);

            if (!task.HeaderModified)
                return true;

            // Build query
            StringBuilder query = new("UPDATE TaskHeader SET ");

            if (task.typeModified)
                query.Append($"Type = '{task.Type}', ");

            if (task.statusModified)
                query.Append($"Status = '{task.Status}', ");

            if (task.priorityModified)
                query.Append($"Priority = '{PrepareString(task.Priority)}', ");

            if (task.iterationModified)
                query.Append($"Iteration = '{PrepareString(task.Iteration)}', ");

            if (task.ticketModified)
                query.Append($"Ticket = '{PrepareString(task.Ticket)}', ");

            if (task.userStoryModified)
                query.Append($"UserStory = '{PrepareString(task.UserStory)}', ");

            if (task.devTaskModified)
                query.Append($"DevTask = '{PrepareString(task.DevTask)}', ");

            if (task.developerModified)
                query.Append($"Developer = '{PrepareString(task.Developer)}', ");

            if (task.customerModified)
                query.Append($"Customer = '{PrepareString(task.Customer)}', ");

            if (task.descriptionModified)
                query.Append($"Description = '{PrepareString(task.Description)}', ");

            if (task.notesModified)
                query.Append($"Notes = '{PrepareString(task.Notes)}', ");

            if (task.closedDateModified)
                query.Append($"ClosedDate = '{task.ClosedDate.ToString("yyyyMMddHHmmss")}', ");

            query.Remove(query.Length - 2, 2);
            query.Append($" WHERE TFS = '{PrepareString(task.TFS)}' AND Project = '{task.Project}'");

            // Run query
            SQLiteCommand command = new(query.ToString(), connection);
            int rowsModified = command.ExecuteNonQuery();

            task.Reset(false);
            if (rowsModified == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Updates an existing task version detail in the TaskVersionDetail table.
        /// </summary>
        /// <param name="task">Task version detail to update</param>
        /// <returns></returns>
        public bool UpdateVersionDetail(TaskVersionDetail versionDetail)
        {
            // If task version detail doesn't exist, add instead
            if (!TaskVersionDetailExists(versionDetail.TFS, versionDetail.Project, versionDetail.Version))
                return AddVersionDetail(versionDetail);

            if (!versionDetail.Modified)
                return true;

            // Build query
            StringBuilder query = new("UPDATE TaskVersionDetail SET ");

            if (versionDetail.notesModified)
                query.Append($"Notes = '{PrepareString(versionDetail.Notes)}', ");

            if (versionDetail.statusModified)
                query.Append($"Status = '{versionDetail.Status}', ");

            if (versionDetail.StepsModified)
            {
                if (versionDetail.Tested.Modified)
                    query.Append($"Tested = '{(int)versionDetail.Tested.Status}', ");

                if (versionDetail.Patched.Modified)
                    query.Append($"Patched = '{(int)versionDetail.Patched.Status}', ");

                if (versionDetail.EmailSent.Modified)
                    query.Append($"EmailSent = '{(int)versionDetail.EmailSent.Status}', ");

                if (versionDetail.TestCase.Modified)
                    query.Append($"TestCase = '{(int)versionDetail.TestCase.Status}', ");

                if (versionDetail.ReleaseTasks.Modified)
                    query.Append($"ReleaseTasks = '{(int)versionDetail.ReleaseTasks.Status}', ");

                if (versionDetail.ReleaseNotes.Modified)
                    query.Append($"ReleaseNotes = '{(int)versionDetail.ReleaseNotes.Status}', ");

                if (versionDetail.UserGuide.Modified)
                    query.Append($"UserGuide = '{(int)versionDetail.UserGuide.Status}', ");

                if (versionDetail.ParametersList.Modified)
                    query.Append($"ParametersList = '{(int)versionDetail.ParametersList.Status}', ");

                if (versionDetail.SampleReport.Modified)
                    query.Append($"SampleReport = '{(int)versionDetail.SampleReport.Status}', ");

                if (versionDetail.InternalDocs.Modified)
                    query.Append($"InternalDocs = '{(int)versionDetail.InternalDocs.Status}', ");

                if (versionDetail.TFSUpdated.Modified)
                    query.Append($"TFSUpdated = '{(int)versionDetail.TFSUpdated.Status}', ");

                if (versionDetail.TicketUpdated.Modified)
                    query.Append($"TicketUpdated = '{(int)versionDetail.TicketUpdated.Status}', ");

                if (versionDetail.AutomatedUITest.Modified)
                    query.Append($"AutomatedUITest = '{(int)versionDetail.AutomatedUITest.Status}', ");

                if (versionDetail.InstallInstructions.Modified)
                    query.Append($"InstallInstructions = '{(int)versionDetail.InstallInstructions.Status}', ");
            }

            query.Remove(query.Length - 2, 2);
            query.Append($" WHERE TFS = '{PrepareString(versionDetail.TFS)}' AND " +
                $"Project = '{versionDetail.Project}' AND " +
                $"Version = '{PrepareString(versionDetail.Version)}'");

            // Run query
            SQLiteCommand command = new(query.ToString(), connection);
            int rowsModified = command.ExecuteNonQuery();

            versionDetail.Reset();
            if (rowsModified == 1)
                return true;
            else
                return false;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates a blank database.
        /// </summary>
        private void CreateDatabase()
        {
            try
            {
                SQLiteCommand command = new(connection);
                string createTableQuery = @"CREATE TABLE IF NOT EXISTS [TaskHeader] (
                    [TFS] varchar NOT NULL,
                    [Project] varchar,
                    [Type] varchar,
                    [Status] varchar,
                    [Priority] varchar,
                    [Iteration] varchar,
                    [Ticket] varchar,
                    [UserStory] varchar,
                    [DevTask] varchar,
                    [Developer] varchar,
                    [Customer] varchar,
                    [Description] varchar,
                    [Notes] varchar,
                    [CreatedDate] varchar,
                    [ClosedDate] varchar,
                    PRIMARY KEY([TFS],[Project])
                    )";
                command.CommandText = createTableQuery;
                command.ExecuteNonQuery();

                createTableQuery = @"CREATE TABLE IF NOT EXISTS [TaskVersionDetail] (
                    [TFS] varchar NOT NULL,
                    [Project] varchar,
                    [Version] varchar,
                    [Notes] varchar,
                    [Status] varchar,
                    [Tested] varchar,
                    [Patched] varchar,
                    [EmailSent] varchar,
                    [TestCase] varchar,
                    [ReleaseTasks] varchar,
                    [ReleaseNotes] varchar,
                    [UserGuide] varchar,
                    [ParametersList] varchar,
                    [SampleReport] varchar,
                    [InternalDocs] varchar,
                    [TFSUpdated] varchar,
                    [TicketUpdated] varchar,
                    [AutomatedUITest] varchar,
                    [InstallInstructions] varchar,
                    PRIMARY KEY([TFS],[Project],[Version])
                    )";
                command.CommandText = createTableQuery;
                command.ExecuteNonQuery();

                createTableQuery = @"CREATE TABLE IF NOT EXISTS [AppParameters] (
                    [ParamName] varchar NOT NULL PRIMARY KEY,
                    [ParamValue] varchar,
                    [LastChanged] varchar
                    )";
                command.CommandText = createTableQuery;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception("Error occured when creating the SQLite database: " + e);
            }
        }

        /// <summary>
        /// Returns whether the task exists in the TaskHeader table
        /// </summary>
        /// <param name="tfsNumber">TFS Number</param>
        /// <param name="project">Project</param>
        /// <returns></returns>
        private bool TaskExists(string tfsNumber, Project project)
        {
            bool result;
            string query = $"select * from TaskHeader where TFS = '{PrepareString(tfsNumber)}' and Project = '{project}'";
            SQLiteCommand command = new(query, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            result = reader.HasRows;
            return result;
        }

        /// <summary>
        /// Returns whether the task version detail exists in the TaskVersionDetail table
        /// </summary>
        /// <param name="tfsNumber">TFS Number</param>
        /// <param name="project">Project</param>
        /// <param name="version">Version</param>
        /// <returns></returns>
        private bool TaskVersionDetailExists(string tfsNumber, Project project, string version)
        {
            bool result;
            string query = $"select * from TaskVersionDetail where TFS = '{PrepareString(tfsNumber)}' and " +
                $"Project = '{project}' and " +
                $"Version = '{PrepareString(version)}'";
            SQLiteCommand command = new(query, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            result = reader.HasRows;
            return result;
        }

        /// <summary>
        /// Escapes single quote characters in the passed string
        /// </summary>
        /// <param name="str">String to prepare</param>
        /// <returns></returns>
        private static string PrepareString(string str)
        {
            StringBuilder stringBuilder = new();

            foreach (char character in str)
            {
                if (character == '\'')
                    stringBuilder.Append("''");
                else
                    stringBuilder.Append(character);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Converts the data from the TaskHeader table to a Task object.
        /// </summary>
        /// <param name="rowData">The row data from the table.</param>
        /// <returns></returns>
        private static TFSTask ConvertTaskHeader(object[] rowData)
        {
            if (rowData.Length < 15)
                throw new Exception("Error reading TaskHeader.");

            // Save fields
            string tfs = (string)rowData[0];
            Project project = (Project)Enum.Parse(typeof(Project), (string)rowData[1]);
            TFSType type = (TFSType)Enum.Parse(typeof(TFSType), (string)rowData[2]);
            TFSStatus status = (TFSStatus)Enum.Parse(typeof(TFSStatus), (string)rowData[3]);
            string priority = (string)rowData[4];
            string iteration = (string)rowData[5];
            string ticket = (string)rowData[6];
            string userStory = (string)rowData[7];
            string devTask = (string)rowData[8];
            string developer = (string)rowData[9];
            string customer = (string)rowData[10];
            string description = (string)rowData[11];
            string notes = (string)rowData[12];
            DateTime createdDate = DateTime.MinValue;
            if (DateTime.TryParseExact((string)rowData[13], "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                createdDate = dateTime;
            DateTime closeddate = DateTime.MinValue;
            if (DateTime.TryParseExact((string)rowData[14], "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                closeddate = dateTime;

            // Create task
            TFSTask task = new(tfs, project, type, status, priority, iteration, ticket, userStory, devTask, developer,
                customer, description, notes, createdDate, closeddate);
            return task;
        }

        /// <summary>
        /// Converts the data from the TaskVersionDetail table to a TaskVersionDetail object.
        /// </summary>
        /// <param name="rowData">The row data from the table.</param>
        /// <returns></returns>
        private static TaskVersionDetail ConvertTaskVersionDetail(object[] rowData)
        {
            if (rowData.Length < 19)
                throw new Exception("Error reading TaskVersionDetail.");

            // Save fields
            string tfs = (string)rowData[0];
            Project project = (Project)Enum.Parse(typeof(Project), (string)rowData[1]);
            string version = (string)rowData[2];
            string notes = (string)rowData[3];
            TFSStatus status = (TFSStatus)Enum.Parse(typeof(TFSStatus), (string)rowData[4]);

            // Create version detail
            TaskVersionDetail versionDetail = new(tfs, project, version, status)
            {
                Notes = notes
            };
            versionDetail.Tested.Status = (StepStatus)Enum.Parse(typeof(StepStatus), (string)rowData[5]);
            versionDetail.Patched.Status = (StepStatus)Enum.Parse(typeof(StepStatus), (string)rowData[6]);
            versionDetail.EmailSent.Status = (StepStatus)Enum.Parse(typeof(StepStatus), (string)rowData[7]);
            versionDetail.TestCase.Status = (StepStatus)Enum.Parse(typeof(StepStatus), (string)rowData[8]);
            versionDetail.ReleaseTasks.Status = (StepStatus)Enum.Parse(typeof(StepStatus), (string)rowData[9]);
            versionDetail.ReleaseNotes.Status = (StepStatus)Enum.Parse(typeof(StepStatus), (string)rowData[10]);
            versionDetail.UserGuide.Status = (StepStatus)Enum.Parse(typeof(StepStatus), (string)rowData[11]);
            versionDetail.ParametersList.Status = (StepStatus)Enum.Parse(typeof(StepStatus), (string)rowData[12]);
            versionDetail.SampleReport.Status = (StepStatus)Enum.Parse(typeof(StepStatus), (string)rowData[13]);
            versionDetail.InternalDocs.Status = (StepStatus)Enum.Parse(typeof(StepStatus), (string)rowData[14]);
            versionDetail.TFSUpdated.Status = (StepStatus)Enum.Parse(typeof(StepStatus), (string)rowData[15]);
            versionDetail.TicketUpdated.Status = (StepStatus)Enum.Parse(typeof(StepStatus), (string)rowData[16]);
            versionDetail.AutomatedUITest.Status = (StepStatus)Enum.Parse(typeof(StepStatus), (string)rowData[17]);
            versionDetail.InstallInstructions.Status = (StepStatus)Enum.Parse(typeof(StepStatus), (string)rowData[18]);
            versionDetail.Reset();
            return versionDetail;
        }

        #endregion

    }
}
