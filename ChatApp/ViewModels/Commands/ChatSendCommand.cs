using ChatApp.AppServices;
using ChatApp.Models;

using System;

namespace ChatApp.ViewModels.Commands
{
    class ChatSendCommand : AbstractCommand
    {
        private ChatSendingService _Service;
        private string _Content;

        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Content {
            get { return _Content; }
            set { _Content = value; RaiseCanExecuteChanged(new EventArgs()); }
        }

        public ChatSendCommand(ChatSendingService service)
        {
            _Service = service;
        }

        public override bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(Content);
        }

        public override void Execute(object parameter)
        {
            _Service.CreateChatEntry(DateTime.Now, new User() { Name = Name, EmailAddress = EmailAddress }, Content);
            RaiseExecuted(new EventArgs());
        }
    }
}
