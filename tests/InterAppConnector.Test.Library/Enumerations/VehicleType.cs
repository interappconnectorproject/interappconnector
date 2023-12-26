using InterAppConnector.Attributes;
using System.ComponentModel;

namespace InterAppConnector.Test.Library.Enumerations
{
    public enum VehicleType
    {
        [Description("A car is a vehicle that has four wheels and a motor")]
        Car = 5,

        [Alias("scooter")]
        [Alias("motorcycle")]
        [Description("A motorbike is a vehicle that has two wheels and a motor")]
        Motorbike = 10,

        [Alias("tandem")]
        [Description("A bike is a vehicle that has two wheels and pedals. Some bikes may have a motor")]
        Bike = 15,

        Airplane = 20,

        [ExcludeItemFromCommand]
        Undefined = 25
    }
}
