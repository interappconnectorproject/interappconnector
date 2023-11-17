using InterAppConnector.Attributes;

namespace InterAppConnector.Test.Library.DataModels
{
    public class DataModelWithNullAlias
    {
        public string First { get; set; }

        [Alias("second")]
        [Alias(null)]
        public string Second { get; set; }
    }
}
