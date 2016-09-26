using RoboticsTxt.Lib.Components.Sequencer;
using RoboticsTxt.Lib.Contracts;

namespace RoboterApp.Commands
{
    public class ContinuousMoveAxisCommand
    {
        private readonly MotorPositionController motorPositionController;
        private readonly Direction direction;
        private Speed? previousSpeed;
        private short previousDistance;

        public ContinuousMoveAxisCommand(MotorPositionController motorPositionController, Direction direction)
        {
            this.motorPositionController = motorPositionController;
            this.direction = direction;
        }

        public void OnMove(Speed currentSpeed, short currentDistance = 0)
        {
            if ((previousSpeed == currentSpeed) && (previousDistance == currentDistance))
            {
                return;
            }

            previousSpeed = currentSpeed;
            previousDistance = currentDistance;

            if (currentDistance > 0)
            {
                motorPositionController.StartMotorAndMoveDistanceAsync(currentSpeed, this.direction, currentDistance);
            }
            else
            {
                motorPositionController.StartMotorAsync(currentSpeed, this.direction);
            }
        }

        public void OnStop()
        {
            previousSpeed = null;
            previousDistance = -1;
            motorPositionController.StopMotor();
        }
    }
}