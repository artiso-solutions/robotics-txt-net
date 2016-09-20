using RoboticsTxt.Lib.Components;
using RoboticsTxt.Lib.Contracts;

namespace RoboterApp.Commands
{
    public class ContinuousMoveAxisCommand
    {
        private readonly ControllerSequencer controllerSequencer;
        private readonly Motor motor;
        private readonly Movement movement;
        private readonly short maxDistance;
        private Speed? previousSpeed;
        private bool isMoving;

        public ContinuousMoveAxisCommand(ControllerSequencer controllerSequencer, Motor motor, Movement movement, short maxDistance)
        {
            this.controllerSequencer = controllerSequencer;
            this.motor = motor;
            this.movement = movement;
            this.maxDistance = maxDistance;
        }

        public void OnMove(Speed currentSpeed)
        {
            if (isMoving)
            {
                return;
            }

            isMoving = true;

            if (previousSpeed == currentSpeed)
            {
                return;
            }

            previousSpeed = currentSpeed;
            controllerSequencer.MotorRunDistance(motor, currentSpeed, movement, maxDistance);
        }

        public void OnStop()
        {
            isMoving = false;

            previousSpeed = null;
            controllerSequencer.StopMotor(motor);
        }
    }
}