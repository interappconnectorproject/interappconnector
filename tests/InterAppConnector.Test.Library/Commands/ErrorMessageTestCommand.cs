using InterAppConnector.Attributes;
using InterAppConnector.DataModels;
using InterAppConnector.Interfaces;

namespace InterAppConnector.Test.Library.Commands
{
    [Command("errormessage")]
    public class ErrorMessageTestCommand : ICommand<EmptyDataModel>
    {
        public string Main(EmptyDataModel arguments)
        {
            CommandOutput.Info("Starting error message command");
            return CommandOutput.Error("This command is created in order to test the error message, so you can ignore this message if you want");
        }
    }
}
