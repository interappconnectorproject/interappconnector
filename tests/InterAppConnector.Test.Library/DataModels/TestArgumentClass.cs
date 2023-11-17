using InterAppConnector.Attributes;
using InterAppConnector.Test.Library.Commands;
using System.ComponentModel;

namespace InterAppConnector.Test.Library.DataModels
{
    public class TestArgumentClass
    {
        [Alias("n1")]
        [Alias("number1")]
        [Description("Contains the first number")]
        public int FirstNumber { get; set; }

        [Alias("n2")]
        [Alias("number2")]
        [Description("Contains the second number")]
        public int SecondNumber { get; set; }

        [Alias("n3")]
        [Description("Contains the third number")]
        [MandatoryForCommand(typeof(CommandTest))]
        public int? ThirdNumber { get; set; }

        [Alias("numbertoignore")]
        [Description("Contains the third number")]
        [MandatoryForCommand(typeof(CommandTest))]
        [ExcludeItemFromCommand]
        public int NumberToIgnoreAsParameter { get; set; } = 90;

        [Alias("allownegative")]
        [Description("Define if negative numbers are allowed or not")]
        public bool? IsNegativeNumbersAllowed { get; set; }
    }
}
