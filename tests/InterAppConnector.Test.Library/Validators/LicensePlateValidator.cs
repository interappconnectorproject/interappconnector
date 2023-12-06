using InterAppConnector.Interfaces;

namespace InterAppConnector.Test.Library.Validators
{
    public class LicensePlateValidator : IValueValidator
    {
        private List<string> _ids = new List<string>();

        public LicensePlateValidator()
        {
            _ids.Add("aaaa1111");
            _ids.Add("bbbb2222");
        }

        public object GetSampleValidValue()
        {
            return _ids[0];
        }

        public bool ValidateValue(object value)
        {
            bool validated = false;
            LicensePlate plate = (LicensePlate) value;

            if (_ids.Contains(plate.Plate))
            {
                validated = true;
            }
            return validated;
        }
    }
}
