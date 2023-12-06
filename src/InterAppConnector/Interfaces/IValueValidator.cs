namespace InterAppConnector.Interfaces
{
    public interface IValueValidator
    {
        public bool ValidateValue(object value);

        public object GetSampleValidValue();

    }
}
