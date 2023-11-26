using InterAppConnector.Attributes;
using InterAppConnector.Test.Library.Enumerations;

namespace InterAppConnector.Test.Library.DataModels
{
    public class Vehicle
    {
        public VehicleType Type { get; set; }
        // This format is used for example in Germany 
        [CustomInputString("llllnnnn")]
        public LicensePlate LicensePlate { get; set; } = new LicensePlate("aaaa0000");
    }
}
