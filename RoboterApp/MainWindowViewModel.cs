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

            ReferenceAxisCommand = new ReferenceAxisCommand(controllerSequencer);

            MoveBackwardCommand = new ContinuousMoveAxisCommand(controllerSequencer, Motor.Two, Movement.Left);
            MoveForwardCommand = new ContinuousMoveAxisCommand(controllerSequencer, Motor.Two, Movement.Right);

            MoveUpCommand = new ContinuousMoveAxisCommand(controllerSequencer, Motor.Three, Movement.Left);
            MoveDownCommand = new ContinuousMoveAxisCommand(controllerSequencer, Motor.Three, Movement.Right);

            TurnLeftCommand = new ContinuousMoveAxisCommand(controllerSequencer, Motor.One, Movement.Left);
            TurnRightCommand = new ContinuousMoveAxisCommand(controllerSequencer, Motor.One, Movement.Right);

            OpenClampCommand = new ContinuousMoveAxisCommand(controllerSequencer, Motor.Four, Movement.Left);
            CloseClampCommand = new ContinuousMoveAxisCommand(controllerSequencer, Motor.Four, Movement.Right);
        }

        public void Dispose()
        {
            controllerSequencer.Dispose();
        }

        public ICommand ReferenceAxisCommand { get; }
        public ContinuousMoveAxisCommand MoveBackwardCommand { get; }
        public ContinuousMoveAxisCommand MoveForwardCommand { get; }
        public ContinuousMoveAxisCommand MoveUpCommand { get; }
        public ContinuousMoveAxisCommand MoveDownCommand { get; }
        public ContinuousMoveAxisCommand TurnLeftCommand { get; }
        public ContinuousMoveAxisCommand TurnRightCommand { get; }
        public ContinuousMoveAxisCommand OpenClampCommand { get; }
        public ContinuousMoveAxisCommand CloseClampCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}