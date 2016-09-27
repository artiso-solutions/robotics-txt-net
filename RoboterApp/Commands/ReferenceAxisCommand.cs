using System;
using System.Threading.Tasks;
using System.Windows.Input;
using log4net;
using log4net.Util;
using RoboticsTxt.Lib.Components.Sequencer;

namespace RoboterApp.Commands
{
    public class ReferenceAxisCommand : ICommand
    {
        private readonly MotorPositionController turnLeftRightPositionController;
        private readonly MotorPositionController upDownPositionController;
        private readonly MotorPositionController backwardForwardPositionController;
        private readonly MotorPositionController openCloseClampPositionController;
        private readonly ILog logger;

        public ReferenceAxisCommand(MotorPositionController turnLeftRightPositionController, MotorPositionController upDownPositionController, MotorPositionController backwardForwardPositionController, MotorPositionController openCloseClampPositionController)
        {
            this.turnLeftRightPositionController = turnLeftRightPositionController;
            this.upDownPositionController = upDownPositionController;
            this.backwardForwardPositionController = backwardForwardPositionController;
            this.openCloseClampPositionController = openCloseClampPositionController;

            logger = LogManager.GetLogger(typeof(ReferenceAxisCommand));
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var referenceTasks = new[]
            {
                turnLeftRightPositionController.MoveMotorToReferenceAsync(),
                upDownPositionController.MoveMotorToReferenceAsync(),
                backwardForwardPositionController.MoveMotorToReferenceAsync(),
                openCloseClampPositionController.MoveMotorToReferenceAsync()
            };
            
            logger.InfoExt("Start referencing of axis...");
            foreach (var referenceTask in referenceTasks)
            {
                Task.Run(() => referenceTask);
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}