using InterAppConnector.Enumerations;
using NUnit.Framework;

namespace InterAppConnector.Test.Library
{
    public class StructHelperTest
    {
        [Test]
        public void IsStruct_UsePrimitiveParameter_ReturnFalse()
        {
            // no assumptions

            bool isEnum = StructHelper.IsStruct(typeof(int));

            Assert.That(isEnum, Is.False);
        }

        [Test]
        public void IsStruct_UseClassParameter_ReturnFalse()
        {
            // no assumptions

            bool isEnum = StructHelper.IsStruct(typeof(LicensePlate));

            Assert.That(isEnum, Is.False);
        }

        [Test]
        public void IsStruct_UseEnumParameter_ReturnFalse()
        {
            // no assumptions

            bool isEnum = StructHelper.IsStruct(typeof(CommandExecutionType));

            Assert.That(isEnum, Is.False);
        }

        [Test]
        public void IsStruct_UseStructParameter_ReturnTrue()
        {
            // no assumptions

            bool isEnum = StructHelper.IsStruct(typeof(DateTime));

            Assert.That(isEnum, Is.True);
        }
    }
}
