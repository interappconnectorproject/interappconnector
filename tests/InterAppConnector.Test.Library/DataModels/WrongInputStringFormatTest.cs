using InterAppConnector.Attributes;

namespace InterAppConnector.Test.Library.DataModels
{
    public class WrongInputStringFormatTest
    {
        [Alias("wrong")]
        [CustomInputString]
        public WrongInputStringFormatClass? WrongFormattedClass { get; set; }
    }
}
