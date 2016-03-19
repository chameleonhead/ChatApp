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

            _ChatHistoryViewModel = new ChatHistoryViewModel();
            _TextSenderViewModel = new TextSenderViewModel();
            ChatHistory.ItemsSource = _ChatHistoryViewModel.Entries;
            ChatSendPanel.DataContext = _TextSenderViewModel;

            WindowTitle = Properties.Settings.Default.ChatHistoryFilePath;
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter && _TextSenderViewModel.SendCommand.CanExecute(null))
                _TextSenderViewModel.SendCommand.Execute(null);
        }
    }
}
