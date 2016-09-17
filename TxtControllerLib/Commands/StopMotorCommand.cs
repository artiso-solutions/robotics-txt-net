using System;
using JetBrains.Annotations;
using RoboticsTxt.Lib.Contracts;
using RoboticsTxt.Lib.Interfaces;
using RoboticsTxt.Lib.Messages;

namespace RoboticsTxt.Lib.Commands
{
    internal class StopMotorCommand : IControllerCommand
    {
        private readonly Motor motor;

        public StopMotorCommand(Motor motor)
        {
            this.motor = motor;
        }

        public void ApplyMessageChanges([NotNull] ExchangeDataCommandMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            var motorIndex = (int)this.motor;

            message.PwmOutputValues[2 * motorIndex] = 0;
            message.PwmOutputValues[2 * motorIndex + 1] = 0;

            message.MotorDistance[motorIndex] = 0;

            message.MotorCommandId[motorIndex]++;
        }
    }
}
