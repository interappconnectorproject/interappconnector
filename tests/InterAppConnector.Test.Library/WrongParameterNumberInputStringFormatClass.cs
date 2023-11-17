using InterAppConnector.Attributes;
using InterAppConnector.Exceptions;

namespace InterAppConnector.Test.Library
{
    /// <summary>
    /// Test class used in order to test class where the custom method used in order to parse the value
    /// has exactly one parameter. As the method in this class has two parameters, it should raise a
    /// <see cref="TypeMismatchException"/> if the class is used as parameter
    /// </summary>
    public class WrongParameterNumberInputStringFormatClass
    {
        [CustomInputString]
        public WrongParameterNumberInputStringFormatClass CostructClass(string inputString, int number)
        {
            WrongParameterNumberInputStringFormatClass newClass = new WrongParameterNumberInputStringFormatClass();
            return newClass;
        }
    }
}
