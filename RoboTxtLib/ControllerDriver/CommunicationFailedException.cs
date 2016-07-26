using System;
using System.Runtime.Serialization;

namespace artiso.Fischertechnik.TxtController.Lib.ControllerDriver
{
    public class CommunicationFailedException : Exception
    {
        public CommunicationFailedException()
        {
        }

        public CommunicationFailedException(string message) : base(message)
        {
        }

        public CommunicationFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CommunicationFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}