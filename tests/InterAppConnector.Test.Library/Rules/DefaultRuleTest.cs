using InterAppConnector.DataModels;
using InterAppConnector.Rules;
using NUnit.Framework;

namespace InterAppConnector.Test.Library.Rules
{
    /// <summary>
    /// These tests are used only to increase the code coverage score.
    /// The methods thested in this class will not be used in real applications
    /// </summary>
    public class DefaultRuleTest
    {
        public static int fictiousField;
        public static int FictiousProperty { get; set; }

        [Theory]
        public void SetArgumentValueIfTypeDoesNotExist_WithAFictiousField_ReturnArgumentDescriptor()
        {
            DefaultRule rule = new DefaultRule();
            ParameterDescriptor newDescriptor = new ParameterDescriptor();

            ParameterDescriptor descriptor = rule.SetArgumentValueIfTypeDoesNotExist(null, typeof(DefaultRuleTest).GetFields()[0], newDescriptor, new ParameterDescriptor());

            Assert.That(descriptor, Is.EqualTo(newDescriptor));
        }

        [Theory]
        public void DefineArgumentIfTypeDoesNotExist_WithAFictiousField_ReturnArgumentDescriptor()
        {
            DefaultRule rule = new DefaultRule();
            ParameterDescriptor newDescriptor = new ParameterDescriptor();

            ParameterDescriptor descriptor = rule.DefineArgumentIfTypeDoesNotExist(null, typeof(DefaultRuleTest).GetFields()[0], newDescriptor);

            Assert.That(descriptor, Is.EqualTo(newDescriptor));
        }

        [Theory]
        public void DefineArgumentIfTypeDoesNotExist_WithAFictiousProperty_ReturnArgumentDescriptor()
        {
            DefaultRule rule = new DefaultRule();
            ParameterDescriptor newDescriptor = new ParameterDescriptor();

            ParameterDescriptor descriptor = rule.DefineArgumentIfTypeDoesNotExist(null, typeof(DefaultRuleTest).GetProperties()[0], newDescriptor);

            Assert.That(descriptor, Is.EqualTo(newDescriptor));
        }

        [Theory]
        public void IsRuleEnabledInArgumentSetting_WithAFictiousField_ReturnFalse()
        {
            DefaultRule rule = new DefaultRule();

            bool returnFalse = rule.IsRuleEnabledInArgumentSetting(typeof(DefaultRuleTest).GetFields()[0]);

            Assert.That(returnFalse, Is.False);
        }

        [Theory]
        public void SetArgumentValueIfTypeExists_WithAFictiousField_ReturnArgumentDescriptor()
        {
            DefaultRule rule = new DefaultRule();
            ParameterDescriptor newDescriptor = new ParameterDescriptor();

            ParameterDescriptor descriptor = rule.SetArgumentValueIfTypeExists(null, typeof(DefaultRuleTest).GetFields()[0], newDescriptor, new ParameterDescriptor());

            Assert.That(descriptor, Is.EqualTo(newDescriptor));
        }
    }
}
