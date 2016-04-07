using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ChatApp.Views
{
    /// <summary>
    /// ChatView.xaml の相互作用ロジック
    /// </summary>
    public partial class ChatView : UserControl
    {
        public ChatView()
        {
            InitializeComponent();
        }

        private void CommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var vw = e.OriginalSource as ListView;
            if (vw == null) return;

            var entries = vw.SelectedItems.Cast<ChatApp.Models.ChatEntry>();
            var contents = entries.Select(ent => ent.Content).Cast<ChatApp.Models.TextContent>();
            Clipboard.SetText(string.Join(Environment.NewLine, contents.Select(c => c.Value).ToArray()));
        }
    }
}
