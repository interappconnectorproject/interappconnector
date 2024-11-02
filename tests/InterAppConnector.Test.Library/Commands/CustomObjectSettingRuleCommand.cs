using InterAppConnector.Attributes;
using InterAppConnector.Interfaces;
using InterAppConnector.Test.Library.DataModels;

namespace InterAppConnector.Test.Library.Commands
{
    [Command("customobjectsettingrulecommand")]
    public class CustomObjectSettingRuleCommand : ICommand<DataModelWithCustomObjectSettingRule>
    {
        public string Main(DataModelWithCustomObjectSettingRule arguments)
        {
            return CommandOutput.Ok(arguments.Argument.Number);
        }
    }
}
