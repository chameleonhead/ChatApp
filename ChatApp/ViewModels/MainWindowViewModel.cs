using ChatApp.AppServices;
using ChatApp.DataStores;
using ChatApp.Views.ViewHelpers;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;


namespace ChatApp.ViewModels
{
    class MainWindowViewModel : AbstractViewModel
    {
        private ChatReceivingService _ChatReceivingService;
        private ChatSendingService _ChatSendingService;
        private ChatEntryRepository _ChatEntryRepository;

        public ObservableCollection<ChatViewModel> ChatViewModels { get; private set; }

        public MainWindowViewModel()
        {
            var loadService = new ChatSourceLoadService();
            var sources = loadService.Soucres;

            _ChatEntryRepository = new ChatEntryRepository(sources);
            _ChatReceivingService = new ChatReceivingService(_ChatEntryRepository);
            _ChatSendingService = new ChatSendingService(_ChatEntryRepository);

            var chatViews = new List<ChatViewModel>();

            foreach (var s in sources)
            {
                chatViews.Add(
                        new ChatViewModel(s, _ChatReceivingService, _ChatSendingService)
                    );
            }

            ChatViewModels = new ObservableCollection<ChatViewModel>(chatViews);
            OnPropertyChanged("ChatViewModels");

            _ChatReceivingService.ChatMessageReceived += ChatMessageReceived;
        }

        ~MainWindowViewModel()
        {
            _ChatReceivingService.ChatMessageReceived -= ChatMessageReceived;
        }

        void ChatMessageReceived(object sender, AppServices.AppEvents.ChatMessageReceivedEventArgs entry)
        {
            Application.Current.Dispatcher.BeginInvoke(
                new Action(() =>
                {
                    // Windowを点滅させる
                    var helper = new FlashWindowHelper(Application.Current);
                    helper.FlashApplicationWindow();
                }));
        }
    }
}
