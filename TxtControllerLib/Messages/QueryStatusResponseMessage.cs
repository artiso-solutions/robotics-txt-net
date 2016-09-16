using System;
using RoboticsTxt.Lib.Messages.Base;

namespace RoboticsTxt.Lib.Messages
{
    internal class QueryStatusResponseMessage : ResponseMessage
    {
        public QueryStatusResponseMessage() : base(CommandIds.ReceiveQueryStatus)
        {
            this.AddProperty("Name", dc => this.Name = ArchiveReader.ReadString(dc, 16))
                .AddProperty("Version", dc => this.Version = ArchiveReader.ReadVersion(dc));
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