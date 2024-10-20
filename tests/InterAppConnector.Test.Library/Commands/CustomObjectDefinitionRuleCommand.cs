using InterAppConnector.Attributes;
using InterAppConnector.Interfaces;
using InterAppConnector.Test.Library.DataModels;

namespace InterAppConnector.Test.Library.Commands
{
    [Command("customobjectdefinitionrulecommand")]
    public class CustomObjectDefinitionRuleCommand : ICommand<DataModelWithCustomObjectDefinitionRule>
    {
        public string Main(DataModelWithCustomObjectDefinitionRule arguments)
        {
            return CommandOutput.Ok(arguments);
        }
    }
}
