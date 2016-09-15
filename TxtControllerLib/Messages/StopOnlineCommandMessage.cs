using RoboticsTxt.Lib.Messages.Base;

namespace RoboticsTxt.Lib.Messages
{
    public class StopOnlineCommandMessage : CommandMessage
    {
        public StopOnlineCommandMessage() : base(CommandIds.SendStopOnline, CommandIds.ReceiveStopOnline)
        {
        }
    }
}