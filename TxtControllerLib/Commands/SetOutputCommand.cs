using System;
using JetBrains.Annotations;
using RoboticsTxt.Lib.Components.Communicator;
using RoboticsTxt.Lib.Contracts;
using RoboticsTxt.Lib.Interfaces;
using RoboticsTxt.Lib.Messages;

namespace RoboticsTxt.Lib.Commands
{
    internal class SetOutputCommand : IControllerCommand
    {
        private readonly short outputValue;

        public SetOutputCommand(Output output, short outputValue)
        {
            if (outputValue < 0 && outputValue > 512)
            {
                throw new ArgumentOutOfRangeException("Output value ist not in range. (0 - 512)");
            }
            this.outputValue = outputValue;

            Output = output;
        }

        public Output Output { get; }

        public void Execute(ControllerCommunicator controllerCommunicator, [NotNull] ExchangeDataCommandMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            var outputIndex = (int)Output;
            message.PwmOutputValues[outputIndex] = outputValue;
        }
    }
}
