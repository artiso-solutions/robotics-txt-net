using artiso.Fischertechnik.TxtController.Lib.Messages.Base;

namespace artiso.Fischertechnik.TxtController.Lib.Messages
{
    public class StartOnlineCommandMessage : CommandMessage
    {
        public StartOnlineCommandMessage() : base(CommandIds.SendStartOnline, CommandIds.ReceiveStartOnline)
        {
            this.AddProperty("Text", stream => ArchiveWriter.WriteString(stream, "Online", 64));
        }
    }
}