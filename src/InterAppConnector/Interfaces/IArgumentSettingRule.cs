namespace InterAppConnector.Interfaces
{
    public interface IArgumentSettingRule<in RuleType> : IArgumentSettingRule
    {
        virtual bool ReturnTrue(RuleType type)
        {
            return true;
        }
    }
}
