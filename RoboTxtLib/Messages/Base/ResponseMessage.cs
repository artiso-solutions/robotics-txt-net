using System.Collections.Generic;
using artiso.Fischertechnik.RoboTxt.Lib.ControllerDriver;

namespace artiso.Fischertechnik.RoboTxt.Lib.Messages.Base
{
    public class ResponseMessage : ControllerMessage
    {
        public ResponseMessage(uint commandId)
        {
            DeserializationProperties = new List<PropertyDeserializationInfo>();
            this.AddProperty("CommandId", dc =>
            {
                var responseCommandId = ArchiveReader.ReadUInt32(dc);
                if (responseCommandId != commandId)
                {
                    throw new CommunicationFailedException(
                        $"Did not receive expected respone id {commandId}. Received response message id {responseCommandId} instead.");
                }

                CommandId = responseCommandId;
            });
        }

        public List<PropertyDeserializationInfo> DeserializationProperties { get; }
    }
}