using InterAppConnector.Interfaces;

namespace InterAppConnector.Exceptions
{
    /// <summary>
    /// Exception raised when more than one <seealso cref="ICommand{ParameterType}"/> are implemented
    /// </summary>
    public class MultipleCommandNotAllowedException : ApplicationException
    {
        /// <summary>
        /// The name of the class that have the problem
        /// </summary>
        public string TargetClass { get; private set; }

        /// <summary>
        /// Constructor used in order to create the exception
        /// </summary>
        /// <param name="targetClass">The name of the class that have the problem</param>
        /// <param name="message">The extended message</param>
        public MultipleCommandNotAllowedException(string targetClass, string message) : base(message)
        {
            TargetClass = targetClass;
        }
    }
}
