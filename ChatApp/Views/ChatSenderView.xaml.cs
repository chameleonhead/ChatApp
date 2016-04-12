using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading;
using ChatApp.ViewModels;

namespace ChatApp.Views
{
    /// <summary>
    /// ChatSenderView.xaml の相互作用ロジック
    /// </summary>
    public partial class ChatSenderView : UserControl
    {
        private static readonly RoutedUICommand SendFileCommand 
            = new RoutedUICommand("SendFile", "Chat.Views.SendFileCommand", typeof(ChatSenderView));

        public ChatSenderView()
        {
            InitializeComponent();
        }

        private void TextBox_Drop(object sender, DragEventArgs e)
        {
            ChatSenderViewModel model = DataContext as ChatSenderViewModel;
            if (model == null) return;

            SynchronizationContext context = SynchronizationContext.Current;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var imageExtensions = new string[] {".jpg", ".jpeg", ".png", ".bmp", ".gif"};
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                // コマンドを実行する

                foreach (var fi in files.Select(f => new FileInfo(f)))
                {

                    if (imageExtensions.Any(ext => ext == fi.Extension.ToLower()))
                    {
                        byte[] data = new byte[fi.Length];
                        var fs = fi.OpenRead();
                        fs.BeginRead(data, 0, data.Length, new AsyncCallback(iar =>
                        {
                            fs.Close();
                            context.Post(new SendOrPostCallback(o => model.SendImage(fi.Name, (byte[])o)), data);
                        }), null);
                    }
                    else
                    {
                        byte[] data = new byte[fi.Length];
                        var fs = fi.OpenRead();
                        fs.BeginRead(data, 0, data.Length, new AsyncCallback(iar =>
                        {
                            fs.Close();
                            context.Post(new SendOrPostCallback(o => model.SendFile(fi.Name, (byte[])o)), data);
                        }), null);
                    }
                }
                e.Handled = true;
            }
        }

        private void TextBox_DragOver(object sender, DragEventArgs e)
        {
            // ファイルをドロップされた場合のみe.Handled を True にする
            e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
        }
    }
}
