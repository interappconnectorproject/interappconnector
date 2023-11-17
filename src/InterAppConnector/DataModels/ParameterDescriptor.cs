namespace InterAppConnector.DataModels
{
    /// <summary>
    /// Describes the argument
    /// </summary>
    public class ParameterDescriptor
    {
        /// <summary>
        /// The argument name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The argument value
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// The description associated to the argument
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The name of the original property in the object
        /// </summary>
        internal string OriginalPropertyName { get; set; } = string.Empty;

        /// <summary>
        /// Set the type of the parameter
        /// </summary>
        internal Type ParameterType { get; set; }

        /// <summary>
        /// Contains the list of the attributes defined for the property without the ones used by
        /// CommandManager
        /// </summary>
        public List<object> Attributes { get; set; } = new List<object>();

        /// <summary>
        /// Define if the parameter is mandatory or not
        /// </summary>
        public bool IsMandatory { get; set; }

        /// <summary>
        /// The argument aliases
        /// </summary>
        public List<string> Aliases { get; set; } = new List<string>();
    }
}
