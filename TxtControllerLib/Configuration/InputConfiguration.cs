namespace RoboticsTxt.Lib.Configuration
{
    /// <summary>
    /// Specifies the input configuration for an input port.
    /// </summary>
    public class InputConfiguration
    {
        /// <summary>
        /// The mode if the input.
        /// </summary>
        public InputMode InputMode { get; set; }

        /// <summary>
        /// Specifies wether the input is digital or analog.
        /// </summary>
        public bool IsDigital { get; set; }
    }
}