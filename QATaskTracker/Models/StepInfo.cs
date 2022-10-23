using QATaskTracker.Helpers;

namespace QATaskTracker.Models
{
    /// <summary>
    /// Class that contains properties for the steps
    /// </summary>
    public class StepInfo
    {
        #region Private Fields

        private string _name;
        private StepStatus _status;
        private bool _modified;

        #endregion

        #region Constructors

        public StepInfo(string name)
        {
            Name = name;
            Status = StepStatus.NotRequired;
            Reset();
        }

        public StepInfo(string name, StepStatus status)
        {
            Name = name;
            Status = status;
            Reset();
        }

        public void Reset()
        {
            Modified = false;
        }

        #endregion

        #region Properties

        public string Name
        {
            get => _name;
            private set => _name = value;
        }

        public StepStatus Status
        {
            get => _status;
            set
            {
                if (value != _status)
                {
                    _status = value;
                    _modified = true;
                }
            }
        }

        public bool Modified
        {
            get => _modified;
            private set => _modified = value;
        }

        #endregion
    }
}
