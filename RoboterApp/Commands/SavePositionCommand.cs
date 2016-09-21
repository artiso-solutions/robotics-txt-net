using System;
using System.Windows.Input;
using RoboticsTxt.Lib.Components.Sequencer;

namespace RoboterApp.Commands
{
    class SavePositionCommand : ICommand
    {
        private readonly ControllerSequencer sequencer;

        public SavePositionCommand(ControllerSequencer sequencer)
        {
            this.sequencer = sequencer;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            this.sequencer.SaveCurrentPosition(parameter.ToString());
        }

        public event EventHandler CanExecuteChanged;
    }
}
