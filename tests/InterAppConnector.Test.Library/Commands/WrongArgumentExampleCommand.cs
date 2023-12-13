using InterAppConnector.Attributes;
using InterAppConnector.Interfaces;
using InterAppConnector.Test.Library.DataModels;

namespace InterAppConnector.Test.Library.Commands
{
    [Command("wrongexampledatamodel")]
    public class WrongArgumentExampleCommand : ICommand<WrongExampleDataModel>
    {
        public string Main(WrongExampleDataModel arguments)
        {
            return CommandOutput.Error("If you see this message, there is a problem with the validation of the Example attribute");
        }
    }
}
