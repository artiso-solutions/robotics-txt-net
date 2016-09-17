using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using JetBrains.Annotations;
using RoboticsTxt.Lib.Components;
using RoboticsTxt.Lib.Contracts;

namespace RoboterApp
{
    public class MainWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly ControllerSequencer controllerSequencer;

        public MainWindowViewModel()
        {
            controllerSequencer = new ControllerSequencer();

            MoveBackwardCommand = new MoveAxisCommand(controllerSequencer, Motor.Two, Movement.Left);
            MoveForwardCommand = new MoveAxisCommand(controllerSequencer, Motor.Two, Movement.Right);

            MoveUpCommand = new MoveAxisCommand(controllerSequencer, Motor.Three, Movement.Left);
            MoveDownCommand = new MoveAxisCommand(controllerSequencer, Motor.Three, Movement.Right);

            TurnLeftCommand = new MoveAxisCommand(controllerSequencer, Motor.One, Movement.Left);
            TurnRightCommand = new MoveAxisCommand(controllerSequencer, Motor.One, Movement.Right);

            OpenClampCommand = new MoveAxisCommand(controllerSequencer, Motor.Four, Movement.Left);
            CloseClampCommand = new MoveAxisCommand(controllerSequencer, Motor.Four, Movement.Right);
        }

        public void Dispose()
        {
            controllerSequencer.Dispose();
        }

        public ICommand MoveBackwardCommand { get; }
        public ICommand MoveForwardCommand { get; }
        public ICommand MoveUpCommand { get; }
        public ICommand MoveDownCommand { get; }
        public ICommand TurnLeftCommand { get; }
        public ICommand TurnRightCommand { get; }
        public ICommand OpenClampCommand { get; }
        public ICommand CloseClampCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}