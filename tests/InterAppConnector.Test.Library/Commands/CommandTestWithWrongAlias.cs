using InterAppConnector.Interfaces;
using InterAppConnector.Test.Library.DataModels;

namespace InterAppConnector.Test.Library.Commands
{
    public class CommandTestWithWrongAlias : ICommand<DataModelWithWrongAlias>
    {
        public string Main(DataModelWithWrongAlias arguments)
        {
            return CommandOutput.Error("You should not see this string. If you see this string, there is a problem with the InterAppConnector library");
        }
    }
}
