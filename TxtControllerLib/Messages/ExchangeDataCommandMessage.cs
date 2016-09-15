using RoboticsTxt.Lib.Messages.Base;

namespace RoboticsTxt.Lib.Messages
{
    public class ExchangeDataCommandMessage : CommandMessage
    {
        public ExchangeDataCommandMessage() : base(CommandIds.SendExchangeData, CommandIds.ReceiveExchangeData)
        {
            this.PwmOutputValues = new short[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            this.MotorMaster = new short[] { 0, 0, 0, 0 };
            this.MotorDistance = new short[] { 0, 0, 0, 0 };
            this.MotorCommandId = new short[] { 0, 0, 0, 0 };
            this.CounterResetCommandId = new short[] { 0, 0, 0, 0 };
            this.SoundCommandId = 0;
            this.SoundIndex = 0;
            this.SoundRepeat = 0;

            this.AddProperty("PwmOutputValues", stream => ArchiveWriter.WriteInt16(stream, this.PwmOutputValues))
                .AddProperty("MotorMaster", stream => ArchiveWriter.WriteInt16(stream, this.MotorMaster))
                .AddProperty("MotorDistance", stream => ArchiveWriter.WriteInt16(stream, this.MotorDistance))
                .AddProperty("MotorCommandId", stream => ArchiveWriter.WriteInt16(stream, this.MotorCommandId))
                .AddProperty("CounterResetCommandId", stream => ArchiveWriter.WriteInt16(stream, this.CounterResetCommandId))
                .AddProperty("SoundCommandId", stream => ArchiveWriter.WriteInt16(stream, this.SoundCommandId))
                .AddProperty("SoundIndex", stream => ArchiveWriter.WriteInt16(stream, this.SoundIndex))
                .AddProperty("SoundRepeat", stream => ArchiveWriter.WriteInt16(stream, this.SoundRepeat))
                .AddProperty("Empty", stream => ArchiveWriter.WriteBytes(stream, new byte[2]));
        }

        /// <summary>
        /// The (PWM) pulse with modulation values between 0 and 512 for the 8 outputs. A motor output uses two consecutive values (M1 uses index 0 and 1). 
        /// If the output is used as motor output, always one of the 2 outputs is 0, while the other value is between 0 and 512.
        /// </summary>
        public short[] PwmOutputValues { get; set; }

        /// <summary>
        /// This is used to synchronize one motor to another motor. 0 means the motor is independent. 1..4 means the motor is synchronized to motor 1..4. 
        /// If a value of 5..8 is given, it is possible to program deviations from the synchronization using the m_motor_distance values. This is called “sync error injection”. 
        /// This is useful e.g. for closed loop trail tracking. If this value is changed, m_motor_command_id must be incremented.
        /// </summary>
        public short[] MotorMaster { get; set; }

        /// <summary>
        /// If this value is not 0, the motor will stop after the corresponding counter input counted the given number of pulses. 
        /// In sync error injection mode, this is used as described above. If this value is changed, m_motor_command_id must be incremented. 
        /// Distance commands automatically reset the counter.
        /// </summary>
        public short[] MotorDistance { get; set; }

        /// <summary>
        /// This value needs to be incremented whenever m_motor_master or m_motor_distance change. 
        /// m_pwmOutputValues can be changed without incrementing the command id. 
        /// A distance command is finished, if the m_motor_command_id in the response structure has the same value.
        /// </summary>
        public short[] MotorCommandId { get; set; }

        /// <summary>
        /// If this values is incremented, the corresponding counter is reset. 
        /// The reset is finished, if m_counter_command_id in the response structure has the same value.
        /// </summary>
        public short[] CounterResetCommandId { get; set; }

        /// <summary>
        /// This value must be incremented, whenever m_sound_index or m_sound_repeat change, or to play the same sound again.
        /// </summary>
        public ushort SoundCommandId { get; set; }

        /// <summary>
        /// Index of the sound to play, 0=no sound. 
        /// Please note that it is possible to exchange the sound files on the TXT filesystem in folder /opt/knobloch/SoundFiles using ssh and/or scp. 
        /// The file name must start with a 2 digit number stating the sound index. The sound files are 8-bit mono 22050 Hz and can be created e.g. with Audacity.
        /// The following list shows the default sounds with the index.
        /// 01_Airplane.wav 
        /// 02_Alarm.wav 
        /// 03_Bell.wav 
        /// 04_Braking.wav 
        /// 05_Car-horn-long.wav 
        /// 06_Car-horn-short.wav 
        /// 07_Crackling-wood.wav 
        /// 08_Excavator.wav 
        /// 09_Fantasy-1.wav 
        /// 10_Fantasy-2.wav 
        /// 11_Fantasy-3.wav 
        /// 12_Fantasy-4.wav 
        /// 13_Farm.wav 
        /// 14_Fire-department.wav 
        /// 15_Fire-noises.wav 
        /// 16_Formula1.wav 
        /// 17_Helicopter.wav 
        /// 18_Hydraulic.wav 
        /// 19_Motor-sound.wav 
        /// 20_Motor-starting.wav 
        /// 21_Propeller-airplane.wav 
        /// 22_Roller-coaster.wav 
        /// 23_Ships-horn.wav 
        /// 24_Tractor.wav 
        /// 25_Truck.wav 
        /// 26_Augenzwinkern.wav 
        /// 27_Fahrgeraeusch.wav 
        /// 28_Kopf_heben.wav 
        /// 29_Kopf_neigen.wav
        /// </summary>
        public ushort SoundIndex { get; set; }

        /// <summary>
        /// A repeat count for the sound. 0 means indefinite. Sounds can be stopped at any time by sending a new sound command with m_sound_index. 
        /// </summary>
        public ushort SoundRepeat { get; set; }
    }
}