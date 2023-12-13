namespace InterAppConnector.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExampleValueAttribute : Attribute
    {
        private readonly string _exampleValue;

        public string ExampleValue { 
            get 
            { 
                return _exampleValue; 
            } 
        }

        public ExampleValueAttribute(string exampleValue)
        {
            _exampleValue = exampleValue;
        }
    }
}
