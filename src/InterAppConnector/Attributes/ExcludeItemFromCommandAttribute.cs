namespace InterAppConnector.Attributes
{
    /// <summary>
    /// Exclude a property or a field from the list of the parameters or the values
    /// If this attribute is used, any other attributes defined are also ignored
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ExcludeItemFromCommandAttribute : Attribute
    {
        /// <summary>
        /// Constructor used to exclude the property or the field
        /// </summary>
        public ExcludeItemFromCommandAttribute()
        {

        }
    }
}
