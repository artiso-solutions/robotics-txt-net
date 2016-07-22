namespace artiso.Fischertechnik.RoboTxt.Lib.Messages.Base
{
    public abstract class ControllerMessage
    {
        protected ControllerMessage()
        {
        }

        protected ControllerMessage(uint commandId)
        {
            CommandId = commandId;
        }

        public uint CommandId { get; protected set; }
    }
}