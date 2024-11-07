namespace InterAppConnector.Test.Library.DataModels
{
    public class ArgumentWithCustomSettingRule
    {
        public static List<string> RulesCalledForThisArgument { get; set; } = new List<string>();

        public int Number { get; set; }

        public ArgumentWithCustomSettingRule(string value)
        {
            Number = int.Parse(value);
        }
    }
}
