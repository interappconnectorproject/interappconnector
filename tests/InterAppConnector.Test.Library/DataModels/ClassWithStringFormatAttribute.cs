using InterAppConnector.Attributes;


namespace InterAppConnector.Test.Library.DataModels
{
    public class ClassWithStringFormatAttribute
    {
        [Alias("name")]
        public string FirstName { get; set; }

        [Alias("surname")]
        public string LastName { get; set; }

        [CustomInputString("yyyyMMdd")]
        public DateTime BirthDate { get; set; }

        [Alias("death")]
        [CustomInputString("ddMMyyyy")]
        public DateTime DeathDate { get; set; }

        [CustomInputString]
        public PublicationList? Publications { get; set; }

        public Uri WikipediaWebsite { get; set; }

        /*
         * If you want, you can also omit the CustomInputString attribute as it is
         * redundant, but in this case it is needed for testing puposes 
         */
        [CustomInputString]
        [Alias("additionallink")]
        public Uri? AdditionalWebsite { get; set; }
    }
}
