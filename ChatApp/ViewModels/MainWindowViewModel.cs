using ChatApp.AppServices;
using ChatApp.Models;
using ChatApp.ViewModels.Commands;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChatApp.ViewModels
{
    class MainWindowViewModel : AbstractViewModel
    {
        public ObservableCollection<ChatViewModel> ChatViewModels { get; private set; }

        private ChatSourceLoadService _sourceLoadService;

        public MainWindowViewModel()
        {
            _sourceLoadService = new ChatSourceLoadService();
            var sources = _sourceLoadService.Load();

            var chatViews = new List<ChatViewModel>();

            foreach (var s in sources)
            {
                chatViews.Add(new ChatViewModel(s));
            }

            ChatViewModels = new ObservableCollection<ChatViewModel>(chatViews);
            OnPropertyChanged("ChatViewModels");
        }

        public void OpenChatSourceFromPath(string filePath)
        {
            var s = new ChatSource(new Uri(filePath));
            ChatViewModels.Add(new ChatViewModel(s));

            _sourceLoadService.Save(ChatViewModels.Select(v => v.Source));
        }

        public bool CanCloseChatView()
        {
            return ChatViewModels.Any(v => v.IsSelected);
        }

        public void CloseChatView()
        {
            var vm = ChatViewModels.Where(v => v.IsSelected).FirstOrDefault();
            var s = vm.Source;
            if (vm != null)
                ChatViewModels.Remove(vm);

            _sourceLoadService.Save(ChatViewModels.Select(v => v.Source));
        }
    }
}
