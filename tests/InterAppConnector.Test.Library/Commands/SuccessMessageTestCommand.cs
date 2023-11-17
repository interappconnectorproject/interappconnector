using InterAppConnector.Attributes;
using InterAppConnector.DataModels;
using InterAppConnector.Interfaces;

namespace InterAppConnector.Test.Library.Commands
{
    [Command("successmessage")]
    public class SuccessMessageTestCommand : ICommand<EmptyDataModel>
    {
        public string Main(EmptyDataModel arguments)
        {
            CommandOutput.Info("Starting success message command");
            return CommandOutput.Ok("Operation completed successfully");
        }
    }
}
