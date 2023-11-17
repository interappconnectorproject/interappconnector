namespace InterAppConnector.Exceptions
{
    /// <summary>
    /// This exception is raised when the text structure does not match with the expected one
    /// For instance, this exception is useful when if it is expected a JSON text message 
    /// and instead it is received a normal text
    /// </summary>
    public class MalformedMessageException : ApplicationException
    {
        private string _originalMessage = "";

        /// <summary>
        /// The original message
        /// </summary>
        public string OriginalMessage
        {
            get
            {
                return _originalMessage;
            }
        }

        /// <summary>
        /// Constructor of the exception
        /// </summary>
        /// <param name="originalMessage">The original message</param>
        /// <param name="message">The extended message</param>
        public MalformedMessageException(string originalMessage, string message) : base(message)
        {
            _originalMessage = originalMessage;
        }
    }
}
