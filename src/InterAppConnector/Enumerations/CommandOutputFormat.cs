using System.ComponentModel;

namespace InterAppConnector.Enumerations
{
    /// <summary>
    /// Define the output of the command
    /// </summary>
    public enum CommandOutputFormat
    {
        /// <summary>
        /// Json format is used by applcations in order to exchange messages and status
        /// </summary>
        [Description("Json format is used by applications in order to exchange messages and status")]
        Json,

        /// <summary>
        /// Text format is used in order to give messages to the user
        /// </summary>
        [Description("Text format is used in order to give messages to the user")]
        Text
    }
}
