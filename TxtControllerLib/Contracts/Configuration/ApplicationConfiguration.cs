namespace RoboticsTxt.Lib.Contracts.Configuration
{
    /// <summary>
    /// Provides basic configuration for the application the ROBOTICS TXT controller is used in.
    /// </summary>
    public class ApplicationConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationConfiguration"/> class.
        /// </summary>
        public ApplicationConfiguration()
        {
            ApplicationName = "Generic";
        }


        /// <summary>
        /// Gets or sets the name of the application. This will be used e.g. to to load and store position data.
        /// The default value is "Generic".
        /// </summary>
        public string ApplicationName { get; set; }
    }
}