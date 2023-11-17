using InterAppConnector.Attributes;

namespace InterAppConnector.Test.Library.DataModels
{
    public class DuplicateParameter
    {
        [Alias("first")]
        public string One { get; set; }
        [Alias("second")]
        public string Two { get; set; }
        [Alias("first")]
        public string Three { get; set; }
    }
}
