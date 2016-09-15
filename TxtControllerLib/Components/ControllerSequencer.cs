using System;
using System.Reactive.Linq;
using System.Threading;
using artiso.Fischertechnik.TxtController.Lib.Commands;
using artiso.Fischertechnik.TxtController.Lib.Contracts;

namespace artiso.Fischertechnik.TxtController.Lib.Components
{
    public class ControllerSequencer : IDisposable
    {
        private readonly ControllerCommunicator controllerCommunicator;

        public ControllerSequencer()
        {
            this.controllerCommunicator = new ControllerCommunicator();

            this.controllerCommunicator.Start();
        }

        public void StartMotor(Motor motor, Speed speed, Movement movement)
        {
            controllerCommunicator.QueueCommand(new StartMotorCommand(motor, speed, movement));
        }

        public void StopMotor(Motor motor)
        {
            controllerCommunicator.QueueCommand(new StopMotorCommand(motor));
        }

        public void StartMotorStopWithDigitalInput(Motor motor, Speed speed, Movement movement, DigitalInput digitalInput, bool expectedInputState)
        {
            StartMotor(motor, speed, movement);
            WaitForInput(digitalInput, expectedInputState);
            StopMotor(motor);
        }

        public void StartMotorStopAfter(Motor motor, Speed speed, Movement movement, TimeSpan stopAfterTimeSpan)
        {
            StartMotor(motor, speed, movement);
            Thread.Sleep(stopAfterTimeSpan);
            StopMotor(motor);
        }

        private void WaitForInput(DigitalInput digitalInput, bool expectedValue)
        {
            controllerCommunicator.UniversalInputs[(int)digitalInput].StateChanges.FirstAsync(b => b == expectedValue).Wait();
        }

        public void Dispose()
        {
            controllerCommunicator.Stop();
        }
    }
}