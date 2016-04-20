using ChatApp.ViewModels;

using Microsoft.Win32;
using System.Windows;

namespace ChatApp.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenCommandExecuted(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;
            if (vm == null) return;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.CheckFileExists = false;
            dialog.Filter = "チャットファイル(.xml)|*.xml";
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                var fn = dialog.FileName;
                vm.OpenChatSourceFromPath(fn);
            }
        }

        private void CloseCommandExecuted(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;
            if (vm == null) return;

            vm.CloseChatView();
        }

        private void CloseCommandCanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;
            if (vm == null) return;

            e.CanExecute = vm.CanCloseChatView();
        }
    }
}
