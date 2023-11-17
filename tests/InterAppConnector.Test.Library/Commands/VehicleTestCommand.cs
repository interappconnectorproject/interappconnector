using InterAppConnector.Attributes;
using InterAppConnector.Interfaces;
using InterAppConnector.Test.Library.DataModels;

namespace InterAppConnector.Test.Library.Commands
{
    [Command("testvehicle", Description = "Test vehicle class")]
    public class VehicleTestCommand : ICommand<Vehicle>
    {
        public string Main(Vehicle arguments)
        {
            return CommandOutput.Ok(arguments);
        }
    }
}
