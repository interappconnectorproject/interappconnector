using InterAppConnector.Attributes;

namespace InterAppConnector.Test.Library.DataModels
{
    public class CustomStringDataModel
    {
        public CustomStringFormatClass CustomString { get; set; }

        [Alias("alias")]
        public CustomStringFormatClass? AdditionalParameter { get; set; }

        public CustomStringFormatClass? Optional { get;set; }
    }
}
