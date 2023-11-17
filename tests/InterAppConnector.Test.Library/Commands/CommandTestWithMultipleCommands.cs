using InterAppConnector.Attributes;
using InterAppConnector.Interfaces;
using InterAppConnector.Test.Library.DataModels;

namespace InterAppConnector.Test.Library.Commands
{
    [Command("wrongcommandwithmultiplecommand", Description = "Command that throws a MultipleCommandNotAllowed exception")]
    public class CommandTestWithMultipleCommands : ICommand<DataModelExample>, ICommand<TestArgumentClass>
    {
        public string Main(TestArgumentClass arguments)
        {
            return CommandOutput.Ok(arguments);
        }

        string ICommand<DataModelExample>.Main(DataModelExample arguments)
        {
            return CommandOutput.Ok(arguments);
        }
    }
}
