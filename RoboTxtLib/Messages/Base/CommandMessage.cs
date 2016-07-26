using System.Collections.Generic;

namespace artiso.Fischertechnik.TxtController.Lib.Messages.Base
{
    public abstract class CommandMessage : ControllerMessage
    {
        public CommandMessage(uint commandId, uint expectedResponseId) : base(commandId)
        {
            ExpectedResponseId = expectedResponseId;

            SerializationProperties = new List<PropertySerializationInfo>();
            this.AddProperty("CommandId", s => ArchiveWriter.WriteInt32(s, CommandId));
        }

        public uint ExpectedResponseId { get; }

        public List<PropertySerializationInfo> SerializationProperties { get; }
    }
}