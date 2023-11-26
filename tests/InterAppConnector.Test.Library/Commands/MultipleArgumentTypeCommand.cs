using InterAppConnector.Interfaces;
using InterAppConnector.Test.Library.DataModels;

namespace InterAppConnector.Test.Library.Commands
{
    public class MultipleArgumentTypeCommand : ICommand<MultipleArgumentTypeDataModel>
    {
        public string Main(MultipleArgumentTypeDataModel arguments)
        {
            return CommandOutput.Ok(arguments.MandatoryNumber);
        }
    }
}
