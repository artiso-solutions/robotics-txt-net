using System;
using artiso.Fischertechnik.TxtController.Lib.Interfaces;
using artiso.Fischertechnik.TxtController.Lib.Messages;
using JetBrains.Annotations;

namespace artiso.Fischertechnik.TxtController.Lib.Components
{
    public class CommandProcessor
    {
        public void ProcessControllerCommand([NotNull] IControllerCommand command, [NotNull] ExchangeDataCommandMessage currentCommandMessage, DigitalInputInfo[] universalInputs)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (currentCommandMessage == null) throw new ArgumentNullException(nameof(currentCommandMessage));

            command.ApplyMessageChanges(currentCommandMessage);
        }
    }
}
