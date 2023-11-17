using InterAppConnector.Attributes;

namespace InterAppConnector.Test.Library.DataModels
{
    public class DataModelWithWrongAlias
    {
        [Alias("first-parameter")]
        public string AllowedAliasWithHyphen { get; set; }

        [Alias("wrong alias")]
        public string PropertyWithWrongAlias { get; set; }
    }
}
