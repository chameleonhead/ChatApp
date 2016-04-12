using ChatApp.AppServices;
using ChatApp.Models;
using ChatApp.ViewModels.Commands;
using System;

namespace ChatApp.ViewModels
{
    class ChatSenderViewModel : AbstractViewModel
    {
        private User _user;

        private string _content;

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged("Content");
            }
        }

        public RelayCommand SendCommand { get; private set; }

        private ChatSendingService _sendingService;

        public ChatSenderViewModel(ChatSendingService sendingService)
        {
            _sendingService = sendingService;

            var userName = Properties.Settings.Default.UserName;
            var emailAddress = Properties.Settings.Default.EmailAddress;

            _user = new User() { Name = userName, EmailAddress = emailAddress };

            SendCommand = new RelayCommand(
                    o => SendMessage(),
                    o => CanSendMessage()
                );
        }

        public bool CanSendMessage()
        {
            return !string.IsNullOrEmpty(Content);
        }

        public void SendMessage()
        {
            _sendingService.CreateChatEntry(DateTime.Now, _user, Content);
            Content = string.Empty;
        }

        public void SendImage(string fileName, byte[] data)
        {
            _sendingService.CreateChatEntry(DateTime.Now, _user, ChatContentType.Image, fileName, data);
            Content = string.Empty;
        }

        public void SendFile(string fileName, byte[] data)
        {
            _sendingService.CreateChatEntry(DateTime.Now, _user, ChatContentType.File, fileName, data);
            Content = string.Empty;
        }
    }
}
