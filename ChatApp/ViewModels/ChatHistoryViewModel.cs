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
        private ChatReceivingService _rcvService;
        private ChatEntryManagementService _mngService;

        public ObservableCollection<ChatEntryViewModel> Entries { get; private set; }

        public ChatHistoryViewModel(ChatReceivingService rcvService, ChatEntryManagementService mngService)
        {
            _rcvService = rcvService;
            _mngService = mngService;

            Entries = new ObservableCollection<ChatEntryViewModel>();

            foreach (var m in _rcvService.ReceivedMessages.Reverse())
            {
                Entries.Add(new ChatEntryViewModel(m));
            }

            _rcvService.ChatMessageReceived += ChatMessageReceived;
        }

        ~ChatHistoryViewModel()
        {
            _rcvService.ChatMessageReceived -= ChatMessageReceived;
        }

        void ChatMessageReceived(object sender, ChatMessageReceivedEventArgs e)
        {
            Entries.Insert(0, new ChatEntryViewModel(e.Entry));
        }

        public void DeleteEntry(ChatEntryViewModel model)
        {
            _mngService.DeleteChatEntry(model.Entry);
            Entries.Remove(model);
        }
    }
}
