using QATaskTracker.Database;
using System;
using System.Collections.Generic;

namespace QATaskTracker.Models
{
    public class DatabaseManager : IDisposable
    {
        private readonly IDatabase database;

        #region Constructor

        public DatabaseManager()
        {
            database = new SQLiteDatabase();
        }

        public void Dispose()
        {
            database.CloseDatabase();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns all tasks from the database
        /// </summary>
        public List<TFSTask> GetAllTasks()
        {
            List<TFSTask> tasks = new();
            try
            {
                tasks = database.GetAllTasks();
            }
            catch (Exception exception)
            {
                database.CloseDatabase();
                throw new Exception("DatabaseManager GetAllTasks: Exception occurred: ", exception);
            }

            return tasks;
        }

        /// <summary>
        /// Create a new task
        /// </summary>
        /// <param name="task">Task to create</param>
        public bool CreateTask(TFSTask task)
        {
            try
            {
                // Add new task
                if (database.AddTask(task))
                {
                    // Add all version details
                    foreach (TaskVersionDetail versionDetail in task.TaskVersionDetails)
                    {
                        if (!database.AddVersionDetail(versionDetail))
                            return false;
                    }
                }
            }
            catch (Exception exception)
            {
                database.CloseDatabase();
                throw new Exception("DatabaseManager CreateTask: Exception occurred: ", exception);
            }

            return true;
        }

        /// <summary>
        /// Update an existing task
        /// </summary>
        /// <param name="task">Task to update</param>
        public bool UpdateTask(TFSTask task)
        {
            try
            {
                // Modify task
                if (database.UpdateTask(task))
                {
                    // Modify all version details
                    foreach (TaskVersionDetail versionDetail in task.TaskVersionDetails)
                    {
                        if (!database.UpdateVersionDetail(versionDetail))
                            return false;
                    }
                }
            }
            catch (Exception exception)
            {
                database.CloseDatabase();
                throw new Exception("DatabaseManager UpdateTask: Exception occurred: ", exception);
            }

            return true;
        }

        /// <summary>
        /// Remove a TaskVersionDetail for an existing task.
        /// </summary>
        /// <param name="versionDetail">TaskVersionDetail to delete</param>
        public bool DeleteVersionDetail(TaskVersionDetail versionDetail)
        {
            try
            {
                return database.DeleteVersionDetail(versionDetail);
            }
            catch (Exception exception)
            {
                database.CloseDatabase();
                throw new Exception("DatabaseManager DeleteVersionDetail: Exception occurred: ", exception);
            }
        }

        /// <summary>
        /// Reads the AppParameters from the database
        /// </summary>
        public AppParameters LoadParameters()
        {
            AppParameters parameters = new();
            try
            {
                parameters = database.ReadParameters();
            }
            catch (Exception exception)
            {
                database.CloseDatabase();
                throw new Exception("DatabaseManager LoadParameters: Exception occurred: ", exception);
            }

            return parameters;
        }

        /// <summary>
        /// Updates the database with the new AppParameters
        /// </summary>
        /// <param name="parameters">New parameters</param>
        public void UpdateParameters(AppParameters parameters)
        {
            try
            {
                database.UpdateParameters(parameters);
            }
            catch (Exception exception)
            {
                database.CloseDatabase();
                throw new Exception("DatabaseManager UpdateParameters: Exception occurred: ", exception);
            }
        }

        #endregion
    }
}
