using System;
using System.Windows.Input;
using RoboterApp.Components;

namespace RoboterApp.Commands
{
    public class StartSequenceCommand : ICommand
    {
        private readonly SequenceCommandLogic sequenceCommandLogic;

        public StartSequenceCommand(SequenceCommandLogic sequenceCommandLogic)
        {
            this.sequenceCommandLogic = sequenceCommandLogic;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.sequenceCommandLogic.RunPickUpBatchSequenceAsync(1);
        }

        public event EventHandler CanExecuteChanged;
    }
}
