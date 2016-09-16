using System;
using JetBrains.Annotations;
using RoboticsTxt.Lib.Interfaces;
using RoboticsTxt.Lib.Messages;

namespace RoboticsTxt.Lib.Components
{
    internal class CommandProcessor
    {
        public void ProcessControllerCommand([NotNull] IControllerCommand command, [NotNull] ExchangeDataCommandMessage currentCommandMessage, DigitalInputInfo[] universalInputs)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (currentCommandMessage == null) throw new ArgumentNullException(nameof(currentCommandMessage));

            command.ApplyMessageChanges(currentCommandMessage);
        }
    }
}
