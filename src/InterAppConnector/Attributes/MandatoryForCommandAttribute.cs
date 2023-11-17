using InterAppConnector.Exceptions;

namespace InterAppConnector.Attributes
{
    /// <summary>
    /// Sometimes it is necessary that an optional parameter is mandatory for
    /// certain actions. Normally for optional parameters you have to define a <see cref="Nullable{T}"/> property.
    /// With this attribute, the property will be required for certain actions. This means that if no value
    /// is defined for this property, a <see cref="MissingMandatoryArgumentException"/> exception is thrown.
    /// You can define more that one attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class MandatoryForCommandAttribute : Attribute
    {
        private Type _command;

        /// <summary>
        /// The command which the parameter is required
        /// </summary>
        public Type Command
        {
            get
            {
                return _command;
            }
        }

        /// <summary>
        /// Constructor that define the command which the parameter is required
        /// </summary>
        /// <param name="command">The command which the parameter is required</param>
        public MandatoryForCommandAttribute(Type command)
        {
            _command = command;
        }
    }
}
