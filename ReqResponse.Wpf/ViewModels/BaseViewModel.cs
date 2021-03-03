
using System;
using System.ComponentModel;
using static ReqResponse.Wpf.Models.Constants;

namespace ReqResponse.Wpf.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _titleMessage;
        public string TitleMessage
        {
            get
            {
                return _titleMessage;
            }
            set
            {
                _titleMessage = value;
                OnPropertyChanged("TitleMessage");
            }
        }

        public virtual void SetTitleMessage()
        { 
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static ViewType GetViewTypeFromString(string str)
        {
            ViewType result = (ViewType)Enum.Parse(typeof(ViewType), str, true);
            return result;
        }
    }
}