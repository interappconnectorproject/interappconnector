using InterAppConnector.Attributes;
using InterAppConnector.Interfaces;
using InterAppConnector.Test.Library.DataModels;

namespace InterAppConnector.Test.Library.Commands
{
    [Command("wrongvalidator")]
    public class WrongValidatorCommand : ICommand<WrongValidatorDataModel>
    {
        public string Main(WrongValidatorDataModel arguments)
        {
            return CommandOutput.Error("You should not see this error message");
        }
    }
}
