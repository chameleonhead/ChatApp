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
                SendCommand.Content = _Content;
                RaisePropertyChanged("Content");
            }
        }

        public ChatSendCommand SendCommand { get; set; }

        public TextSenderViewModel()
        {
            string userName;
            string emailAddress;

            var sendingService = ChatAppContext.Context.ChatSendingService;


            userName = Properties.Settings.Default.UserName;
            emailAddress = Properties.Settings.Default.EmailAddress;

            SendCommand = new ChatSendCommand(sendingService) { Name = userName, EmailAddress = emailAddress };
            SendCommand.Executed += new EventHandler(SendCommand_Executed);
        }

        void SendCommand_Executed(object sender, EventArgs e)
        {
            Content = string.Empty;
        }
    }
}
