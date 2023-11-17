using InterAppConnector.Attributes;

namespace InterAppConnector.Interfaces
{
    /// <summary>
    /// Represent the base interface for commands.
    /// This interface should be used in conjunction with <seealso cref="CommandAttribute"/> attribute
    /// </summary>
    public interface ICommand<ParameterType> : ICommand
    {
        public string Main(ParameterType arguments);
    }
}