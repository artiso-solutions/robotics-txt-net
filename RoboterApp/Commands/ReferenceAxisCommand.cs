using System;
using System.Threading.Tasks;
using System.Windows.Input;
using log4net;
using log4net.Util;
using RoboticsTxt.Lib.Components;
using RoboticsTxt.Lib.Contracts;

namespace RoboterApp.Commands
{
    public class ReferenceAxisCommand : ICommand
    {
        private readonly ILog logger;
        private readonly ControllerSequencer controllerSequencer;

        public ReferenceAxisCommand(ControllerSequencer controllerSequencer)
        {
            logger = LogManager.GetLogger(typeof(ReferenceAxisCommand));
            this.controllerSequencer = controllerSequencer;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            logger.InfoExt("Start referencing of axis...");
            logger.InfoExt("Reference \"Turn Left / Right\"");
            await ReferenceAxis(Motor.One, Movement.Right, Speed.Quick, DigitalInput.One);

            logger.InfoExt("Reference \"Move Up / Down\"");
            await ReferenceAxis(Motor.Three, Movement.Left, Speed.Fast, DigitalInput.Three);

            logger.InfoExt("Reference \"Move Backward / Forward\"");
            await ReferenceAxis(Motor.Two, Movement.Left, Speed.Maximal, DigitalInput.Two);

            logger.InfoExt("Reference \"Open / Close clamp\"");
            await ReferenceAxis(Motor.Four, Movement.Left, Speed.Quick, DigitalInput.Four);
        }

        private async Task ReferenceAxis(Motor motor, Movement movement, Speed referenceSpeed, DigitalInput referenceInput)
        {
            await controllerSequencer.StartMotorStopWithDigitalInputAsync(motor, referenceSpeed, movement, referenceInput, false);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}