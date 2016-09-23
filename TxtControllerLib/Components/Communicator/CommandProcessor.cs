using System;
using JetBrains.Annotations;
using RoboticsTxt.Lib.Interfaces;
using RoboticsTxt.Lib.Messages;

namespace RoboticsTxt.Lib.Components.Communicator
{
    internal class CommandProcessor
    {
        public void ProcessControllerCommand(ControllerCommunicator controllerCommunicator, [NotNull] IControllerCommand command, [NotNull] ExchangeDataCommandMessage currentCommandMessage)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (currentCommandMessage == null) throw new ArgumentNullException(nameof(currentCommandMessage));

            command.Execute(controllerCommunicator, currentCommandMessage);
        }
    }
}
