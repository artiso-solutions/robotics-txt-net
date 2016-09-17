using System;
using System.Windows.Input;
using RoboticsTxt.Lib.Components;
using RoboticsTxt.Lib.Contracts;

namespace RoboterApp
{
    public class MoveAxisCommand : ICommand
    {
        private readonly ControllerSequencer controllerSequencer;
        private readonly Motor motor;
        private readonly Movement movement;

        public MoveAxisCommand(ControllerSequencer controllerSequencer, Motor motor, Movement movement)
        {
            this.controllerSequencer = controllerSequencer;
            this.motor = motor;
            this.movement = movement;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            await controllerSequencer.StartMotorStopAfterAsync(motor, Speed.Quick, movement, TimeSpan.FromSeconds(1));
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}