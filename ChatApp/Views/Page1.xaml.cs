using ChatApp.AppServices;
using ChatApp.DataStores;
using ChatApp.ViewModels;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChatApp.Views
{
    /// <summary>
    /// Page1.xaml の相互作用ロジック
    /// </summary>
    public partial class Page1 : Page
    {
        private TextSenderViewModel _TextSenderViewModel;
        private ChatHistoryViewModel _ChatHistoryViewModel;

        public Page1()
        {
            InitializeComponent();

            var repository = new ChatEntryRepository(Properties.Settings.Default.ChatHistoryFilePath);
            var sendingService = new ChatSendingService(repository);
            var receivingService = new ChatReceivingService(repository);

            _ChatHistoryViewModel = new ChatHistoryViewModel(receivingService);
            _TextSenderViewModel = new TextSenderViewModel(sendingService);
            ChatHistory.ItemsSource = _ChatHistoryViewModel.Entries;
            ChatSendPanel.DataContext = _TextSenderViewModel;
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter && _TextSenderViewModel.SendCommand.CanExecute(null))
                _TextSenderViewModel.SendCommand.Execute(null);
        }
    }
}
