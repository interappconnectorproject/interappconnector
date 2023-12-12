using InterAppConnector.Interfaces;

namespace InterAppConnector.Test.Library.Validators
{
    public class AgeValidatorWithCustomErrorMessageValidator : IValueValidator
    {
        public object GetSampleValidValue()
        {
            return 25;
        }

        public bool ValidateValue(object value)
        {
            bool validated = true;
            uint number = (uint)value;

            if (number > 100)
            {
                throw new ArgumentException("The age must be between 0 and 100. For instance, a valid value is " + GetSampleValidValue());
            }

            return validated;
        }
    }
}
