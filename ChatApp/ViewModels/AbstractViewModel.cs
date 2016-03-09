using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ChatApp.ViewModels
{
    class AbstractViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        // IDataErrorInfo用のエラーメッセージを保持する辞書
        private Dictionary<string, string> _ErrorMessages = new Dictionary<string, string>();


        // エラーメッセージのセット
        protected void SetError(string propertyName, string errorMessage)
        {
            _ErrorMessages[propertyName] = errorMessage;
        }

        // エラーメッセージのクリア
        protected void ClearErrror(string propertyName)
        {
            if (_ErrorMessages.ContainsKey(propertyName))
                _ErrorMessages.Remove(propertyName);
        }

        public string Error
        {
            get { return string.Join(Environment.NewLine, _ErrorMessages.Select(r => r.Value)); }
        }

        public string this[string columnName]
        {
            get
            {
                return _ErrorMessages.ContainsKey(columnName) ? _ErrorMessages[columnName] : null;
            }
        }
    }
}
