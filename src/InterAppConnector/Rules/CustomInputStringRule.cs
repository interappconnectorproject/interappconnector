using InterAppConnector.Attributes;
using InterAppConnector.DataModels;
using InterAppConnector.Exceptions;
using InterAppConnector.Interfaces;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace InterAppConnector.Rules
{
    public class CustomInputStringRule : IArgumentSettingRule<CustomInputStringAttribute>
    {
        public bool IsRuleEnabledInArgumentSetting(PropertyInfo property)
        {
            /*
             * If the parameter is a value, the value is directly assigned to the parameter.
             * But what about a class that receives a custom InputString as his parameter?
             * Generally speaking, the command class may have only one method between the constructor that
             * receive a string as argument and the Parse method
             * Don't forget that there is also the case when CustomInputStringAttribute is not assigned
             * or there is a custom method that needs to be used 
             * There should be only one and one method with this attribute, and if more methods are found that have this attribute
             * raise a DuplicateObjectException exception
             * A string is a class, so you have to treat it as a special object.
             * What about parameters that are structs, for example in the case of Guid? Actually check only if it is a struct,
             * so check if the type is a value type and it is not an enumeration and a primitive
             */
            Type? parameterType = Nullable.GetUnderlyingType(property.PropertyType);
            if (parameterType == null)
            {
                parameterType = property.PropertyType;
            }
            return (parameterType.IsClass || StructHelper.IsStruct(parameterType)) && parameterType != typeof(string);
        }

        public bool IsRuleEnabledInArgumentSetting(FieldInfo field)
        {
            return false;
        }

        public ParameterDescriptor SetArgumentValueIfTypeDoesNotExist(object parentObject, FieldInfo property, ParameterDescriptor argumentDescriptor, ParameterDescriptor userValueDescriptor)
        {
            return argumentDescriptor;
        }

        public ParameterDescriptor SetArgumentValueIfTypeDoesNotExist(object parentObject, PropertyInfo property, ParameterDescriptor argumentDescriptor, ParameterDescriptor userValueDescriptor)
        {
            List<MethodInfo> methodsWithCustomInputStringAttribute = (from method in argumentDescriptor.ParameterType.GetMethods()
                                                                      where method.GetCustomAttribute<CustomInputStringAttribute>() != null
                                                                      select method).ToList();
            switch (methodsWithCustomInputStringAttribute.Count)
            {
                case 0:
                    // Check if there is a constructor that accepts a string as parameter
                    ConstructorInfo? constructor = argumentDescriptor.ParameterType.GetConstructor(new[] { typeof(string) });
                    if (constructor != null)
                    {
                        try
                        {
                            argumentDescriptor.Value = constructor.Invoke(new[] { userValueDescriptor.Value });
                            property.SetValue(parentObject, argumentDescriptor.Value);
                            argumentDescriptor.IsSetByUser = true;
                        }
                        catch (Exception exc)
                        {
                            throw new ArgumentException("The value provided to argument " + userValueDescriptor.Name + " is not acceptable. Reason: " + exc.GetBaseException().Message, userValueDescriptor.Name, exc.InnerException);
                        }
                    }
                    else
                    {
                        throw new MethodNotFoundException(property.PropertyType.Name, "Cannot find a public constructor that accepts a string as parameter in class " + property.PropertyType.Name);
                    }
                    break;
                case 1:
                        if (methodsWithCustomInputStringAttribute[0].GetParameters().Length != 1)
                        {
                            throw new TypeMismatchException(typeof(string).FullName!, "More than 1 parameter", "", "Error in argument definition. It is expected one parameter of type string, but the method has multiple parameters.");
                        }

                        if (methodsWithCustomInputStringAttribute[0].GetParameters()[0].ParameterType == typeof(string))
                        {
                            try
                            {
                                argumentDescriptor.Value = methodsWithCustomInputStringAttribute[0].Invoke(null, new[] { userValueDescriptor.Value })!;
                                property.SetValue(parentObject, argumentDescriptor.Value);
                                argumentDescriptor.IsSetByUser = true;
                            }
                            catch (Exception exc)
                            {
                                throw new ArgumentException("The value provided to argument " + userValueDescriptor.Name + " is not acceptable. Reason: " + exc.GetBaseException().Message, userValueDescriptor.Name, exc.InnerException);
                            }
                        }
                        else
                        {
                            throw new TypeMismatchException(typeof(string).FullName!, methodsWithCustomInputStringAttribute[0].GetParameters()[0].ParameterType.FullName!, "", "Error in argument definition. It is expected a string, but the actual type is " + methodsWithCustomInputStringAttribute[0].GetParameters()[0].ParameterType.FullName);
                        }
                    break;
                default:
                    throw new DuplicateObjectException(methodsWithCustomInputStringAttribute[1].Name, new List<string> { "[CustomInputString] attribute" }, methodsWithCustomInputStringAttribute[0].Name, "Error in class " + property.PropertyType.Name + ". It is expected that only one method may have the [CustomInputString] attribute, but there are " + methodsWithCustomInputStringAttribute.Count + " methods with this attribute");
            }

            return argumentDescriptor;
        }

        public ParameterDescriptor SetArgumentValueIfTypeExists(object parentObject, PropertyInfo property, ParameterDescriptor argumentDescriptor, ParameterDescriptor userValueDescriptor)
        {
            if (string.IsNullOrEmpty(property.GetCustomAttribute<CustomInputStringAttribute>()!.StringFormat))
            {
                /*
                 * CustomInputStringAttribute without arguments is redundant and should not be used.
                 * However it may happen that it is used by mistake or for some incomprehension,
                 * so consider this case
                 */
                argumentDescriptor = SetArgumentValueIfTypeDoesNotExist(parentObject, property, argumentDescriptor, userValueDescriptor);
            }
            else
            {
                try
                {
                    // Use ParseExact method
                    argumentDescriptor.Value = argumentDescriptor.ParameterType.GetMethod("ParseExact", new[] { typeof(string), typeof(string), typeof(IFormatProvider) })!.Invoke(null, new[] { userValueDescriptor.Value, property.GetCustomAttribute<CustomInputStringAttribute>()!.StringFormat, CultureInfo.InvariantCulture })!;
                    property.SetValue(parentObject, argumentDescriptor.Value);
                    argumentDescriptor.IsSetByUser = true;
                }
                catch (Exception exc)
                {
                    throw new ArgumentException("The value provided to argument " + userValueDescriptor.Name + " is not acceptable. Reason: " + exc.GetBaseException().Message, userValueDescriptor.Name, exc.InnerException);
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
