using ChatApp.AppServices;
using ChatApp.Models;

using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

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
            foreach (var m in _service.ReceivedMessages.Reverse())
            {
                Entries.Add(m);
            }
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

            // Windowを点滅させる
            var helper = new FlashWindowHelper(Application.Current);
            helper.FlashApplicationWindow();
        }
    }
}
