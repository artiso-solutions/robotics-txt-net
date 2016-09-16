using RoboticsTxt.Lib.Messages.Base;

namespace RoboticsTxt.Lib.Messages
{
    internal class QueryStatusCommandMessage : CommandMessage
    {
        public QueryStatusCommandMessage() : base(CommandIds.SendQueryStatus, CommandIds.ReceiveQueryStatus)
        {
        }
    }
}