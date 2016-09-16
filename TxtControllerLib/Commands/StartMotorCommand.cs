using System;
using JetBrains.Annotations;
using RoboticsTxt.Lib.Contracts;
using RoboticsTxt.Lib.Interfaces;
using RoboticsTxt.Lib.Messages;

namespace RoboticsTxt.Lib.Commands
{
    internal class StartMotorCommand : IControllerCommand
    {
        private readonly Speed speed;
        private readonly Movement movement;

        public StartMotorCommand(Motor motor, Speed speed, Movement movement)
        {
            this.Motor = motor;
            this.speed = speed;
            this.movement = movement;
        }

        public Motor Motor { get; }

        public void ApplyMessageChanges([NotNull] ExchangeDataCommandMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            var motorIndex = (int) this.Motor;
            var speedValue = (short) this.speed;

            switch (this.movement)
            {
                case Movement.Left:
                    message.PwmOutputValues[2*motorIndex] = speedValue;
                    message.PwmOutputValues[2*motorIndex + 1] = 0;
                    break;
                case Movement.Right:
                    message.PwmOutputValues[2*motorIndex] = 0;
                    message.PwmOutputValues[2*motorIndex + 1] = speedValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            message.MotorCommandId[motorIndex]++;
        }
    }
}
