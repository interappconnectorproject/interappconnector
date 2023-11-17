namespace InterAppConnector.Exceptions
{
    /// <summary>
    /// Exception raised when a duplicate object is found, for instance
    /// <list type="bullet">
    ///     <item>when a command is already defined in another class</item>
    ///     <item>when an alias is already used in another property</item>
    ///     <item>when an alias refers to a multiple values in an enumeration</item>
    /// </list>
    /// </summary>
    public class DuplicateObjectException : ApplicationException
    {
        /// <summary>
        /// The object that has the duplicate parameter
        /// </summary>
        public string ActualObjectName { get; private set; }

        /// <summary>
        /// List of the duplicate values assigned for that object
        /// </summary>
        public List<string> DuplicateValuesAssigned { get; private set; }

        /// <summary>
        /// The first object that has the parameter  
        /// </summary>
        public string SourceObjectName { get; private set; }

        /// <summary>
        /// Exception constructor
        /// </summary>
        /// <param name="actualObjectName">The object that has the duplicate parameter</param>
        /// <param name="duplicateValuesAssigned">The list of the values duplicated</param>
        /// <param name="sourceObjectName">The first object that has the parameter</param>
        /// <param name="message">The exception message</param>
        public DuplicateObjectException(string actualObjectName, List<string> duplicateValuesAssigned, string sourceObjectName, string message) : base(message)
        {
            ActualObjectName = actualObjectName;
            DuplicateValuesAssigned = duplicateValuesAssigned;
            SourceObjectName = sourceObjectName;
        }
    }
}
