using InterAppConnector.Attributes;

namespace InterAppConnector.Test.Library.DataModels
{
    public class WrongExampleDataModel
    {
        [ExampleValue("")]
        public int Number { get; set; }
    }
}
