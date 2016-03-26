using ChatApp.Views;

using System.Windows;
using System.Windows.Threading;

namespace ChatApp
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);

            MainWindow window = new MainWindow();

            var viewModel = new ChatApp.ViewModels.MainWindowViewModel();
            window.DataContext = viewModel;

            window.Show();
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message + "\n" + e.Exception.StackTrace);
        }
    }
}
