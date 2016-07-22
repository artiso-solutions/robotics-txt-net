using System;
using artiso.Fischertechnik.RoboTxt.Lib.Messages.Base;

namespace artiso.Fischertechnik.RoboTxt.Lib.Messages
{
    public class QueryStatusResponseMessage : ResponseMessage
    {
        public QueryStatusResponseMessage() : base(CommandIds.ReceiveQueryStatus)
        {
            this.AddProperty("Name", dc => Name = ArchiveReader.ReadString(dc, 16))
                .AddProperty("Version", dc => Version = ArchiveReader.ReadVersion(dc));
        }

        public string Name { get; private set; }
        public Version Version { get; private set; }
    }
}