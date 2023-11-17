using InterAppConnector.Attributes;
using InterAppConnector.Enumerations;
using InterAppConnector.Interfaces;
using InterAppConnector.Test.Library.DataModels;

namespace InterAppConnector.Test.Library.Commands
{
    [Command("testbatch", Description = "Tests for batch command")]
    public class BatchTestCommand : ICommand<BatchCommandDataModel>
    {
        public string Main(BatchCommandDataModel arguments)
        {
            string message = CommandOutput.Error("Value not recognized");

            switch (arguments.ExecutionMessageType)
            {
                case CommandExecutionMessageType.Success:
                    message = CommandOutput.Ok("Operation completed successfully");
                    break;
                case CommandExecutionMessageType.Warning:
                    message = CommandOutput.Warning("Operation completed with a warning");
                    break;
                case CommandExecutionMessageType.Info:
                    message = CommandOutput.Info("An informational message");
                    break;
                case CommandExecutionMessageType.Failed:
                    message = CommandOutput.Error("Operation failed");
                    break;
                default:
                    break;
            }

            return message;
        }
    }
}
