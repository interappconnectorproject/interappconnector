using InterAppConnector.DataModels;
using InterAppConnector.Interfaces;
using InterAppConnector.Test.Library.DataModels;
using NUnit.Framework;
using System.Reflection;

namespace InterAppConnector.Test.Library.Rules
{
    public class ArgumentWithCustomSettingRuleRule : IArgumentSettingRule<ArgumentWithCustomSettingRule>
    {
        public bool IsRuleEnabledInArgumentSetting(PropertyInfo property)
        {
            return true;
        }

        public bool IsRuleEnabledInArgumentSetting(FieldInfo field)
        {
            return false;
        }

        public ParameterDescriptor SetArgumentValueIfTypeDoesNotExist(object parentObject, PropertyInfo property, ParameterDescriptor argumentDescriptor, ParameterDescriptor userValueDescriptor)
        {
            if (property.GetValue(parentObject) == null)
            {
                ArgumentWithCustomSettingRule customSettingRule = new ArgumentWithCustomSettingRule("5");
                property.SetValue(parentObject, customSettingRule);
            }
            return argumentDescriptor;
        }

        public ParameterDescriptor SetArgumentValueIfTypeDoesNotExist(object parentObject, FieldInfo property, ParameterDescriptor argumentDescriptor, ParameterDescriptor userValueDescriptor)
        {
            throw new NotImplementedException();
        }

        public ParameterDescriptor SetArgumentValueIfTypeExists(object parentObject, PropertyInfo property, ParameterDescriptor argumentDescriptor, ParameterDescriptor userValueDescriptor)
        {
            ArgumentWithCustomSettingRule.RulesCalledForThisArgument.Add("ArgumentWithCustomSettingRuleRule.SetArgumentValueIfTypeExists(object,PropertyInfo,ParameterDescriptor,ParameterDescriptor)");
            return argumentDescriptor;
        }

        public ParameterDescriptor SetArgumentValueIfTypeExists(object parentObject, FieldInfo property, ParameterDescriptor argumentDescriptor, ParameterDescriptor userValueDescriptor)
        {
            throw new NotImplementedException();
        }

        [Theory]
        public void TestSetRule_ShouldReturnTrue()
        {
            ArgumentWithCustomSettingRuleRule test = new ArgumentWithCustomSettingRuleRule();

            bool returnTrue = ((IArgumentSettingRule<ArgumentWithCustomSettingRule>)test).ReturnTrue(new ArgumentWithCustomSettingRule("20"));

            Assert.That(returnTrue, Is.True);
        }
    }
}
