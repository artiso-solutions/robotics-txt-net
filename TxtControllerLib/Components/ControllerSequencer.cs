using System;
using System.Net;
using System.Reactive.Linq;
using System.Threading.Tasks;
using RoboticsTxt.Lib.Commands;
using RoboticsTxt.Lib.Contracts;

namespace RoboticsTxt.Lib.Components
{
    /// <summary>
    /// The <see cref="ControllerSequencer"/> provides high level methods for operation of the Fischertechnik ROBOTICS TXT controller.
    /// This includes the operation of motors and inputs of any kind.
    /// </summary>
    /// <remarks>
    /// The operations provided by the <see cref="ControllerSequencer"/> are not implemented completely. More operations will follow...
    /// </remarks>
    public class ControllerSequencer : IDisposable
    {
        private readonly ControllerCommunicator controllerCommunicator;

        /// <summary>
        /// Creates a new instance of the <see cref="ControllerSequencer"/> and starts the communication with the controller. To stop the communication
        /// you have to dispose the <see cref="ControllerSequencer"/>.
        /// </summary>
        /// <param name="ipAddress"></param>
        public ControllerSequencer(IPAddress ipAddress)
        {
            this.controllerCommunicator = new ControllerCommunicator(ipAddress);

            this.controllerCommunicator.Start();
        }

        /// <summary>
        /// Starts the specified <paramref name="motor"/> immediately.
        /// </summary>
        /// <param name="motor">The motor to start.</param>
        /// <param name="speed">The speed of the motor.</param>
        /// <param name="movement">The direction to start.</param>
        public void StartMotor(Motor motor, Speed speed, Movement movement)
        {
            this.controllerCommunicator.QueueCommand(new StartMotorCommand(motor, speed, movement));
        }

        /// <summary>
        /// Starts the specified <paramref name="motor"/> immediately.
        /// </summary>
        /// <param name="motor">The motor to start.</param>
        /// <param name="speed">The speed of the motor.</param>
        /// <param name="movement">The direction to start.</param>
        public void MotorRunDistance(Motor motor, Speed speed, Movement movement, short distance)
        {
            controllerCommunicator.QueueCommand(new MotorRunDistanceCommand(motor, speed, movement, distance));
        }

        /// <summary>
        /// Stops the specified <paramref name="motor"/> immediately.
        /// </summary>
        /// <param name="motor">The motor to stop.</param>
        public void StopMotor(Motor motor)
        {
            this.controllerCommunicator.QueueCommand(new StopMotorCommand(motor));
        }

        /// <summary>
        /// Starts the specified <paramref name="motor"/> and stops it on state trigger of the specified <paramref name="digitalInput"/>.
        /// </summary>
        /// <param name="motor">The motor to start.</param>
        /// <param name="speed">The speed of the motor.</param>
        /// <param name="movement">The direction to start.</param>
        /// <param name="digitalInput">The digital input to trigger the stop.</param>
        /// <param name="expectedInputState">The expected value for the state trigger.</param>
        /// <returns>This method is async. The returned task will be completed as soon as the movement is finished.</returns>
        public async Task StartMotorStopWithDigitalInputAsync(Motor motor, Speed speed, Movement movement, DigitalInput digitalInput, bool expectedInputState)
        {
            this.StartMotor(motor, speed, movement);
            await this.WaitForInputAsync(digitalInput, expectedInputState);
            this.StopMotor(motor);
        }

        /// <summary>
        /// Starts the specified <paramref name="motor"/> and stops it after the given time span <paramref name="stopAfterTimeSpan"/>.
        /// </summary>
        /// <param name="motor">The motor to start.</param>
        /// <param name="speed">The speed of the motor.</param>
        /// <param name="movement">The direction to start.</param>
        /// <param name="stopAfterTimeSpan">The time span which is used to stop the motor again.</param>
        /// <returns>This method is async. The returned task will be completed as soon as the movement is finished.</returns>
        public async Task StartMotorStopAfterAsync(Motor motor, Speed speed, Movement movement, TimeSpan stopAfterTimeSpan)
        {
            this.StartMotor(motor, speed, movement);
            await Task.Delay(stopAfterTimeSpan);
            this.StopMotor(motor);
        }

        private async Task WaitForInputAsync(DigitalInput digitalInput, bool expectedValue)
        {
            await this.controllerCommunicator.UniversalInputs[(int)digitalInput].StateChanges.FirstAsync(b => b == expectedValue);
        }
        
        /// <summary>
        /// Cleanup of all resrouces. This also stops the communication to the controller.
        /// </summary>
        public void Dispose()
        {
            this.controllerCommunicator.Stop();
        }

        /// <summary>
        /// Retrieves the current input state of the specified <paramref name="referenceInput"/>.
        /// </summary>
        /// <param name="referenceInput">The digital input to get the state from.</param>
        /// <returns><c>true</c> if the input is triggered, otherwise <c>false</c>.</returns>
        public bool GetDigitalInputState(DigitalInput referenceInput)
        {
            return this.controllerCommunicator.UniversalInputs[(int)referenceInput].CurrentState;
        }
    }
}