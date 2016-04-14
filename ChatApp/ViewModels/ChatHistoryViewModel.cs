using ChatApp.AppServices;
using ChatApp.AppServices.AppEvents;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ChatApp.ViewModels
{
    class ChatHistoryViewModel : AbstractViewModel
    {
        private ChatReceivingService _service;

        public ObservableCollection<ChatEntryViewModel> Entries { get; private set; }

        public ChatHistoryViewModel(ChatReceivingService service)
        {
            _service = service;

            Entries = new ObservableCollection<ChatEntryViewModel>();

            foreach (var m in _service.ReceivedMessages.Reverse())
            {
                Entries.Add(new ChatEntryViewModel(m));
            }

            _service.ChatMessageReceived += ChatMessageReceived;
        }

        ~ChatHistoryViewModel()
        {
            _service.ChatMessageReceived -= ChatMessageReceived;
        }

        void ChatMessageReceived(object sender, ChatMessageReceivedEventArgs e)
        {
            Entries.Insert(0, new ChatEntryViewModel(e.Entry));
        }
    }
}
