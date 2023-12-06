using InterAppConnector.Interfaces;

namespace InterAppConnector.Test.Library.Validators
{
    public class AgeValidator : IValueValidator
    {
        public object GetSampleValidValue()
        {
            return 25;
        }

        public bool ValidateValue(object value)
        {
            bool validated = true;
            uint number = (uint) value;

            if (number > 100)
            {
                validated = false;
            }

            return validated;
        }
    }
}
