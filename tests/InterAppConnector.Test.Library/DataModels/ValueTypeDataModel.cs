using InterAppConnector.Attributes;
using InterAppConnector.Test.Library.Validators;

namespace InterAppConnector.Test.Library.DataModels
{
    public class ValueTypeDataModel
    {
        [ExampleValue("50")]
        public int MandatoryValue { get; set; }

        public uint? OptionalValue { get; set; }
    }
}
