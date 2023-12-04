using InterAppConnector.Interfaces;

namespace InterAppConnector.Test.Library.Validators
{
    public class BirthDateValidator : IValueValidator
    {
        public object GetSampleValidValue()
        {
            return DateTime.Now.AddYears(-5).ToString();
        }

        public bool ValidateValue(object value)
        {
            bool validated = true;
            DateTime chosenDate = (DateTime) value;

            if (chosenDate.Year < 1900 || chosenDate > DateTime.Now)
            {
                validated = false;
            }

            return validated;
        }
    }
}
