using InterAppConnector.Attributes;
using InterAppConnector.Enumerations;

namespace InterAppConnector.Test.Library.DataModels
{
    public class SetArgumentMethodDataModel
    {
        [CustomInputString("llllnnnn")]
        [Alias("plate")]
        public LicensePlate? LicensePlate { get; set; }

        [Alias("char")]
        [Alias("onechar")]
        public char? SingleChar { get; set; }

        public Guid? Guid { get; set; }

        [CustomInputString]
        public Guid? OtherGuid { get; set; }

        public CustomStringFormatClass Custom { get; set; }

        [Alias("customalias")]
        public CustomStringFormatClass CustomAlias { get; set; }

        [Alias("aliasedguid")]
        public Guid AliasedGuid { get; set; }

        public WrongInputStringFormatTest Wrong { get; set; }

        public WrongParameterMethodInputStringFormatClass WrongParameter { get; set; }

        [Alias("wronginputstringformattest")]
        public WrongInputStringFormatTest AliasedWrong { get; set; }

        [Alias("wrongparametermethodinputstringformatclass")]
        public WrongParameterMethodInputStringFormatClass AliasedWrongParameter { get; set; }

        [Alias("commandoutputformat")]
        public CommandOutputFormat Format { get; set; }

        public CommandOutputFormat AlternativeFormat { get; set; }

        [Alias("duplicatecustomstringformatclass")]
        public DuplicateCustomStringFormatClass DuplicateCustom { get; set; }

        public WrongParameterNumberInputStringFormatClass WrongParameterNumberParameter { get; set; }
    }
}
