using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RoboticsTxt.Lib.Components;
using RoboticsTxt.Lib.Contracts;

namespace RoboterApp
{
    public class ReferenceAxisCommand : ICommand
    {
        private readonly ControllerSequencer controllerSequencer;

        public ReferenceAxisCommand(ControllerSequencer controllerSequencer)
        {
            this.controllerSequencer = controllerSequencer;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            // Turn Left/Right
            await ReferenceAxis(Motor.One, Movement.Right, Speed.Quick, DigitalInput.One);

            // Up / Down
            await ReferenceAxis(Motor.Three, Movement.Left, Speed.Fast, DigitalInput.Three);

            // Backward / Forward
            await ReferenceAxis(Motor.Two, Movement.Left, Speed.Maximal, DigitalInput.Two);

            // Open / Close clamp
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