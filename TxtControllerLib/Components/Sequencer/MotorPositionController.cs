using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RoboticsTxt.Lib.Commands;
using RoboticsTxt.Lib.Components.Communicator;
using RoboticsTxt.Lib.Contracts;

namespace RoboticsTxt.Lib.Components.Sequencer
{
    public class MotorPositionController : IDisposable
    {
        private readonly ControllerCommunicator controllerCommunicator;
        private readonly ControllerSequencer controllerSequencer;
        private MotorDistanceInfo motorDistanceInfo;
        private Movement? currentDirection;

        private int currentPosition;

        public MotorConfiguration MotorConfiguration { get; }

        internal MotorPositionController(MotorConfiguration motorConfiguration, ControllerCommunicator controllerCommunicator, ControllerSequencer controllerSequencer)
        {
            this.controllerCommunicator = controllerCommunicator;
            this.controllerSequencer = controllerSequencer;
            MotorConfiguration = motorConfiguration;

            motorDistanceInfo = controllerCommunicator.MotorDistanceInfos.First(m => m.Motor == motorConfiguration.Motor);
            motorDistanceInfo.DistanceChanges.Subscribe(diff =>
            {
                if (currentDirection == null)
                {
                    return;
                }

                if (currentDirection.Value == motorConfiguration.ReferencingMovement)
                {
                    currentPosition -= diff;
                }
                else
                {
                    currentPosition += diff;
                }
            });
        }

        /// <summary>
        /// Starts the configured <see cref="MotorConfiguration"/> immediately and runs the specified <paramref name="distance"/>.
        /// </summary>
        /// <param name="speed">The speed of the motor.</param>
        /// <param name="movement">The direction to start.</param>
        /// <param name="distance">The distance to run.</param>
        public void MotorRunDistance(Speed speed, Movement movement, short distance)
        {
            currentDirection = movement;
            controllerCommunicator.QueueCommand(new MotorRunDistanceCommand(MotorConfiguration.Motor, speed, movement, distance));
        }

        /// <summary>
        /// Stops the specified <see cref="MotorConfiguration"/> immediately.
        /// </summary>
        public void StopMotor()
        {
            this.controllerCommunicator.QueueCommand(new StopMotorCommand(MotorConfiguration.Motor));
        }

        public async Task ReferenceAsync()
        {
            if (controllerSequencer.GetDigitalInputState(MotorConfiguration.ReferencingInput) == MotorConfiguration.ReferencingInputState)
            {
                var freeRunMovement = MotorConfiguration.ReferencingMovement == Movement.Left ? Movement.Right : Movement.Left;
                await controllerSequencer.StartMotorStopWithDigitalInputInternalAsync(MotorConfiguration.Motor, MotorConfiguration.ReferencingSpeed, freeRunMovement, MotorConfiguration.ReferencingInput, true);
                await controllerSequencer.StartMotorStopAfterTimeSpanInternalAsync(MotorConfiguration.Motor, MotorConfiguration.ReferencingSpeed, freeRunMovement, TimeSpan.FromMilliseconds(100));
            }

            await controllerSequencer.StartMotorStopWithDigitalInputInternalAsync(MotorConfiguration.Motor, MotorConfiguration.ReferencingSpeed, MotorConfiguration.ReferencingMovement, MotorConfiguration.ReferencingInput, MotorConfiguration.ReferencingInputState);

            currentPosition = 0;
        }

        public void Dispose()
        {
        }
    }
}