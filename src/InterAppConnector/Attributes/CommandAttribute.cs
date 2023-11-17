using InterAppConnector.Interfaces;

namespace InterAppConnector.Attributes
{
    /// <summary>
    /// Command attribute define a new command. This attribute is used only to describe the command.
    /// In order to complete the definition of the command, you have to implement the <seealso cref="ICommand{ParameterType}"/> interface
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        private string _name;

        /// <summary>
        /// Description of the command. Even if this property is not mandatory,
        /// it is recommended to add a description in the command, as it wll be
        /// added in the command description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The command name
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Define a new command
        /// </summary>
        /// <param name="actionName">The action name. It cannot be <see langword="null"/>, empty or <seealso cref="string.Empty"/></param>
        public CommandAttribute(string actionName)
        {
            _name = actionName;
        }
    }
}
