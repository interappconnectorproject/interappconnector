using InterAppConnector.Interfaces;

namespace InterAppConnector.Test.Library.Validators
{
    public class GuidValidator : IValueValidator
    {
        private List<Guid> _ids = new List<Guid>();

        public GuidValidator() 
        { 
            _ids.Add(Guid.Parse("e6aad9ac-2205-44ba-bb28-0ad0a1c75a0f"));
        }

        public object GetSampleValidValue()
        {
            return _ids[0];
        }

        public bool ValidateValue(object value)
        {
            bool validated = false;
            Guid id = (Guid)value;

            if (_ids.Contains(id))
            {
                validated = true;
            }
            return validated;
        }
    }
}
