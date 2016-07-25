using System.Linq;
using artiso.Fischertechnik.RoboTxt.Lib.Configuration;
using artiso.Fischertechnik.RoboTxt.Lib.Messages.Base;

namespace artiso.Fischertechnik.RoboTxt.Lib.Messages
{
    public class UpdateConfigCommandMessage : CommandMessage
    {
        public UpdateConfigCommandMessage() : base(CommandIds.SendUpdateConfig, CommandIds.ReceiveUpdateConfig)
        {
            this.AddProperty("UpdateConfigSequence", stream => ArchiveWriter.WriteInt16(stream, UpdateConfigSequence))
                .AddProperty("Dummy1", stream => ArchiveWriter.WriteInt16(stream, 0))
                .AddProperty("DummyTxOnly", stream => ArchiveWriter.WriteInt32(stream, 0))
                .AddProperty("MotorConfiguration", stream => ArchiveWriter.WriteBytes(stream, MotorModes.Select(motorMode => (byte)motorMode).ToArray()))
                .AddProperty("InputConfiguration", stream => ArchiveWriter.WriteBytes(stream, InputConfigurations.SelectMany(ic => new byte[] { (byte)ic.InputMode, (byte)(ic.IsDigital ? 1 : 0), 0, 0 }).ToArray()))
                .AddProperty("CounterConfiguration", stream => ArchiveWriter.WriteBytes(stream, CounterModes.SelectMany(cm => new byte[] { (byte)cm, 0, 0, 0 }).ToArray()))
                .AddProperty("DummyMotorConfig", stream => ArchiveWriter.WriteBytes(stream, new byte[32]));
        }

        public ushort UpdateConfigSequence { get; set; }

        public MotorMode[] MotorModes { get; set; }

        public InputConfiguration[] InputConfigurations { get; set; }

        public CounterMode[] CounterModes { get; set; }
    }
}