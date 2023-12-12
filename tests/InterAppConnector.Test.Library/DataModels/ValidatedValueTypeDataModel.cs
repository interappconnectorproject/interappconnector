using InterAppConnector.Attributes;
using InterAppConnector.Test.Library.Validators;

namespace InterAppConnector.Test.Library.DataModels
{
    public class ValidatedValueTypeDataModel
    {
        [ValueValidator(typeof(AgeValidatorWithCustomErrorMessageValidator))]
        public uint Age { get; set; }

        [ValueValidator(typeof(BirthDateValidator))]
        [CustomInputString("ddMMyyyy")]
        public DateTime? OptionalDate { get; set; }
    }
}
