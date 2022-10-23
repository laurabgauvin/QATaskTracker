namespace QATaskTracker.Helpers
{
    /// <summary>
    /// Project
    /// </summary>
    public enum Project
    {
        ProjectGreen,
        ProjectMagenta
    }

    /// <summary>
    /// Type of the TFS item
    /// </summary>
    public enum TFSType
    {
        Task,
        Bug,
        Issue
    }

    /// <summary>
    /// Status of the TFS item
    /// </summary>
    public enum TFSStatus
    {
        NotReady,
        Ready,
        InProgress,
        Waiting,
        Done
    }

    /// <summary>
    /// Status of the step
    /// </summary>
    public enum StepStatus
    {
        NotRequired = 0,
        Required = 1,
        Done = 2
    }
}
