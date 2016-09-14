using System;
using artiso.Fischertechnik.TxtController.Lib.Contracts;
using artiso.Fischertechnik.TxtController.Lib.Interfaces;
using artiso.Fischertechnik.TxtController.Lib.Messages;
using JetBrains.Annotations;

namespace artiso.Fischertechnik.TxtController.Lib.Commands
{
    public class StopMotorCommand : IControllerCommand
    {
        private readonly Motor motor;

        public StopMotorCommand(Motor motor)
        {
            this.motor = motor;
        }

        public void ApplyMessageChanges([NotNull] ExchangeDataCommandMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            var motorIndex = (int) this.motor;

            message.PwmOutputValues[motorIndex] = 0;
            message.PwmOutputValues[motorIndex + 1] = 0;

            message.MotorDistance[motorIndex] = 0;

            message.MotorCommandId[motorIndex]++;
        }
    }
}
