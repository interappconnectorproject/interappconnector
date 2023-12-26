using InterAppConnector.Attributes;
using InterAppConnector.DataModels;
using InterAppConnector.Exceptions;
using InterAppConnector.Interfaces;
using System.Reflection;

namespace InterAppConnector.Rules
{
    public class ValueValidatorRule : IArgumentSettingRule<ValueValidatorAttribute>
    {
        public bool IsRuleEnabledInArgumentSetting(PropertyInfo property)
        {
            return true;
        }

        public bool IsRuleEnabledInArgumentSetting(FieldInfo field)
        {
            return true;
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
            ValueValidatorAttribute validator = property.GetCustomAttribute<ValueValidatorAttribute>()!;

            if (validator.ValueValidatorType.GetInterface(typeof(IValueValidator).Name) != null)
            {
                IValueValidator valueValidator = (IValueValidator)Activator.CreateInstance(validator.ValueValidatorType)!;
                bool customValidationErrorMessageMissing = false;

                try
                {
                    if (!valueValidator.ValidateValue(argumentDescriptor.Value))
                    {
                        customValidationErrorMessageMissing = true;
                    }
                }
                catch (Exception exc)
                {
                    throw new ArgumentException("The value provided to argument " + userValueDescriptor.Name + " is not acceptable. Reason: " + exc.GetBaseException().Message, userValueDescriptor.Name, exc.InnerException);
                }

                if (customValidationErrorMessageMissing)
                {
                    throw new ArgumentException("The value provided to argument " + userValueDescriptor.Name + " is not valid according to the validation procedure");
                }
            }
            else
            {
                throw new TypeMismatchException(typeof(IValueValidator).Name, "", "", validator.ValueValidatorType.Name + " doesn't have an interface of type " + typeof(IValueValidator).Name);
            }

            return argumentDescriptor;
        }

        public ParameterDescriptor SetArgumentValueIfTypeExists(object parentObject, FieldInfo property, ParameterDescriptor argumentDescriptor, ParameterDescriptor userValueDescriptor)
        {
            return argumentDescriptor;
        }
    }
}
