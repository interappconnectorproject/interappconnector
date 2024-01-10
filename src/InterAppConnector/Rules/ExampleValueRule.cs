using InterAppConnector.Attributes;
using InterAppConnector.DataModels;
using InterAppConnector.Interfaces;
using System.Reflection;

namespace InterAppConnector.Rules
{
    public class ExampleValueRule : IArgumentDefinitionRule<ExampleValueAttribute>
    {
        public ParameterDescriptor DefineArgumentIfTypeDoesNotExist(object parentObject, PropertyInfo property, ParameterDescriptor descriptor)
        {
            return descriptor;
        }

        public ParameterDescriptor DefineArgumentIfTypeDoesNotExist(object parentObject, FieldInfo property, ParameterDescriptor descriptor)
        {
            return descriptor;
        }

        public ParameterDescriptor DefineArgumentIfTypeExists(object parentObject, PropertyInfo property, ParameterDescriptor descriptor)
        {
            ExampleValueAttribute? attribute = property.GetCustomAttribute<ExampleValueAttribute>();
            if (string.IsNullOrEmpty(attribute!.ExampleValue))
            {
                throw new ArgumentException("The example value cannot be null or empty", property.Name);
            }
            return descriptor;
        }

        public ParameterDescriptor DefineArgumentIfTypeExists(object parentObject, FieldInfo field, ParameterDescriptor descriptor)
        {
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
    }
}
