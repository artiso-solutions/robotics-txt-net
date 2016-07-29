using System;
using artiso.Fischertechnik.TxtController.Lib.Messages.Base;

namespace artiso.Fischertechnik.TxtController.Lib.Messages
{
    public class QueryStatusResponseMessage : ResponseMessage
    {
        public QueryStatusResponseMessage() : base(CommandIds.ReceiveQueryStatus)
        {
            this.AddProperty("Name", dc => Name = ArchiveReader.ReadString(dc, 16))
                .AddProperty("Version", dc => Version = ArchiveReader.ReadVersion(dc));
        }

        /// <summary>
        /// Name of the TXT controller
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Version code of the TXT Firmware
        /// </summary>
        public Version Version { get; private set; }
    }
}