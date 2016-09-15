using RoboticsTxt.Lib.Messages.Base;

namespace RoboticsTxt.Lib.Messages
{
    public class QueryStatusCommandMessage : CommandMessage
    {
        public QueryStatusCommandMessage() : base(CommandIds.SendQueryStatus, CommandIds.ReceiveQueryStatus)
        {
        }
    }
}