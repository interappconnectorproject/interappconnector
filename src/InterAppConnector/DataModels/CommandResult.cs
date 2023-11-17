using InterAppConnector.Enumerations;

namespace InterAppConnector.DataModels
{
    /// <summary>
    /// Contains the result of the execution
    /// </summary>
    /// <typeparam name="ResultType"></typeparam>
    public class CommandResult<ResultType>
    {
        /// <summary>
        /// The status of the message
        /// </summary>
        public CommandExecutionMessageType MessageStatus { get; set; } = CommandExecutionMessageType.Undefined;

        /// <summary>
        /// The fully qualified name of the type of the message
        /// </summary>
        public string MessageType
        {
            get
            {
                return typeof(ResultType).FullName;
            }
        }

        /// <summary>
        /// The message
        /// </summary>
        public ResultType Message { get; set; }
    }
}
