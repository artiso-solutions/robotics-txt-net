using RoboticsTxt.Lib.Components;
using RoboticsTxt.Lib.Components.Sequencer;
using RoboticsTxt.Lib.Contracts;

namespace RoboterApp.Commands
{
    public class ContinuousMoveAxisCommand
    {
        private readonly MotorPositionController motorPositionController;
        private readonly Direction direction;
        private readonly short maxDistance;
        private Speed? previousSpeed;
        private bool isMoving;

        public ContinuousMoveAxisCommand(MotorPositionController motorPositionController, Direction direction, short maxDistance)
        {
            this.motorPositionController = motorPositionController;
            this.direction = direction;
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
            motorPositionController.MotorRunDistance(currentSpeed, this.direction, maxDistance);
        }

        public void OnStop()
        {
            isMoving = false;

            previousSpeed = null;
            motorPositionController.StopMotor();
        }
    }
}