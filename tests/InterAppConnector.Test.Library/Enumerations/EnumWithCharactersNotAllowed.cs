using InterAppConnector.Attributes;

namespace InterAppConnector.Test.Library.Enumerations
{
    public enum EnumWithCharactersNotAllowed
    {
        One,

        [Alias("two")]
        [Alias("£")]
        Two
    }
}
