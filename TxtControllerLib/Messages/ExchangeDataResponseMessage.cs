using RoboticsTxt.Lib.Messages.Base;

namespace RoboticsTxt.Lib.Messages
{
    internal class ExchangeDataResponseMessage : ResponseMessage
    {
        public ExchangeDataResponseMessage() : base(CommandIds.ReceiveExchangeData)
        {
            this.UniversalInputs = new short[8];
            this.CounterInput = new short[4];
            this.CounterValue = new short[4];

            this.AddProperty(nameof(UniversalInputs), dc => this.UniversalInputs = ArchiveReader.ReadInt16(dc, 8))
                .AddProperty(nameof(CounterInput), dc => this.CounterInput = ArchiveReader.ReadInt16(dc, 4))
                .AddProperty(nameof(CounterValue), dc => this.CounterValue = ArchiveReader.ReadInt16(dc, 4))
                .AddProperty(nameof(CounterCommandId), dc => this.CounterCommandId = ArchiveReader.ReadInt16(dc))
                .AddProperty(nameof(MotorCommandId), dc => this.MotorCommandId = ArchiveReader.ReadInt16(dc))
                .AddProperty(nameof(SoundCommandId), dc => this.SoundCommandId = ArchiveReader.ReadInt16(dc));
        }

        /// <summary>
        /// Values of the 8 universal inputs. Depending on the configuration this is either a analog value or 0 or 1 for digital inputs.
        /// </summary>
        public short[] UniversalInputs { get; private set; }

        /// <summary>
        /// The current values (0 or 1) of the 4 counter inputs. 
        /// </summary>
        public short[] CounterInput { get; private set; }

        /// <summary>
        /// The count value of the 4 counter inputs. The counter value can be reset by incrementing m_counter_reset_command_id.
        /// </summary>
        public short[] CounterValue { get; private set; }

        /// <summary>
        /// This value changes to the last m_counter_reset_command_id in the command structure after a reset command finished.
        /// </summary>
        public short CounterCommandId { get; private set; }

        /// <summary>
        /// This value changes to the last m_motor_command_id in the command structure after a motor distance command is finished
        /// </summary>
        public short MotorCommandId { get; private set; }

        /// <summary>
        /// This value changes to the last m_sound_command_id in the command structure after a sound playback finished.
        /// </summary>
        public short SoundCommandId { get; private set; }

        /// <summary>
        /// This array of structures contains the infrared remote control input values. 
        /// The values are given once for each combination of switches, so that up to 4 remote controls can be used. 
        /// The 5th structure (with index 4) responds to a control with any switch setting. 
        /// Please note, that the switch states are also submitted, but not when the switch changes. 
        /// They are only submitted when some other value changes.
        /// </summary>
        public short[] InfraRed { get; private set; }
    }
}