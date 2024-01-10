namespace InterAppConnector.Interfaces
{
    /// <summary>
    /// Interface that can be used in order to define rules for a particular attribute or object
    /// </summary>
    /// <typeparam name="Type">The type </typeparam>
    public interface IArgumentDefinitionRule<in RuleType> : IArgumentDefinitionRule
    {
        virtual bool ReturnTrue(RuleType type)
        {
            return true;
        }
    }
}
