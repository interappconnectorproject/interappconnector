using InterAppConnector.DataModels;
using InterAppConnector.Rules;
using NUnit.Framework;

namespace InterAppConnector.Test.Library.Rules
{
    /// <summary>
    /// These tests are used only to increase the code coverage score.
    /// The methods thested in this class will not be used in real applications
    /// </summary>
    public class MandatoryForCommandRuleTest
    {
        [Theory]
        public void DefineArgumentIfTypeExists_WithAFictiousField_ReturnArgumentDescriptor()
        {
            MandatoryForCommandRule rule = new MandatoryForCommandRule();
            ParameterDescriptor newDescriptor = new ParameterDescriptor();

            ParameterDescriptor descriptor = rule.DefineArgumentIfTypeExists(null, typeof(ExampleValueRuleTest).GetFields()[0], newDescriptor);

            Assert.That(descriptor, Is.EqualTo(newDescriptor));
        }
    }
}
