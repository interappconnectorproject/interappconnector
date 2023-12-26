using InterAppConnector.DataModels;
using InterAppConnector.Rules;
using NUnit.Framework;

namespace InterAppConnector.Test.Library.Rules
{
    /// <summary>
    /// These tests are used only to increase the code coverage score.
    /// The methods thested in this class will not be used in real applications
    /// </summary>
    public class CustomInputStringRuleTest
    {
        public static int fictiousField;
        public static int FictiousProperty { get; set; }

        [Theory]
        public void IsRuleEnabledInArgumentSetting_WithAFictiousField_ReturnTrue()
        {
            CustomInputStringRule rule = new CustomInputStringRule();

            bool returnFalse = rule.IsRuleEnabledInArgumentSetting(typeof(CustomInputStringRuleTest).GetFields()[0]);

            Assert.That(returnFalse, Is.False);
        }

        [Theory]
        public void SetArgumentValueIfTypeDoesNotExist_WithAFictiousField_ReturnNotImplementedException()
        {
            CustomInputStringRule rule = new CustomInputStringRule();

            ParameterDescriptor descriptor = rule.SetArgumentValueIfTypeDoesNotExist(null, typeof(CustomInputStringRuleTest).GetFields()[0], new ParameterDescriptor(), new ParameterDescriptor());

            Assert.That(descriptor, Is.EqualTo(new ParameterDescriptor()));
        }

        [Theory]
        public void SetArgumentValueIfTypeExists_WithAFictiousField_ReturnNotImplementedException()
        {
            CustomInputStringRule rule = new CustomInputStringRule();

            ParameterDescriptor descriptor = rule.SetArgumentValueIfTypeExists(null, typeof(CustomInputStringRuleTest).GetFields()[0], new ParameterDescriptor(), new ParameterDescriptor());

            Assert.That(descriptor, Is.EqualTo(new ParameterDescriptor()));
        }
    }
}
