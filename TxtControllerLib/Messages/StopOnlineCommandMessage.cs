using artiso.Fischertechnik.TxtController.Lib.Messages.Base;

namespace artiso.Fischertechnik.TxtController.Lib.Messages
{
    public class StopOnlineCommandMessage : CommandMessage
    {
        public StopOnlineCommandMessage() : base(CommandIds.SendStopOnline, CommandIds.ReceiveStopOnline)
        {
        }
    }
}