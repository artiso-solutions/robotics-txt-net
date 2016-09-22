using System;
using System.Windows.Input;
using RoboticsTxt.Lib.Components.Sequencer;

namespace RoboterApp.Commands
{
    public class MoveToPositionCommand : ICommand
    {
        private readonly ControllerSequencer controllerSequencer;

        public MoveToPositionCommand(ControllerSequencer controllerSequencer)
        {
            this.controllerSequencer = controllerSequencer;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.controllerSequencer.MoveToPosition(parameter.ToString());
        }

        public event EventHandler CanExecuteChanged;
    }
}
