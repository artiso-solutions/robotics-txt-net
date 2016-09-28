using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using RoboticsTxt.Lib.Components.Communicator;
using RoboticsTxt.Lib.Contracts;
using RoboticsTxt.Lib.Interfaces;
using RoboticsTxt.Lib.Messages;

namespace RoboticsTxt.Lib.Commands
{
    internal class MotorRunDistanceCommand : IControllerCommand
    {
        private readonly Speed speed;
        private readonly Direction direction;
        private readonly short distance;

        private readonly TaskCompletionSource<object> taskCompletionSource;

        public MotorRunDistanceCommand(Motor motor, Speed speed, Direction direction, short distance)
        {
            this.Motor = motor;
            this.speed = speed;
            this.direction = direction;
            this.distance = distance;

            this.taskCompletionSource = new TaskCompletionSource<object>();
        }

        public Motor Motor { get; }

        public void Execute([NotNull] ControllerCommunicator controllerCommunicator, [NotNull] ExchangeDataCommandMessage message)
        {
            if (controllerCommunicator == null) throw new ArgumentNullException(nameof(controllerCommunicator));
            if (message == null) throw new ArgumentNullException(nameof(message));

            var motorIndex = (int)this.Motor;
            var speedValue = (short)this.speed;

            switch (this.direction)
            {
                case Direction.Left:
                    message.PwmOutputValues[2 * motorIndex] = speedValue;
                    message.PwmOutputValues[2 * motorIndex + 1] = 0;
                    break;
                case Direction.Right:
                    message.PwmOutputValues[2 * motorIndex] = 0;
                    message.PwmOutputValues[2 * motorIndex + 1] = speedValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            message.MotorDistance[motorIndex] = this.distance;
            message.MotorCommandId[motorIndex]++;
            
            controllerCommunicator.MotorDistanceInfos[motorIndex].CommandIdChanges.FirstAsync(cc => cc == message.MotorCommandId[motorIndex]).Subscribe(cc => this.taskCompletionSource.SetResult(null));
        }

        public async Task WaitForCompletionAsync()
        {
            await this.taskCompletionSource.Task;
        }
    }
}