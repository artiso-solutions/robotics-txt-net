using System;
using JetBrains.Annotations;

namespace artiso.Fischertechnik.RoboTxt.Lib.Messages.Base
{
    public static class ResponseMessageExtensions
    {
        public static ResponseMessage AddProperty([NotNull] this ResponseMessage commandMessage, [NotNull] string name,
            [NotNull] Action<DeserializationContext> readValue)
        {
            commandMessage.DeserializationProperties.Add(new PropertyDeserializationInfo(name, readValue));
            return commandMessage;
        }
    }
}