using InterAppConnector.Attributes;

namespace InterAppConnector.Enumerations
{
    /// <summary>
    /// The type of the message. It is used to contain the status of an application
    /// </summary>
    public enum CommandExecutionMessageType
    {
        /// <summary>
        /// An undefined value. This value must only be used internally in the class, as every message
        /// returned ny the application must return a significative execution status
        /// </summary>
        [ExcludeItemFromCommandAttribute]
        Undefined,

        /// <summary>
        /// All operations have been completed successfully
        /// </summary>
        Success,

        /// <summary>
        /// There are some operations that have not been completed successfully or there are operations that are not necessary to complete
        /// a specific operation. It may not block the program execution
        /// </summary>
        Warning,

        /// <summary>
        /// An informational message
        /// </summary>
        Info,

        /// <summary>
        /// A fatal error. The operation should be blocked 
        /// </summary>
        Failed,
    }
}
