using QATaskTracker.Models;
using System.Collections.Generic;

namespace QATaskTracker.Database
{
    public interface IDatabase
    {
        void CloseDatabase();

        // Read Database
        List<TFSTask> GetAllTasks();
        AppParameters ReadParameters();

        // Update database
        bool AddTask(TFSTask task);
        bool UpdateTask(TFSTask task);
        bool AddVersionDetail(TaskVersionDetail versionDetail);
        bool UpdateVersionDetail(TaskVersionDetail versionDetail);
        bool DeleteVersionDetail(TaskVersionDetail versionDetail);
        bool UpdateParameters(AppParameters parameters);
    }
}
