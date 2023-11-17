using InterAppConnector.Attributes;
using InterAppConnector.DataModels;
using InterAppConnector.Interfaces;

namespace InterAppConnector.Test.Library.Commands
{
    [Command("warningmessage")]
    public class WarningMessageTestCommand : ICommand<EmptyDataModel>
    {
        public string Main(EmptyDataModel arguments)
        {
            CommandOutput.Info("Starting warning message command");
            return CommandOutput.Warning("This command is created in order to test the warning message, so you can ignore this message if you want");
        }
    }
}
