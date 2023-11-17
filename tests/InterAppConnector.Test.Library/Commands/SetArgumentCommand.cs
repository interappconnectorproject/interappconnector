using InterAppConnector.Attributes;
using InterAppConnector.Interfaces;
using InterAppConnector.Test.Library.DataModels;

namespace InterAppConnector.Test.Library.Commands
{
    [Command("setargument")]
    public class SetArgumentCommand : ICommand<SetArgumentMethodDataModel>
    {
        public string Main(SetArgumentMethodDataModel arguments)
        {
            return CommandOutput.Ok(arguments);
        }
    }
}
