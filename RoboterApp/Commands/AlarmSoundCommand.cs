using System;
using System.Windows.Input;
using RoboticsTxt.Lib.Components.Sequencer;
using RoboticsTxt.Lib.Contracts;

namespace RoboterApp.Commands
{
    public class AlarmSoundCommand : ICommand
    {
        private readonly IControllerSequencer sequencer;

        public AlarmSoundCommand(IControllerSequencer sequencer)
        {
            this.sequencer = sequencer;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            sequencer.PlaySound(Sound.Alarm, 3);
        }

        public event EventHandler CanExecuteChanged;
    }
}
