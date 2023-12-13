using InterAppConnector.Attributes;

namespace InterAppConnector.Test.Library.DataModels
{
    public class WrongValidatorDataModel
    {
        [ValueValidator(typeof(WrongValidatorDataModel))]
        public string Validator { get; set; } = string.Empty;
    }
}
