using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using RoboterApp.Annotations;
using RoboticsTxt.Lib.Components;

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

    public class MainWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly ControllerSequencer controllerSequencer;

        public MainWindowViewModel()
        {
            controllerSequencer = new ControllerSequencer();
        }

        public void Dispose()
        {
            controllerSequencer.Dispose();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}