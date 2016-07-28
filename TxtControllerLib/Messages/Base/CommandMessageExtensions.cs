using System;
using System.IO;
using JetBrains.Annotations;

namespace artiso.Fischertechnik.TxtController.Lib.Messages.Base
{
    public static class CommandMessageExtensions
    {
        public static CommandMessage AddProperty([NotNull] this CommandMessage commandMessage, [NotNull] string name,
            [NotNull] Action<Stream> writeValueAction)
        {
            commandMessage.SerializationProperties.Add(new PropertySerializationInfo(name, writeValueAction));
            return commandMessage;
        }
    }
}