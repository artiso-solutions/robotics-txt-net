using RoboticsTxt.Lib.Components.Sequencer;
using RoboticsTxt.Lib.Contracts;

namespace RoboterApp.Commands
{
    public class ContinuousMoveAxisCommand
    {
        private readonly MotorPositionController motorPositionController;
        private readonly Direction direction;
        private Speed? previousSpeed;
        private bool isMoving;

        public ContinuousMoveAxisCommand(MotorPositionController motorPositionController, Direction direction)
        {
            this.motorPositionController = motorPositionController;
            this.direction = direction;
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
            motorPositionController.StartMotor(currentSpeed, this.direction);
        }

        public void OnStop()
        {
            isMoving = false;

            previousSpeed = null;
            motorPositionController.StopMotor();
        }
    }
}