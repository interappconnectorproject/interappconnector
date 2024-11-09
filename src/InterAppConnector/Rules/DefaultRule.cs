using InterAppConnector.Attributes;
using InterAppConnector.DataModels;
using InterAppConnector.Interfaces;
using System.Reflection;

namespace InterAppConnector.Rules
{
    public class DefaultRule : IArgumentDefinitionRule, IArgumentSettingRule
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
            descriptor.OriginalPropertyName = property.Name;

            Type? parameterType = Nullable.GetUnderlyingType(property.PropertyType);

            if (parameterType == null)
            {
                descriptor.ParameterType = property.PropertyType;
            }
            else
            {
                descriptor.ParameterType = parameterType;
            }

            descriptor.Attributes = property.GetCustomAttributes().ToList<object>();
            descriptor.Value = property.GetValue(parentObject)!;

            return descriptor;
        }

        public ParameterDescriptor DefineArgumentIfTypeExists(object parentObject, FieldInfo field, ParameterDescriptor descriptor)
        {
            descriptor.OriginalPropertyName = field.Name;
            descriptor.Value = (int) field.GetValue(parentObject)!;
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

        public bool IsRuleEnabledInArgumentSetting(PropertyInfo property)
        {
            bool isRuleEnabled = true;

            if (property != null)
            {
                Type? parameterType = Nullable.GetUnderlyingType(property.PropertyType);
                if (parameterType == null)
                {
                    parameterType = property.PropertyType;
                }
                bool isEnumType = parameterType.IsEnum;
                bool isValueTypeAndIsNotAStruct = parameterType.IsValueType && !StructHelper.IsStruct(parameterType);
                isRuleEnabled = isEnumType || isValueTypeAndIsNotAStruct || property.PropertyType == typeof(string);
            }

            return isRuleEnabled;
        }

        public bool IsRuleEnabledInArgumentSetting(FieldInfo field)
        {
            return false;
        }

        public ParameterDescriptor SetArgumentValueIfTypeDoesNotExist(object parentObject, PropertyInfo property, ParameterDescriptor argumentDescriptor, ParameterDescriptor userValueDescriptor)
        {
            return argumentDescriptor;
        }

        public ParameterDescriptor SetArgumentValueIfTypeDoesNotExist(object parentObject, FieldInfo property, ParameterDescriptor argumentDescriptor, ParameterDescriptor userValueDescriptor)
        {
            return argumentDescriptor;
        }

        public ParameterDescriptor SetArgumentValueIfTypeExists(object parentObject, PropertyInfo property, ParameterDescriptor argumentDescriptor, ParameterDescriptor userValueDescriptor)
        {
            if (argumentDescriptor.ParameterType.IsEnum)
            {
                /*
                 * Remember that a value in an enumeration may have different aliases that works 
                 * like aliases in properties, with the main difference that refers to a value instead of
                 * the property.
                 * Aliases can indentify a value uniquely and could mask the original value name, but be careful for batch execution, where the value
                 * passed to the property is also the name of the property
                 */
                try
                {
                    argumentDescriptor.Value = EnumHelper.GetEnumerationFieldByValue(argumentDescriptor.ParameterType, userValueDescriptor.Value.ToString()!);
                    property.SetValue(parentObject, EnumHelper.GetEnumerationFieldByValue(argumentDescriptor.ParameterType, userValueDescriptor.Value.ToString()!));
                    argumentDescriptor.IsSetByUser = true;
                }
                catch (ArgumentException exc)
                {
                    /*
                     * If something went wrong with the parse of the value, throw an exception.
                     * You may need to change this exception with a more specialized exception,
                     * but the InnerException property should contain the exception raised by
                     * EnumHelper.GetFieldName()
                     */
                    throw new ArgumentException("The value provided in parameter " + userValueDescriptor.Name + " is not acceptable. Please provide a new value", userValueDescriptor.Name, exc);
                }
            }
            else
            {
                try
                {
                    /*
                     * During the parse of the arguments, if an argument is passed without a value is considered a switch 
                     * This is not always true, especially if the argument is placed as last token in the argument definition
                     * So if the type of the argument is not a boolean, you have to specify a value explicitly
                     */
                    if (argumentDescriptor.ParameterType != typeof(bool) && !(userValueDescriptor.Value is bool))
                    {
                        argumentDescriptor.Value = Convert.ChangeType(userValueDescriptor.Value, argumentDescriptor.ParameterType);
                        property.SetValue(parentObject, Convert.ChangeType(userValueDescriptor.Value, argumentDescriptor.ParameterType));
                        argumentDescriptor.IsSetByUser = true;
                    }
                    else
                    {
                        if (argumentDescriptor.ParameterType == typeof(bool))
                        {
                            argumentDescriptor.Value = Convert.ChangeType(userValueDescriptor.Value, argumentDescriptor.ParameterType);
                            property.SetValue(parentObject, Convert.ChangeType(userValueDescriptor.Value, argumentDescriptor.ParameterType));
                            argumentDescriptor.IsSetByUser = true;
                        }
                        else
                        {
                            throw new ArgumentException("The value provided to argument " + userValueDescriptor.Name + " is not acceptable. Please provide a valid value", userValueDescriptor.Name);
                        }
                    }
                }
                catch (Exception exc)
                {
                    throw new ArgumentException("The value provided to argument " + userValueDescriptor.Name + " is not acceptable. Please provide a valid value", userValueDescriptor.Name, exc);
                }
            }
            return argumentDescriptor;
        }

        public ParameterDescriptor SetArgumentValueIfTypeExists(object parentObject, FieldInfo property, ParameterDescriptor argumentDescriptor, ParameterDescriptor userValueDescriptor)
        {
            return argumentDescriptor;
        }
    }
}
