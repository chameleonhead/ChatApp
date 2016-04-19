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
            var textContents = entries.Select(ent => ent.Content).OfType<ChatApp.ViewModels.TextContentViewModel>();
            if (textContents.Any())
            {
                Clipboard.SetText(string.Join(Environment.NewLine, textContents.Select(c => string.IsNullOrEmpty(c.Content) ? "" : c.Content.Replace("\n", Environment.NewLine)).ToArray()));
                return;
            }

            var imageContents = entries.Select(ent => ent.Content).OfType<ChatApp.ViewModels.ImageContentViewModel>();
            if (imageContents.Count() == 1)
            {
                Clipboard.SetImage(imageContents.First().Image);
                return;
            }

            var filePaths = entries.Select(ent => ent.Content).OfType<ChatApp.ViewModels.DataContentViewModel>().Select(dc => dc.SaveFile(System.IO.Path.GetTempPath()));
            if (filePaths.Any())
            {
                var fc = new System.Collections.Specialized.StringCollection();
                foreach (var s in filePaths)
                {
                    fc.Add(s);
                }
                Clipboard.SetFileDropList(fc);
            }
        }
    }
}
