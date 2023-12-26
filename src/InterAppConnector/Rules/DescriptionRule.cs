using InterAppConnector.Attributes;
using InterAppConnector.DataModels;
using InterAppConnector.Interfaces;
using System.ComponentModel;
using System.Reflection;

namespace InterAppConnector.Rules
{
    public class DescriptionRule : IArgumentDefinitionRule<DescriptionAttribute>
    {
        public ParameterDescriptor DefineArgumentIfTypeDoesNotExist(object parentObject, PropertyInfo property, ParameterDescriptor descriptor)
        {
            descriptor.Description = "No description provided";
            return descriptor;
        }

        public ParameterDescriptor DefineArgumentIfTypeDoesNotExist(object parentObject, FieldInfo property, ParameterDescriptor descriptor)
        {
            descriptor.Description = "No description provided";
            return descriptor;
        }

        public ParameterDescriptor DefineArgumentIfTypeExists(object parentObject, PropertyInfo property, ParameterDescriptor descriptor)
        {
            DescriptionAttribute? attribute = property.GetCustomAttribute<DescriptionAttribute>();

            if (attribute != null)
            {
                descriptor.Description = attribute.Description;
            }
            
            return descriptor;
        }

        public ParameterDescriptor DefineArgumentIfTypeExists(object parentObject, FieldInfo field, ParameterDescriptor descriptor)
        {
            DescriptionAttribute? attribute = field.GetCustomAttribute<DescriptionAttribute>();

            if (attribute != null)
            {
                descriptor.Description = attribute.Description;
            }

            return descriptor;
        }

        public bool IsRuleEnabledInArgumentDefinition(PropertyInfo property)
        {
            bool isRuleEnabled = true;

            if (property.GetCustomAttribute<ExcludeItemFromCommandAttribute>() != null)
            {
                isRuleEnabled = false;
            }

            return isRuleEnabled;
        }

        public bool IsRuleEnabledInArgumentDefinition(FieldInfo field)
        {
            bool isRuleEnabled = true;

            if (field.GetCustomAttribute<ExcludeItemFromCommandAttribute>() != null)
            {
                isRuleEnabled = false;
            }

            return isRuleEnabled;
        }
    }
}
