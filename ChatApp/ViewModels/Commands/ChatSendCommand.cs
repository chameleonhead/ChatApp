using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatApp.AppServices;
using ChatApp.Models;
using ChatApp.DataStores;
using System.Xml.Linq;

namespace ChatApp.ViewModels.Commands
{
    class ChatSendCommand : AbstractCommand
    {
        private ChatEntryService _Service;
        private string _Content;

        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Content {
            get { return _Content; }
            set { _Content = value; RaiseCanExecuteChanged(new EventArgs()); }
        }

        public ChatSendCommand(ChatEntryService service)
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
