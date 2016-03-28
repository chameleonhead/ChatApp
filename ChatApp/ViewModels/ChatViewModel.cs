using ChatApp.AppServices;
using ChatApp.Models;

namespace ChatApp.ViewModels
{
    class ChatViewModel : AbstractViewModel
    {
        public ChatHistoryViewModel ChatHistoryViewModel { get; private set; }
        public TextSenderViewModel TextSenderViewModel { get; private set; }

        public string Title
        {
            get
            {
                return _Source != null ? new System.IO.FileInfo(_Source.DocumentUri.LocalPath).Name : string.Empty;
            }
        }

        private ChatSource _Source;
        public ChatSource Source 
        {
            get { return _Source; }
            set
            {
                _Source = value;
            }
        }

        public ChatViewModel(ChatSource source, ChatReceivingService receivingService, ChatSendingService sedingService)
        {
            Source = source;

            ChatHistoryViewModel = new ChatHistoryViewModel(source, receivingService);
            TextSenderViewModel = new TextSenderViewModel(source, sedingService);
        }
    }
}
