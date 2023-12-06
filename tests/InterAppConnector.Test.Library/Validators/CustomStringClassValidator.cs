using InterAppConnector.Interfaces;

namespace InterAppConnector.Test.Library.Validators
{
    public class CustomStringClassValidator : IValueValidator
    {
        private List<string> _allowedValues = new List<string>()
        {
            "apple",
            "pear",
            "banana",
            "pineapple"
        };

        public object GetSampleValidValue()
        {
            return _allowedValues[0];
        }

        public bool ValidateValue(object value)
        {
            bool validated = true;
            CustomStringFormatClass data = (CustomStringFormatClass) value;

            foreach (string item in data.List)
            {
                if (!_allowedValues.Contains(item.ToLower().Trim()))
                {
                    validated = false;
                }
            }

            return validated;
        }
    }
}
