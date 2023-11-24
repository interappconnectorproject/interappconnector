using InterAppConnector.Attributes;

namespace InterAppConnector.Test.Library
{
    public class CustomStringFormatClass
    {
        private List<string> _list= new List<string>();

        public List<string> List 
        { 
            get 
            { 
                return _list; 
            } 
        }

        public CustomStringFormatClass(List<string> strings)
        {
            _list.AddRange(strings);

            if (_list.Count == 0)
            {
                throw new FormatException("No elements was added to the list. Please specify at least one string");
            }
        }

        [CustomInputString]
        public static CustomStringFormatClass ConstructClass(string parameter)
        {
            CustomStringFormatClass thisClass = new CustomStringFormatClass(parameter.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList());
            return thisClass;
        }
    }
}
