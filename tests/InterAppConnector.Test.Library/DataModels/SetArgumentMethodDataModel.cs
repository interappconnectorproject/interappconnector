using InterAppConnector.Attributes;
using InterAppConnector.Enumerations;
using InterAppConnector.Test.Library.Validators;

namespace InterAppConnector.Test.Library.DataModels
{
    public class SetArgumentMethodDataModel
    {
        [CustomInputString("llllnnnn")]
        [Alias("plate")]
        public LicensePlate LicensePlate { get; set; }

        public LicensePlate AnotherLicensePlate { get; set; }

        [CustomInputString]
        public LicensePlate AnotherLicensePlate2 { get; set; }

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

        [ValueValidator(typeof(AgeValidator))]
        public uint Age { get; set; }

        [ValueValidator(typeof(GuidValidator))]
        public Guid ValidatedGuid { get; set; }

        [ValueValidator(typeof(GuidValidator))]
        public Guid ValidateOtherGuid { get; set; }

        [ValueValidator(typeof(CustomStringClassValidator))]
        public CustomStringFormatClass ValidatedCustomClass { get; set; }

        [CustomInputString("llllnnnn")]
        [ValueValidator(typeof(LicensePlateValidator))]
        public LicensePlate ValidatedLicensePlate { get; set; }

        [CustomInputString()]
        [ValueValidator(typeof(LicensePlateValidator))]
        public LicensePlate AnotherValidatedLicensePlate { get; set; }

        [ValueValidator(typeof(LicensePlate))]
        public LicensePlate WrongValidator { get; set; }

        [ValueValidator(typeof(BirthDateValidator))]
        [CustomInputString("ddMMyyyy")]
        public DateTime BirthDate { get; set; }

        /*
         * To check. There may be a bug here 
         * [ValueValidator(typeof(CustomStringClassvalidator))]
        [CustomInputString]
        public CustomStringFormatClass OtherValidatedCustomClass { get; set; } 
        */
    }
}
