using InterAppConnector.Attributes;
using InterAppConnector.Interfaces;
using InterAppConnector.Test.Library.DataModels;

namespace InterAppConnector.Test.Library.Commands
{
    [Command("valuetype")]
    public class ValueTypeCommand : ICommand<ValueTypeDataModel>
    {
        public string Main(ValueTypeDataModel arguments)
        {
            if (!arguments.OptionalValue.HasValue)
            {
                arguments.OptionalValue = 0;
            }

            return CommandOutput.Ok(arguments.MandatoryValue + arguments.OptionalValue!.Value);
        }
    }
}
