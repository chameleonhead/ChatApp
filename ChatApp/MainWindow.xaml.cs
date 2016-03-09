using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatApp.ViewModels;
using System.Xml.Linq;
using ChatApp.DataStores;
using ChatApp.AppServices;
using System.IO;

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
            var service = new ChatEntryService(repository);

            var chvm = new ChatHistoryViewModel(repository, Properties.Settings.Default.ReloadTimeInMillis);
            var tsvm = new TextSenderViewModel(service, Properties.Settings.Default.UserName, Properties.Settings.Default.EmailAddress);
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
