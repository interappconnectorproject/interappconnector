using InterAppConnector.Attributes;

namespace InterAppConnector.Test.Library.DataModels
{
    public class WrongFormattedInputStringFormatTest
    {
        [Alias("wrong")]
        [CustomInputString("ddMMyyyy")]
        public WrongInputStringFormatClass? WrongFormattedClass { get; set; }
    }
}
