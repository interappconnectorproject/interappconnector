using InterAppConnector.Interfaces;
using InterAppConnector.Test.Library.DataModels;

namespace InterAppConnector.Test.Library.Commands
{
    public class CommandTestWithNullAlias : ICommand<DataModelWithNullAlias>
    {
        public string Main(DataModelWithNullAlias arguments)
        {
            return CommandOutput.Ok("This class will thow an ArgumentException exception");
        }
    }
}
