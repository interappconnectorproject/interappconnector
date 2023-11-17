using InterAppConnector.Attributes;

namespace InterAppConnector.Test.Library
{
    /// <summary>
    /// This class is useful to test duplicate methods 
    /// </summary>
    public class DuplicateCustomStringFormatClass
    {
        private List<string> _list = new List<string>();

        public List<string> List
        {
            get
            {
                return _list;
            }
        }

        public DuplicateCustomStringFormatClass(List<string> strings)
        {
            _list.AddRange(strings);
        }

        [CustomInputString]
        public static DuplicateCustomStringFormatClass ConstructClass(string parameter)
        {
            DuplicateCustomStringFormatClass thisClass = new DuplicateCustomStringFormatClass(parameter.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList());
            return thisClass;
        }

        [CustomInputString]
        public static DuplicateCustomStringFormatClass AddDuplicateClassClass(string parameter)
        {
            DuplicateCustomStringFormatClass thisClass = new DuplicateCustomStringFormatClass(parameter.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList());
            return thisClass;
        }
    }
}
