using System;

namespace RoboticsTxt.Lib.Contracts.Configuration
{
    /// <summary>
    /// Provides some basic configuration parameters for the communication with the ROBOTICS TXT controller.
    /// </summary>
    public class ControllerConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerConfiguration"/> class.
        /// </summary>
        public ControllerConfiguration()
        {
            CommunicationCycleTime = TimeSpan.FromMilliseconds(10);
        }

        /// <summary>
        /// Gets or sets the communication cycle time. The default value is 10 ms.
        /// </summary>
        public TimeSpan CommunicationCycleTime { get; set; }

        /// <summary>
        /// Gets or sets the motor modes. The default value is O1O2 for all motors.
        /// </summary>
        public MotorMode[] MotorModes { get; set; }

        /// <summary>
        /// Gets or sets the counter modes. The default value is Normal for all motors.
        /// </summary>
        public CounterMode[] CounterModes { get; set; }

        /// <summary>
        /// Gets or sets the input configurations. By default Resistance-inputs are configured.
        /// </summary>
        public InputConfiguration[] InputConfigurations { get; set; }
    }
}