using InterAppConnector.Attributes;
using InterAppConnector.DataModels;
using InterAppConnector.Interfaces;
using System.Reflection;

namespace InterAppConnector.Rules
{
    public class MandatoryForCommandRule : IArgumentDefinitionRule<MandatoryForCommandAttribute>
    {
        public ParameterDescriptor DefineArgumentIfTypeDoesNotExist(object parentObject, PropertyInfo property, ParameterDescriptor descriptor)
        {
            if (property.PropertyType.IsValueType)
            {
                if (Nullable.GetUnderlyingType(property.PropertyType) == null)
                {
                    descriptor.IsMandatory = true;
                }
            }
            else
            {
                if (property.GetValue(parentObject) != null)
                {
                    property.SetValue(parentObject, null);
                    descriptor.IsMandatory = true;
                }
            }
            return descriptor;
        }

        public ParameterDescriptor DefineArgumentIfTypeDoesNotExist(object parentObject, FieldInfo property, ParameterDescriptor descriptor)
        {
            return descriptor;
        }

        public ParameterDescriptor DefineArgumentIfTypeExists(object parentObject, PropertyInfo property, ParameterDescriptor descriptor)
        {
            foreach (MandatoryForCommandAttribute propertyAttribute in property.GetCustomAttributes<MandatoryForCommandAttribute>())
            {
                if (propertyAttribute.Command == CommandManager.CurrentCommand!.GetType())
                {
                    descriptor.IsMandatory = true;
                }
            }
            return descriptor;
        }

        public ParameterDescriptor DefineArgumentIfTypeExists(object parentObject, FieldInfo field, ParameterDescriptor descriptor)
        {
            return descriptor;
        }

        public bool IsRuleEnabledInArgumentDefinition(PropertyInfo property)
        {
            bool isRuleEnabled = true;

            if (property == null || property.GetCustomAttribute<ExcludeItemFromCommandAttribute>() != null)
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
