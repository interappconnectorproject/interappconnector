using InterAppConnector.Attributes;
using InterAppConnector.Enumerations;
using System.ComponentModel;

namespace InterAppConnector.Test.Library.DataModels
{
    public class BatchCommandDataModel
    {
        [Alias("messagetype")]
        [Description("Define the status of the message to return")]
        public CommandExecutionMessageType ExecutionMessageType { get; set; }
    }
}
