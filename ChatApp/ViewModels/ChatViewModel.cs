using ChatApp.AppServices;
using ChatApp.DataStores;
using ChatApp.Models;
using ChatApp.ViewModels.Helpers;

using System.Windows;

namespace ChatApp.ViewModels
{
    class ChatViewModel : AbstractViewModel
    {
        private ChatReceivingService _ChatReceivingService;
        private ChatSendingService _ChatSendingService;
        private ChatEntryRepository _ChatEntryRepository;

        public ChatHistoryViewModel ChatHistoryViewModel { get; private set; }
        public ChatSenderViewModel TextSenderViewModel { get; private set; }

        public string Title
        {
            get
            {
                return _source != null ? new System.IO.FileInfo(_source.DocumentUri.LocalPath).Name : string.Empty;
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    _unreadCount = 0;
                    OnPropertyChanged("IsSelected");
                    OnPropertyChanged("UnreadCount");
                }
            }
        }

        private int _unreadCount;
        public int UnreadCount
        {
            get
            {
                return _unreadCount;
            }
            private set
            {
                _unreadCount = value;
                OnPropertyChanged("UnreadCount");
            }
        }

        private ChatSource _source;

        public ChatViewModel(ChatSource source)
        {
            _source = source;

            _ChatEntryRepository = new ChatEntryRepository(source);
            _ChatReceivingService = new ChatReceivingService(source, _ChatEntryRepository);
            _ChatSendingService = new ChatSendingService(_ChatEntryRepository);

            ChatHistoryViewModel = new ChatHistoryViewModel(_ChatReceivingService);
            TextSenderViewModel = new ChatSenderViewModel(_ChatSendingService);
            _ChatReceivingService.ChatMessageReceived += ChatMessageReceived;
        }

        void ChatMessageReceived(object sender, AppServices.AppEvents.ChatMessageReceivedEventArgs entry)
        {
            // Windowを点滅させる
            var helper = new FlashWindowHelper(Application.Current);
            helper.FlashApplicationWindow();

            if (!IsSelected) UnreadCount++;
        }
    }
}
