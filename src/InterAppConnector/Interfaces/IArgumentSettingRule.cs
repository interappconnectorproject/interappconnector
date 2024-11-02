namespace InterAppConnector.Interfaces
{
    public interface IArgumentSettingRule<in RuleType> : IArgumentSettingRule
    {
        /// <summary>
        /// This method is useful in order to resolve an issue raised by SonarQube.
        /// The issue raised is S2326: Unused type parameters should be removed but it is necessary.
        /// This method should not be used in real applications
        /// </summary>
        virtual bool ReturnTrue(RuleType type)
        {
            return true;
        }
    }
}
