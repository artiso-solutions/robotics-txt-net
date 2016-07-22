using artiso.Fischertechnik.RoboTxt.Lib.Messages.Base;

namespace artiso.Fischertechnik.RoboTxt.Lib.Messages
{
    public class StopOnlineCommandMessage : CommandMessage
    {
        public StopOnlineCommandMessage() : base(CommandIds.SendStopOnline, CommandIds.ReceiveStopOnline)
        {
        }
    }
}