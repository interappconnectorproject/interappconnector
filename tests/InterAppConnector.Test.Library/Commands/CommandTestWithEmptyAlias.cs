using InterAppConnector.Interfaces;
using InterAppConnector.Test.Library.DataModels;

namespace InterAppConnector.Test.Library.Commands
{
    public class CommandTestWithEmptyAlias : ICommand<DataModelWithEmptyAlias>
    {
        public string Main(DataModelWithEmptyAlias arguments)
        {
            return CommandOutput.Ok("This class will thow an ArgumentException exception");
        }
    }
}
