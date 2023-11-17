namespace InterAppConnector.Exceptions
{
    /// <summary>
    /// Exception thrown when a parameter that is defined as mandatory is missing
    /// </summary>
    public class MissingMandatoryArgumentException : ApplicationException
    {
        private List<string> _missingParameters = new List<string>();

        /// <summary>
        /// List of missing parameters
        /// </summary>
        public List<string> MissingParameters
        {
            get
            {
                return _missingParameters;
            }
        }

        /// <summary>
        /// Constructor for the exception
        /// </summary>
        /// <param name="missingParameters">List of missing parameters</param>
        /// <param name="message">The extended message</param>
        public MissingMandatoryArgumentException(List<string> missingParameters, string message) : base(message)
        {
            _missingParameters = missingParameters;
        }
    }
}
