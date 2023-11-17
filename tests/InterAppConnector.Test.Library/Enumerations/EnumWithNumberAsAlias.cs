using InterAppConnector.Attributes;

namespace InterAppConnector.Test.Library.Enumerations
{
    public enum EnumWithNumberAsAlias
    {
        One,

        [Alias("two")]
        [Alias("12")]
        Two
    }
}
