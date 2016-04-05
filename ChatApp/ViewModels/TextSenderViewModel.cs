﻿using ChatApp.AppServices;
using ChatApp.Models;
using ChatApp.ViewModels.Commands;
using System;

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

        public RelayCommand SendCommand { get; private set; }

        private ChatSendingService _sendingService;

        public TextSenderViewModel(ChatSendingService sendingService)
        {
            _sendingService = sendingService;

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
