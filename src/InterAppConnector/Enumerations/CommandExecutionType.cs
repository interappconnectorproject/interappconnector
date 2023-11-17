namespace InterAppConnector.Enumerations
{
    /// <summary>
    /// Define the mode to be used in rder to launch the command
    /// </summary>
    public enum CommandExecutionType
    {
        /// <summary>
        /// A batch execution is used between applications in order to exchange data and status.
        /// It is not intended to use with the user. 
        /// </summary>
        Batch,

        /// <summary>
        /// An interactive execution is used when the command is launched by the user.
        /// It provides friendly messages to the user. 
        /// </summary>
        Interactive
    }
}
