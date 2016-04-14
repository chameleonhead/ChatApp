using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ChatApp.Views
{
    /// <summary>
    /// ChatHistoryView.xaml の相互作用ロジック
    /// </summary>
    public partial class ChatHistoryView : UserControl
    {
        public ChatHistoryView()
        {
            InitializeComponent();
        }

        private void CommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var vw = e.OriginalSource as ListView;
            if (vw == null) return;

            var entries = vw.SelectedItems.OfType<ChatApp.ViewModels.ChatEntryViewModel>();
            var contents = entries.Select(ent => ent.Content).OfType<ChatApp.ViewModels.TextContentViewModel>();
            Clipboard.SetText(string.Join(Environment.NewLine, contents.Select(c => string.IsNullOrEmpty(c.Content) ? "" : c.Content.Replace("\n", Environment.NewLine)).ToArray()));
        }
    }
}
