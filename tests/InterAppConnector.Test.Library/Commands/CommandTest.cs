using InterAppConnector.Attributes;
using InterAppConnector.Interfaces;
using InterAppConnector.Test.Library.DataModels;

namespace InterAppConnector.Test.Library.Commands
{
    [Command("read", Description = "Command Test")]
    public class CommandTest : ICommand<TestArgumentClass>
    {
        public string Main(TestArgumentClass arguments)
        {
            return CommandOutput.Ok(arguments.FirstNumber + arguments.SecondNumber + arguments.ThirdNumber.Value);
        }
    }
}
