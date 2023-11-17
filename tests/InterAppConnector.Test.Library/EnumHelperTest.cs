using InterAppConnector.Enumerations;
using InterAppConnector.Exceptions;
using InterAppConnector.Test.Library.Enumerations;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace InterAppConnector.Test.Library
{
    public class EnumHelperTest
    {
        [Test]
        public void LoadEnumerationValues_WithEnumExample_ReturnParsedParameters()
        {
            EnumHelper helper = new EnumHelper();

            helper.LoadEnumerationValues<VehicleType>();

            Assert.That(helper._parameters, Has.Count.EqualTo(4));
            Assert.That(helper._parameters["car"].Value, Is.EqualTo(5));
            Assert.That(helper._parameters["car"].Aliases, Has.Count.EqualTo(0));
            Assert.That(helper._parameters["motorbike"].Value, Is.EqualTo(10));
            Assert.That(helper._parameters["motorbike"].Aliases, Has.Count.EqualTo(2));
            Assert.That(helper._parameters["bike"].Value, Is.EqualTo(15));
            Assert.That(helper._parameters["bike"].Aliases, Has.Count.EqualTo(1));
        }

        [Test]
        public void LoadEnumerationValues_WithEnumWithEmptyString_ReturnArgumentException()
        {
            EnumHelper helper = new EnumHelper();

            Action parsedValue = () =>
            {
                helper.LoadEnumerationValues<EnumWithEmptyStringAlias>();
            };

            Assert.That(parsedValue, Throws.ArgumentException);
        }

        [Test]
        public void LoadEnumerationValues_WithAliasNotAllowed_ReturnArgumentException()
        {
            EnumHelper helper = new EnumHelper();

            Action parsedValue = () =>
            {
                helper.LoadEnumerationValues<EnumWithCharactersNotAllowed>();
            };

            Assert.That(parsedValue, Throws.ArgumentException);
        }

        [Test]
        public void LoadEnumerationValuesGeneric_WithEnumWithNullValue_ReturnArgumentException()
        {
            EnumHelper helper = new EnumHelper();

            Action parsedValue = () =>
            {
                helper.LoadEnumerationValues<EnumWithNullAlias>();
            };

            Assert.That(parsedValue, Throws.ArgumentException);
        }

        [Test]
        public void LoadEnumerationValues_WithEnumWithNullValue_ReturnArgumentException()
        {
            EnumHelper helper = new EnumHelper();

            Action parsedValue = () =>
            {
                helper.LoadEnumerationValues(typeof(EnumWithNullAlias));
            };

            Assert.That(parsedValue, Throws.ArgumentException);
        }

        [Test]
        public void LoadEnumerationValues_WithAnObjectDifferentFromEnum_ReturnTypeMismatchException()
        {
            EnumHelper helper = new EnumHelper();

            Action parsedValue = () =>
            {
                helper.LoadEnumerationValues(typeof(int));
            };

            Assert.That(parsedValue, Throws.InstanceOf<TypeMismatchException>());
        }

        [Test]
        public void LoadEnumerationValues_WithEnumWithNumberValue_ReturnArgumentException()
        {
            EnumHelper helper = new EnumHelper();

            Action parsedValue = () =>
            {
                helper.LoadEnumerationValues<EnumWithNumberAsAlias>();
            };

            Assert.That(parsedValue, Throws.ArgumentException);
        }

        [TestCase("car", VehicleType.Car)]
        [TestCase("5", VehicleType.Car)]
        [TestCase("Scooter", VehicleType.Motorbike)]
        [TestCase("MotorCycle", VehicleType.Motorbike)]
        [TestCase("10", VehicleType.Motorbike)]
        [TestCase("tandem", VehicleType.Bike)]
        [TestCase("15", VehicleType.Bike)]
        public void GetEnumerationFieldByValue_WithEnumExample_ReturnEnumFields(string value, VehicleType expectedValue)
        {
            // no arrangement

            VehicleType result = EnumHelper.GetEnumerationFieldByValue<VehicleType>(value);

            Assert.That(result, Is.EqualTo(expectedValue));
        }

        [TestCase("undefined", VehicleType.Undefined)]
        [TestCase("25", VehicleType.Undefined)]
        public void GetEnumerationFieldByValue_WithExcludedEnumExample_ReturnArgumentExceptionException(string value, VehicleType unexpectedValue)
        {
            // no arrangement

            Action parsedValue = () => EnumHelper.GetEnumerationFieldByValue(typeof(VehicleType), value);

            Assert.That(parsedValue, Throws.ArgumentException);
        }

        [Test]
        public void GetEnumerationFieldByValue_WithInexistentValuesForEnum_ReturnArgumentException()
        {
            // no arrangement

            Action parsedValue = () => EnumHelper.GetEnumerationFieldByValue(typeof(VehicleType), "18");

            Assert.That(parsedValue, Throws.ArgumentException);
        }

        [TestCase("18")]
        [TestCase("quod")]
        [TestCase("150")]
        public void GetEnumerationFieldByValueGeneric_WithInexistentValuesForEnum_ReturnArgumentException(string value)
        {
            // no arrangement

            Action parsedValue = () => EnumHelper.GetEnumerationFieldByValue<VehicleType>(value);

            Assert.That(parsedValue, Throws.ArgumentException);
        }

        [Test]
        public void GetEnumerationFieldByValue_GetFieldNameWithNoAlias_ReturnFieldName()
        {
            // no arrangement

            string field = EnumHelper.GetFieldName<CommandExecutionType>(nameof(CommandExecutionType.Batch));

            Assert.That(field, Is.EqualTo("Batch"));
        }

        [Test]
        public void GetEnumerationFieldByValue_GetFieldNameWithAlias_ReturnFirstAlias()
        {
            // no arrangement

            string field = EnumHelper.GetFieldName<VehicleType>(nameof(VehicleType.Motorbike));

            Assert.That(field, Is.EqualTo("scooter"));
        }

        [Test]
        public void GetEnumerationFieldByValue_WithAnObjectDifferentFromEnum_ReturnTypeMismatchException()
        {
            EnumHelper helper = new EnumHelper();

            Action parsedValue = () =>
            {
                EnumHelper.GetEnumerationFieldByValue(typeof(int), "Test string");
            };

            Assert.That(parsedValue, Throws.InstanceOf<TypeMismatchException>());
        }
    }
}
