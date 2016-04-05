using ChatApp.AppServices;
using ChatApp.AppServices.AppEvents;
using ChatApp.Models;

using System;
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
            _service = service;

            Entries = new ObservableCollection<ChatEntry>();

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

        void ChatMessageReceived(object sender, ChatMessageReceivedEventArgs e)
        {
            Entries.Insert(0, e.Entry);
        }
    }
}
