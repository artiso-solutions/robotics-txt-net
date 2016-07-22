using artiso.Fischertechnik.RoboTxt.Lib.Messages.Base;

namespace artiso.Fischertechnik.RoboTxt.Lib.Messages
{
    public class QueryStatusCommandMessage : CommandMessage
    {
        public QueryStatusCommandMessage() : base(CommandIds.SendQueryStatus, CommandIds.ReceiveQueryStatus)
        {
        }
    }
}