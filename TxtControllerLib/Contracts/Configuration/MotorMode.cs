namespace RoboticsTxt.Lib.Contracts.Configuration
{
    /// <summary>
    /// Specifies the motor mode.
    /// </summary>
    public enum MotorMode
    {
        /// <summary>
        /// Two voltages for O1 and O2
        /// </summary>
        O1O2 = 0,
        /// <summary>
        /// Only one speed. ATTENTION: Not supported at this time.
        /// </summary>
        M1 = 1
    }
}