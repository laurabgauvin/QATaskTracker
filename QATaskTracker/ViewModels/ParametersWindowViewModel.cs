using QATaskTracker.Models;

namespace QATaskTracker.ViewModels
{
    public class ParametersWindowViewModel
    {
        public AppParameters Parameters { get; set; }

        public ParametersWindowViewModel()
        {
            Parameters = new AppParameters();
        }

        public ParametersWindowViewModel(AppParameters parameters)
        {
            Parameters = parameters;
        }

        public void SaveParameters()
        {
            // Commit changes to the database
            using (DatabaseManager databaseManager = new DatabaseManager())
            {
                databaseManager.UpdateParameters(Parameters);
            }
        }
    }
}
