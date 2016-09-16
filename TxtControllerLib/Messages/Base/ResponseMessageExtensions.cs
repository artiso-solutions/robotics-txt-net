using System;
using JetBrains.Annotations;

namespace RoboticsTxt.Lib.Messages.Base
{
    internal static class ResponseMessageExtensions
    {
        public static ResponseMessage AddProperty([NotNull] this ResponseMessage commandMessage, [NotNull] string name,
            [NotNull] Action<DeserializationContext> readValue)
        {
            commandMessage.DeserializationProperties.Add(new PropertyDeserializationInfo(name, readValue));
            return commandMessage;
        }
    }
}