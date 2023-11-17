using InterAppConnector.DataModels;
using InterAppConnector.Interfaces;

namespace InterAppConnector.Test.Library.Commands
{
    public class MinimalCommand : ICommand<EmptyDataModel>
    {
        public string Main(EmptyDataModel arguments)
        {
           return CommandOutput.Ok("Success");
        }
    }
}
