using InterAppConnector.Attributes;

namespace InterAppConnector.Test.Library.DataModels
{
    public class DuplicateCustomStringDataModel
    {
        public DuplicateCustomStringFormatClass CustomString { get; set; }

        [Alias("alias")]
        public DuplicateCustomStringFormatClass? AdditionalParameter { get; set; }

        public DuplicateCustomStringFormatClass? Optional { get; set; }
    }
}
