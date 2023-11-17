namespace InterAppConnector.Exceptions
{
    /// <summary>
    /// This exception is raised when a class does not have a method that can be used to
    /// parse the user input. It is raised by custom objects that do not have a public
    /// constructor or the ParseExact static method defined
    /// </summary>
    public class MethodNotFoundException : ApplicationException
    {
        private string _targetClassName;

        /// <summary>
        /// The target class name that does not have a valid method that can be used
        /// to parse the string
        /// </summary>
        public string TargetClassName
        {
            get
            {
                return _targetClassName;
            }
        }

        public MethodNotFoundException(string targetClassName, string message) : base(message)
        {
            _targetClassName = targetClassName;
        }
    }
}
