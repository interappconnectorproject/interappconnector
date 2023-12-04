namespace InterAppConnector.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValueValidatorAttribute : Attribute
    {
        private readonly Type _valueValidatorType;

        /// <summary>
        /// The type of the class that contains the validation logic
        /// </summary>
        public Type ValueValidatorType
        {
            get
            {
                return _valueValidatorType;
            }
        }

        public ValueValidatorAttribute(Type valueValidatorType)
        {
            _valueValidatorType = valueValidatorType;
        }
    }
}
