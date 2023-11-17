using InterAppConnector.Attributes;

namespace InterAppConnector.Test.Library
{
    public class WrongParameterMethodInputStringFormatClass
    {
        [CustomInputString]
        public static WrongParameterMethodInputStringFormatClass ConstructClass(int number)
        {
            WrongParameterMethodInputStringFormatClass thisClass = new WrongParameterMethodInputStringFormatClass();
            return thisClass;
        }
    }
}
