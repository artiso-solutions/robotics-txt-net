using artiso.Fischertechnik.TxtController.Lib.Messages.Base;

namespace artiso.Fischertechnik.TxtController.Lib.Messages
{
    public class QueryStatusCommandMessage : CommandMessage
    {
        public QueryStatusCommandMessage() : base(CommandIds.SendQueryStatus, CommandIds.ReceiveQueryStatus)
        {
        }
    }
}