using System;
using System.Threading.Tasks;
using System.Windows.Input;
using log4net;
using log4net.Util;
using RoboticsTxt.Lib.Components;
using RoboticsTxt.Lib.Components.Sequencer;
using RoboticsTxt.Lib.Contracts;

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

        public async void Execute(object parameter)
        {
            logger.InfoExt("Start referencing of axis...");
            logger.InfoExt("Reference \"Turn Left / Right\"");
            await turnLeftRightPositionController.ReferenceAsync();

            logger.InfoExt("Reference \"Move Up / Down\"");
            await upDownPositionController.ReferenceAsync();

            logger.InfoExt("Reference \"Move Backward / Forward\"");
            await backwardForwardPositionController.ReferenceAsync();

            logger.InfoExt("Reference \"Open / Close clamp\"");
            await openCloseClampPositionController.ReferenceAsync();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}