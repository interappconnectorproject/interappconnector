using InterAppConnector.DataModels;
using InterAppConnector.Interfaces;
using NUnit.Framework;
using System.Reflection;

namespace InterAppConnector.Test.Library.Rules
{
    internal class GenericRuleTest : IArgumentDefinitionRule<int>, IArgumentSettingRule<int>
    {
        public ParameterDescriptor DefineArgumentIfTypeDoesNotExist(object parentObject, PropertyInfo property, ParameterDescriptor descriptor)
        {
            throw new NotImplementedException();
        }

        public ParameterDescriptor DefineArgumentIfTypeDoesNotExist(object parentObject, FieldInfo property, ParameterDescriptor descriptor)
        {
            throw new NotImplementedException();
        }

        public ParameterDescriptor DefineArgumentIfTypeExists(object parentObject, FieldInfo field, ParameterDescriptor descriptor)
        {
            throw new NotImplementedException();
        }

        public ParameterDescriptor DefineArgumentIfTypeExists(object parentObject, PropertyInfo property, ParameterDescriptor descriptor)
        {
            throw new NotImplementedException();
        }

        public bool IsRuleEnabledInArgumentDefinition(PropertyInfo property)
        {
            return false;
        }

        public bool IsRuleEnabledInArgumentDefinition(FieldInfo field)
        {
            return false;
        }

        public bool IsRuleEnabledInArgumentSetting(PropertyInfo property)
        {
            return false;
        }

        public bool IsRuleEnabledInArgumentSetting(FieldInfo field)
        {
            return false;
        }

        public ParameterDescriptor SetArgumentValueIfTypeDoesNotExist(object parentObject, PropertyInfo property, ParameterDescriptor argumentDescriptor, ParameterDescriptor userValueDescriptor)
        {
            throw new NotImplementedException();
        }

        public ParameterDescriptor SetArgumentValueIfTypeDoesNotExist(object parentObject, FieldInfo property, ParameterDescriptor argumentDescriptor, ParameterDescriptor userValueDescriptor)
        {
            throw new NotImplementedException();
        }

        public ParameterDescriptor SetArgumentValueIfTypeExists(object parentObject, PropertyInfo property, ParameterDescriptor argumentDescriptor, ParameterDescriptor userValueDescriptor)
        {
            throw new NotImplementedException();
        }

        public ParameterDescriptor SetArgumentValueIfTypeExists(object parentObject, FieldInfo property, ParameterDescriptor argumentDescriptor, ParameterDescriptor userValueDescriptor)
        {
            throw new NotImplementedException();
        }

        [Theory]
        public void TestDefineRule_ShouldReturnTrue()
        {
            GenericRuleTest test = new GenericRuleTest();

            bool returnTrue = ((IArgumentDefinitionRule<int>)test).ReturnTrue(23);

            Assert.That(returnTrue, Is.True);
        }

        [Theory]
        public void TestSetRule_ShouldReturnTrue()
        {
            GenericRuleTest test = new GenericRuleTest();

            bool returnTrue = ((IArgumentSettingRule<int>)test).ReturnTrue(23);

            Assert.That(returnTrue, Is.True);
        }
    }
}
