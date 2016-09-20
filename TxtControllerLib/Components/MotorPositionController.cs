using System;
using System.Threading.Tasks;
using RoboticsTxt.Lib.Commands;
using RoboticsTxt.Lib.Contracts;

namespace RoboticsTxt.Lib.Components
{
    public class MotorPositionController : IDisposable
    {
        private readonly ControllerCommunicator controllerCommunicator;
        private readonly ControllerSequencer controllerSequencer;
        public MotorConfiguration MotorConfiguration { get; }

        internal MotorPositionController(MotorConfiguration motorConfiguration, ControllerCommunicator controllerCommunicator, ControllerSequencer controllerSequencer)
        {
            this.controllerCommunicator = controllerCommunicator;
            this.controllerSequencer = controllerSequencer;
            MotorConfiguration = motorConfiguration;
        }

        /// <summary>
        /// Starts the configured <see cref="MotorConfiguration"/> immediately and runs the specified <paramref name="distance"/>.
        /// </summary>
        /// <param name="speed">The speed of the motor.</param>
        /// <param name="movement">The direction to start.</param>
        /// <param name="distance">The distance to run.</param>
        public void MotorRunDistance(Speed speed, Movement movement, short distance)
        {
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
        }

        public void Dispose()
        {
        }
    }
}