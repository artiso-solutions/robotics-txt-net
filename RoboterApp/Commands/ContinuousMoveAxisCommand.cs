using RoboticsTxt.Lib.Components;
using RoboticsTxt.Lib.Components.Sequencer;
using RoboticsTxt.Lib.Contracts;

namespace RoboterApp.Commands
{
    public class ContinuousMoveAxisCommand
    {
        private readonly MotorPositionController motorPositionController;
        private readonly Movement movement;
        private readonly short maxDistance;
        private Speed? previousSpeed;
        private bool isMoving;

        public ContinuousMoveAxisCommand(MotorPositionController motorPositionController, Movement movement, short maxDistance)
        {
            this.motorPositionController = motorPositionController;
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
            motorPositionController.MotorRunDistance(currentSpeed, movement, maxDistance);
        }

        public void OnStop()
        {
            isMoving = false;

            previousSpeed = null;
            motorPositionController.StopMotor();
        }
    }
}