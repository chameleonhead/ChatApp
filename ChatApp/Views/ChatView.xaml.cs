using System.Windows.Controls;
using System.Windows.Input;

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

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter && SendButton.Command != null && SendButton.Command.CanExecute(null))
                SendButton.Command.Execute(null);
        }
    }
}
