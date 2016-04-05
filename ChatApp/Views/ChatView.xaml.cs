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

            var ce = vw.SelectedItems.Cast<ChatApp.Models.ChatEntry>();
            Clipboard.SetText(string.Join(Environment.NewLine, ce.Select(ent => ent.Content).ToArray()));
        }
    }
}
