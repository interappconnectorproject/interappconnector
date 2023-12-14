using InterAppConnector.Attributes;
using InterAppConnector.Interfaces;
using InterAppConnector.Test.Library.DataModels;

namespace InterAppConnector.Test.Library.Commands
{
    [Command("booleancommand")]
    public class BooleanCommand : ICommand<DataModelWithBoolean>
    {
        public string Main(DataModelWithBoolean arguments)
        {
            return CommandOutput.Ok(arguments);
        }
    }
}
