using ChatApp.AppServices;
using ChatApp.Models;
using ChatApp.ViewModels.Commands;

using System;
using System.Windows.Input;

namespace ChatApp.ViewModels
{
    class TextSenderViewModel : AbstractViewModel
    {
        private string _Content { get; set; }
        public string Content
        {
            get { return _Content; }
            set
            {
                _Content = value;
                OnPropertyChanged("Content");
            }
        }

        public ICommand SendCommand { get; set; }

        private ChatSendingService _sendingService;

        public TextSenderViewModel()
        {
            _sendingService = ChatAppContext.Context.ChatSendingService;

            SendCommand = new RelayCommand(
                    new Action<object>(SendMessage),
                    new Predicate<object>(o => !string.IsNullOrEmpty(Content))
                );
        }

        void SendMessage(object parameter)
        {
            string userName;
            string emailAddress;

            userName = Properties.Settings.Default.UserName;
            emailAddress = Properties.Settings.Default.EmailAddress;

            var user = new User() { Name = userName, EmailAddress = emailAddress };

            _sendingService.CreateChatEntry(DateTime.Now, user, Content);
            Content = string.Empty;
        }
    }
}
