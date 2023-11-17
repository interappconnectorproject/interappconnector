namespace InterAppConnector.Attributes
{
    /// <summary>
    /// Alias attribute is used to define an alternative name for a property in a class or a field in enumerations.
    /// When this attribute is defined, the original name is hidden. You have to use the alias in order to reference to the
    /// property or the field.
    /// When this attribute is used in a property defined in a class, it define the name of the property, while if used
    /// in an enumeration it define an alternative name of the value
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class AliasAttribute : Attribute
    {
        private string _name;

        /// <summary>
        /// The name of the alias
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Define the name of the alias
        /// </summary>
        /// <param name="attributeName">The name of the attribute. The string must not be empty, <see cref="string.Empty"/> or <see langword="null"/></param>
        public AliasAttribute(string attributeName)
        {
            _name = attributeName;
        }
    }
}
