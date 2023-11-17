using InterAppConnector.Attributes;

namespace InterAppConnector.Test.Library.Enumerations
{
    public enum EnumWithNullAlias
    {
        One,

        [Alias("two")]
        [Alias(null)]
        Two
    }
}
