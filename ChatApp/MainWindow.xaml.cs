using ChatApp.AppServices;
using ChatApp.DataStores;
using ChatApp.ViewModels;

using System;
using System.Windows;

namespace ChatApp
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var repository = new ChatEntryRepository(Properties.Settings.Default.ChatHistoryFilePath);
            var sendingService = new ChatSendingService(repository);
            var receivingService = new ChatReceivingService(repository);

            var chvm = new ChatHistoryViewModel(receivingService);
            var tsvm = new TextSenderViewModel(sendingService);
            ChatHistory.ItemsSource = chvm.Entries;
            ChatSendPanel.DataContext = tsvm;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Properties.Settings.Default.Save();
        }
    }
}
