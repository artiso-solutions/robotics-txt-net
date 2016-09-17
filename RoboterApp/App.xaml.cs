using System.Windows;
using log4net;

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
            var logger = LogManager.GetLogger(typeof(App));
            logger.Info("Starting ROBOTICS TXT Sample application");
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