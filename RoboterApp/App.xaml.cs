using System.Windows;

namespace RoboterApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private MainWindowViewModel mainWindowViewModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            mainWindowViewModel = new MainWindowViewModel();
            MainWindow = new MainWindow();
            MainWindow.DataContext = mainWindowViewModel;
            MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            mainWindowViewModel.Dispose();
        }
    }
}