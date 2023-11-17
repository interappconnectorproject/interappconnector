using InterAppConnector.Attributes;

namespace InterAppConnector.Test.Library.Enumerations
{
    public enum EnumWithEmptyStringAlias
    {
        One,

        [Alias("two")]
        [Alias("  ")]
        Two
    }
}
