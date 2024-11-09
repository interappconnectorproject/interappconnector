using InterAppConnector.DataModels;
using InterAppConnector.Interfaces;
using InterAppConnector.Test.Library.DataModels;
using NUnit.Framework;
using System.Reflection;

namespace InterAppConnector.Test.Library.Rules
{
    public class ArgumentWithCustomDefinitionRuleRule : IArgumentDefinitionRule<ArgumentWithCustomDefinitionRule>
    {
        public ParameterDescriptor DefineArgumentIfTypeDoesNotExist(object parentObject, PropertyInfo property, ParameterDescriptor descriptor)
        {
            return descriptor;
        }

        public ParameterDescriptor DefineArgumentIfTypeDoesNotExist(object parentObject, FieldInfo property, ParameterDescriptor descriptor)
        {
            return descriptor;
        }

        public ParameterDescriptor DefineArgumentIfTypeExists(object parentObject, FieldInfo field, ParameterDescriptor descriptor)
        {
            return descriptor;
        }

        public ParameterDescriptor DefineArgumentIfTypeExists(object parentObject, PropertyInfo property, ParameterDescriptor descriptor)
        {
            ArgumentWithCustomDefinitionRule rule = new ArgumentWithCustomDefinitionRule
            {
                Count = 1
            };

            if (property.GetValue(parentObject) == null)
            {
                property.SetValue(parentObject, rule);
                descriptor.Value = rule;
            }

            return descriptor;

        }

        public bool IsRuleEnabledInArgumentDefinition(PropertyInfo property)
        {
            return true;
        }

        public bool IsRuleEnabledInArgumentDefinition(FieldInfo field)
        {
            return false;
        }

        [Theory]
        public void TestDefineRule_ShouldReturnTrue()
        {
            ArgumentWithCustomDefinitionRuleRule test = new ArgumentWithCustomDefinitionRuleRule();

            bool returnTrue = ((IArgumentDefinitionRule<ArgumentWithCustomDefinitionRule>)test).ReturnTrue(new ArgumentWithCustomDefinitionRule());

            Assert.That(returnTrue, Is.True);
        }
    }
}
