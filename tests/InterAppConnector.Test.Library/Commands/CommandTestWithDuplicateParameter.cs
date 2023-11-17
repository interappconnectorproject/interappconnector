using InterAppConnector.Attributes;
using InterAppConnector.Interfaces;
using InterAppConnector.Test.Library.DataModels;

namespace InterAppConnector.Test.Library.Commands
{
    [Command("duplicateparameter", Description = "Test with duplicate parameter")]
    public class CommandTestWithDuplicateParameter : ICommand<DuplicateParameter>
    {
        public string Main(DuplicateParameter arguments)
        {
            return CommandOutput.Ok(arguments);
        }
    }
}
