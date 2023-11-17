using InterAppConnector.Attributes;

namespace InterAppConnector.Test.Library.DataModels
{
    public class DataModelWithEmptyAlias
    {
        public string First { get; set; }

        [Alias("second")]
        [Alias("")]
        public string Second { get; set; }
    }
}
