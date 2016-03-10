using ChatApp.AppServices;
using ChatApp.Models;

using System.Collections.ObjectModel;

namespace ChatApp.ViewModels
{
    class ChatHistoryViewModel : AbstractViewModel
    {
        private ChatReceivingService _service;

        public ObservableCollection<ChatEntry> Entries { get; private set; }

        public ChatHistoryViewModel(ChatReceivingService service)
        {
            Entries = new ObservableCollection<ChatEntry>();
            _service = service;
            _service.ChatMessageReceived += ChatMessageReceived;
        }

        ~ChatHistoryViewModel()
        {
            _service.ChatMessageReceived -= ChatMessageReceived;
        }

        void ChatMessageReceived(object sender, ChatEntry entry)
        {
            Entries.Add(entry);
            Entries.Move(Entries.IndexOf(entry), 0);
        }
    }
}
