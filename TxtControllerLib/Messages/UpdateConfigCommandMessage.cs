using System.Linq;
using RoboticsTxt.Lib.Configuration;
using RoboticsTxt.Lib.Messages.Base;

namespace RoboticsTxt.Lib.Messages
{
    public class UpdateConfigCommandMessage : CommandMessage
    {
        public UpdateConfigCommandMessage() : base(CommandIds.SendUpdateConfig, CommandIds.ReceiveUpdateConfig)
        {
            this.AddProperty("ConfigId", stream => ArchiveWriter.WriteInt16(stream, this.ConfigId))
                .AddProperty("ExtensionId", stream => ArchiveWriter.WriteInt16(stream, this.ExtensionId))
                .AddProperty("DummyTxOnly", stream => ArchiveWriter.WriteInt32(stream, 0))
                .AddProperty("MotorConfiguration", stream => ArchiveWriter.WriteBytes(stream, this.MotorModes.Select(motorMode => (byte)motorMode).ToArray()))
                .AddProperty("InputConfiguration", stream => ArchiveWriter.WriteBytes(stream, this.InputConfigurations.SelectMany(ic => new byte[] { (byte)ic.InputMode, (byte)(ic.IsDigital ? 1 : 0), 0, 0 }).ToArray()))
                .AddProperty("CounterConfiguration", stream => ArchiveWriter.WriteBytes(stream, this.CounterModes.SelectMany(cm => new byte[] { (byte)cm, 0, 0, 0 }).ToArray()))
                .AddProperty("DummyMotorConfig", stream => ArchiveWriter.WriteBytes(stream, new byte[32]));
        }

        /// <summary>
        /// A configuration id counter which starts at 0 and needs to be incremented on each change of configuration. 
        /// An update config command is ignored, if the m_config_id does not change. 
        /// The first configuration usually has configId=1.
        /// </summary>
        public ushort ConfigId { get; set; }

        /// <summary>
        /// 0 for master, 1 for extension (or higher numbers when more extensions are supported by the TXT firmware)
        /// </summary>
        public short ExtensionId { get; set; }

        /// <summary>
        /// Configuration of motors
        /// 0=single output O1/O2, 1=motor output M1
        /// </summary>
        public MotorMode[] MotorModes { get; set; }

        /// <summary>
        /// Configuration of the inputs
        /// </summary>
        public InputConfiguration[] InputConfigurations { get; set; }

        /// <summary>
        /// Mode of the counters
        /// 1=normal, 0=inverted
        /// </summary>
        public CounterMode[] CounterModes { get; set; }
    }
}