using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChatApp.Views
{
    /// <summary>
    /// ChatSenderView.xaml の相互作用ロジック
    /// </summary>
    public partial class ChatSenderView : UserControl
    {
        public ChatSenderView()
        {
            InitializeComponent();
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                // コマンドを実行する
            }
            e.Handled = true;
        }
    }
}
