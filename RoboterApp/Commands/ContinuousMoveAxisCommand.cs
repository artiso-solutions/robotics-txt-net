using RoboticsTxt.Lib.Components;
using RoboticsTxt.Lib.Contracts;

namespace RoboterApp.Commands
{
    public class ContinuousMoveAxisCommand
    {
        private readonly ControllerSequencer controllerSequencer;
        private readonly Motor motor;
        private readonly Movement movement;
        private Speed? previousSpeed;

        public ContinuousMoveAxisCommand(ControllerSequencer controllerSequencer, Motor motor, Movement movement)
        {
            this.controllerSequencer = controllerSequencer;
            this.motor = motor;
            this.movement = movement;
        }

        public void OnMove(Speed currentSpeed)
        {
            if (previousSpeed == currentSpeed)
            {
                return;
            }

            previousSpeed = currentSpeed;
            controllerSequencer.StartMotor(motor, currentSpeed, movement);
        }

        public void OnStop()
        {
            previousSpeed = null;
            controllerSequencer.StopMotor(motor);
        }
    }
}