using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading;
using ChatApp.ViewModels;
using System.Windows.Media.Imaging;

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
            DataObject.AddPastingHandler(tb, TextBox_Pasted);
        }

        private void TextBox_Drop(object sender, DragEventArgs e)
        {
            ChatSenderViewModel model = DataContext as ChatSenderViewModel;
            if (model == null) return;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                SendFiles(files, model);
                e.Handled = true;
            }
        }

        private static void SendFiles(string[] files, ChatSenderViewModel model)
        {
            var imageExtensions = new string[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
            // コマンドを実行する

            foreach (var fi in files.Select(f => new FileInfo(f)))
            {

                if (imageExtensions.Any(ext => ext == fi.Extension.ToLower()))
                {
                    SendFileAsync(model.SendImage, fi);
                }
                else
                {
                    SendFileAsync(model.SendFile, fi);
                }
            }
        }

        private static void SendFileAsync(Action<string, byte[]> sendAction, FileInfo fi)
        {
            SynchronizationContext context = SynchronizationContext.Current;
            byte[] data = new byte[fi.Length];

            var fs = fi.OpenRead();
            fs.BeginRead(data, 0, data.Length, new AsyncCallback(iar =>
            {
                fs.Close();
                context.Post(new SendOrPostCallback(o => sendAction(fi.Name, (byte[])o)), data);
            }), null);
        }

        private void TextBox_DragOver(object sender, DragEventArgs e)
        {
            // ファイルをドロップされた場合のみe.Handled を True にする
            e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
        }

        private void TextBox_Pasted(object sender, DataObjectPastingEventArgs e)
        {
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ChatSenderViewModel model = DataContext as ChatSenderViewModel;
            if (model == null) return;

            var image = Clipboard.GetImage();
            if (image != null)
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                using (MemoryStream stream = new MemoryStream())
                {
                    encoder.Frames.Add(BitmapFrame.Create(image));
                    encoder.Save(stream);
                    byte[] data = stream.ToArray();
                    model.SendImage(Guid.NewGuid().ToString() + ".png", data);
                    stream.Close();
                    e.Handled = true;
                }
                e.Handled = true;
            }

            var fileList = Clipboard.GetFileDropList();
            if (fileList.Count > 0)
            {
                string[] files = (string[])fileList.Cast<string>().ToArray();
                SendFiles(files, model);
                e.Handled = true;
            }

            var text = Clipboard.GetText();
            tb.AppendText(text);
        }
    }
}
