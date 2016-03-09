using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ChatApp.ViewModels.Commands;
using ChatApp.AppServices;

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
                SendCommand.Content = _Content;
                RaisePropertyChanged("Content");
            }
        }

        public ChatSendCommand SendCommand { get; set; }

        public TextSenderViewModel(ChatEntryService service, string userName, string emailAddress)
        {
            SendCommand = new ChatSendCommand(service) { Name = userName, EmailAddress = emailAddress };
            SendCommand.Executed += new EventHandler(SendCommand_Executed);
        }

        void SendCommand_Executed(object sender, EventArgs e)
        {
            Content = string.Empty;
        }
    }
}
