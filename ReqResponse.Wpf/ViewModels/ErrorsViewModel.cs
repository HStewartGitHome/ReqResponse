using ReqResponse.DataLayer.Models;

namespace ReqResponse.Wpf.ViewModels
{
    public class ErrorsViewModel : BaseViewModel
    {
        public TestErrorReport ErrorReport { get; set; }

        #region ErrorCount property
        private string _errorCount;
        public string ErrorCount
        {
            get
            {
                return _errorCount;
            }
            set
            {
                _errorCount = value;
                OnPropertyChanged("ErrorCount");
            }
        }
        #endregion

        #region ErrorSet property
        private string _errorSet;
        public string ErrorSet
        {
            get
            {
                return _errorSet;
            }
            set
            {
                _errorSet = value;
                OnPropertyChanged("ErrorSet");
            }
        }
        #endregion

        #region CurrentLastErrorDateTime property
        private string _currentLastErrorDateTime;
        public string CurrentLastErrorDateTime
        {
            get
            {
                return _currentLastErrorDateTime;
            }
            set
            {
                _currentLastErrorDateTime = value;
                OnPropertyChanged("CurrentLastErrorDateTime");
            }
        }
        #endregion

        #region LastErrorDateTime property
        private string _lastErrorDateTime;
        public string LastErrorDateTime
        {
            get
            {
                return _lastErrorDateTime;
            }
            set
            {
                _lastErrorDateTime = value;
                OnPropertyChanged("LastErrorDateTime");
            }
        }
        #endregion

        #region Created property
        private string _created;
        public string Created
        {
            get
            {
                return _created;
            }
            set
            {
                _created = value;
                OnPropertyChanged("Created");
            }
        }
        #endregion

       

        public ErrorsViewModel()
        {
            SetTitleMessage(false);
        }

        public void SetErrorReport( TestErrorReport report)
        {
            ErrorReport = report;
            ErrorCount = report.ErrorCount.ToString();
            ErrorSet = report.ErrorSet.ToString();
            CurrentLastErrorDateTime = report.CurrentLastErrorDateTime.ToString();
            LastErrorDateTime = report.LastErrorDateTime.ToString();
            Created = report.Created.ToString();

            SetTitleMessage(true);
        }

        public void SetTitleMessage(bool finish)
        {
            if ( finish == true )
                TitleMessage = "Errors ReqResponse Screen Finish";
            else
                TitleMessage = "Errors ReqResponse Screen";
        }
    }
}