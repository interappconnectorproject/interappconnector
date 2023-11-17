namespace InterAppConnector.Exceptions
{
    /// <summary>
    /// Excepion raised when the object is not the same type of the expected one
    /// </summary>
    public class TypeMismatchException : ApplicationException
    {
        private string _expectedType = "";
        private string _declaredType = "";
        private string _originalMessage = "";

        /// <summary>
        /// The expected type
        /// </summary>
        public string ExpectedType
        {
            get
            {
                return _expectedType;
            }
        }

        /// <summary>
        /// The type of the actual object
        /// </summary>
        public string DeclaredType
        {
            get
            {
                return _declaredType;
            }
        }

        /// <summary>
        /// The original message. This property is useful if you expect that there are various type
        /// of messages (for instance if an action is successful returns an object, otherwise a string
        /// with the error or another object). You can parse this message with the
        /// <see cref="CommandOutput.Parse{T}(string)"/> method
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
        /// <param name="expectedType">The expected type</param>
        /// <param name="declaredType">The type of the actual object</param>
        /// <param name="message">The extended message</param>
        public TypeMismatchException(string expectedType, string declaredType, string originalMessage, string message) : base(message)
        {
            _expectedType = expectedType;
            _declaredType = declaredType;
            _originalMessage = originalMessage;
        }
    }
}
