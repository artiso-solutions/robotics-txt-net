namespace RoboticsTxt.Lib.Messages.Base
{
    internal abstract class ControllerMessage
    {
        protected ControllerMessage()
        {
        }

        protected ControllerMessage(uint commandId)
        {
            this.CommandId = commandId;
        }

        public uint CommandId { get; protected set; }
    }
}