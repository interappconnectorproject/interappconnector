using InterAppConnector.DataModels;
using InterAppConnector.Rules;
using NUnit.Framework;

namespace InterAppConnector.Test.Library.Rules
{
    public class ValueValidatorRuleTest
    {
        public static int fictiousField;
        public static int FictiousProperty { get; set; }

        [Theory]
        public void IsRuleEnabledInArgumentSetting_WithAFictiousField_ReturnTrue()
        {
            ValueValidatorRule rule = new ValueValidatorRule();

            bool returnFalse = rule.IsRuleEnabledInArgumentSetting(typeof(ValueValidatorRuleTest).GetFields()[0]);

            Assert.That(returnFalse, Is.True);
        }

        [Theory]
        public void SetArgumentValueIfTypeDoesNotExist_WithAFictiousField_ReturnArgumentDescriptor()
        {
            ValueValidatorRule rule = new ValueValidatorRule();

            ParameterDescriptor descriptor = rule.SetArgumentValueIfTypeDoesNotExist(null, typeof(ValueValidatorRuleTest).GetFields()[0], new ParameterDescriptor(), new ParameterDescriptor());

            Assert.That(descriptor, Is.EqualTo(new ParameterDescriptor()));
        }

        [Theory]
        public void SetArgumentValueIfTypeExists_WithAFictiousField_ReturnArgumentDescriptor()
        {
            ValueValidatorRule rule = new ValueValidatorRule();

            ParameterDescriptor descriptor = rule.SetArgumentValueIfTypeExists(null, typeof(ValueValidatorRuleTest).GetFields()[0], new ParameterDescriptor(), new ParameterDescriptor());

            Assert.That(descriptor, Is.EqualTo(new ParameterDescriptor()));
        }
    }
}
