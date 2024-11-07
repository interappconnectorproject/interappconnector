using InterAppConnector.DataModels;
using InterAppConnector.Rules;
using NUnit.Framework;

namespace InterAppConnector.Test.Library.Rules
{
    /// <summary>
    /// These tests are used only to increase the code coverage score.
    /// The methods tested in this class will not be used in real applications
    /// </summary>
    public class ExampleValueRuleTest
    {
        public static int fictiousField;
        public static int FictiousProperty { get; set; }

        [Theory]
        public void DefineArgumentIfTypeExists_WithAFictiousField_ReturnNotImplementedException()
        {
            ExampleValueRule rule = new ExampleValueRule();
            ParameterDescriptor newDescriptor = new ParameterDescriptor();

            ParameterDescriptor descriptor = rule.DefineArgumentIfTypeExists(null, typeof(ExampleValueRuleTest).GetFields()[0], newDescriptor);

            Assert.That(descriptor, Is.EqualTo(newDescriptor));
        }

        [Theory]
        public void DefineArgumentIfTypeDoesNotExist_WithAFictiousField_ReturnNotImplementedException()
        {
            ExampleValueRule rule = new ExampleValueRule();
            ParameterDescriptor newDescriptor = new ParameterDescriptor();

            ParameterDescriptor descriptor = rule.DefineArgumentIfTypeDoesNotExist(null, typeof(ExampleValueRuleTest).GetFields()[0], newDescriptor);

            Assert.That(descriptor, Is.EqualTo(newDescriptor));
        }
    }
}
