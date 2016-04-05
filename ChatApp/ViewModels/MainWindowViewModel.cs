using ChatApp.AppServices;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ChatApp.ViewModels
{
    class MainWindowViewModel : AbstractViewModel
    {
        public ObservableCollection<ChatViewModel> ChatViewModels { get; private set; }

        public MainWindowViewModel()
        {
            var loadService = new ChatSourceLoadService();
            var sources = loadService.Load();

            var chatViews = new List<ChatViewModel>();

            foreach (var s in sources)
            {
                chatViews.Add(new ChatViewModel(s));
            }

            ChatViewModels = new ObservableCollection<ChatViewModel>(chatViews);
            OnPropertyChanged("ChatViewModels");
        }
    }
}
