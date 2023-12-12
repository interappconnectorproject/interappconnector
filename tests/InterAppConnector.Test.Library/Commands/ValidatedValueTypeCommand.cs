using InterAppConnector.Attributes;
using InterAppConnector.Interfaces;
using InterAppConnector.Test.Library.DataModels;

namespace InterAppConnector.Test.Library.Commands
{
    [Command("validatedvaluetype")]
    public class ValidatedValueTypeCommand : ICommand<ValidatedValueTypeDataModel>
    {
        public string Main(ValidatedValueTypeDataModel arguments)
        {
            return CommandOutput.Ok(arguments.Age);
        }
    }
}
