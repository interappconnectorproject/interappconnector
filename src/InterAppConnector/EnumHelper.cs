using InterAppConnector.DataModels;
using InterAppConnector.Exceptions;
using InterAppConnector.Interfaces;
using System.Reflection;

namespace InterAppConnector
{
    public class EnumHelper
    {
        internal Dictionary<string, ParameterDescriptor> _parameters = new Dictionary<string, ParameterDescriptor>();

        internal void LoadEnumerationValues<EnumType>() where EnumType : Enum
        {
            Type argumentBaseType = typeof(EnumType);
            foreach (FieldInfo item in argumentBaseType.GetFields())
            {
                // take only the value defined by the user
                if (!item.IsSpecialName)
                {
                    List<Attribute> distinctAttributes = RuleManager.GetDistinctAttributes(item);
                    ParameterDescriptor descriptor = new ParameterDescriptor();
                    List<IArgumentDefinitionRule> rulesToExecute = RuleManager.GetAssemblyRules<IArgumentDefinitionRule>(typeof(CommandManager));

                    foreach (Attribute attribute in distinctAttributes)
                    {
                        rulesToExecute = RuleManager.MergeRules(rulesToExecute, RuleManager.GetAssemblyRules<IArgumentDefinitionRule>(item.DeclaringType!));
                    }

                    foreach (var rule in from IArgumentDefinitionRule rule in rulesToExecute
                                         where rule.IsRuleEnabledInArgumentDefinition(item)
                                         select rule)
                    {
                        try
                        {
                            Type argumentType = rule.GetType().GetInterface(typeof(IArgumentDefinitionRule<>).FullName!)!.GetGenericArguments()[0];

                            Attribute? attributeExists = (from attributeItem in distinctAttributes
                                                          where attributeItem.GetType() == argumentType
                                                          select attributeItem).FirstOrDefault();

                            if (attributeExists != default(Attribute))
                            {
                                descriptor = rule.DefineArgumentIfTypeExists(argumentBaseType, item, descriptor);
                            }
                            else
                            {
                                descriptor = rule.DefineArgumentIfTypeDoesNotExist(argumentBaseType, item, descriptor);
                            }
                        }
                        catch
                        {
                            /*
                             * A default rule does not have an interface with an argument type,
                             * so you have to consider also this case
                             */
                            descriptor = rule.DefineArgumentIfTypeExists(argumentBaseType, item, descriptor);
                        }
                    }

                    if (!string.IsNullOrEmpty(descriptor.Name))
                    {
                        _parameters.Add(item.Name.ToLower().Trim(), descriptor);
                    }                  
                }
            }
        }

        internal void LoadEnumerationValues(Type enumType)
        {
            if (enumType.IsEnum)
            {
                try
                {
                    /*
                     * The class to change is this one, so a new constructor is not needed.
                     * Moreover, as the LoadEnumerationValues is internal to this class and it is not public,
                     * it is necessary the BindingFlags attributes in order to invoke the method
                     */
                    #pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
                    typeof(EnumHelper).GetMethod("LoadEnumerationValues", BindingFlags.Instance | BindingFlags.NonPublic, Type.EmptyTypes)!.MakeGenericMethod(enumType).Invoke(this, null);
                    #pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
                }
                catch (Exception exc)
                {
                    throw exc.InnerException;
                }
            }
            else
            {
                throw new TypeMismatchException(typeof(Enum).FullName!, enumType.FullName!, null!, enumType.FullName + " is not an enumeration");
            }
        }

        /// <summary>
        /// Gets the first alias or the field name of the field in the enumeration that can be assigned 
        /// to a dynamic property in the case of a obfuscated code
        /// </summary>
        /// <typeparam name="EnumType">The enum type</typeparam>
        /// <param name="fieldName">The field name</param>
        /// <returns>a string that represent the field name or the first alias of the field</returns>
        public static string GetFieldName<EnumType>(string fieldName) where EnumType : Enum
        {
            string enumValue = "";
            EnumHelper helper = new EnumHelper();
            helper.LoadEnumerationValues<EnumType>();
            List<ParameterDescriptor> parameters = (from item in helper._parameters.Values
                                                    where item.OriginalPropertyName == fieldName
                                                    select item).ToList();
            if (parameters.Count > 0)
            {
                if (parameters[0].Aliases.Count > 0)
                {
                    enumValue = parameters[0].Aliases[0];
                }
                else
                {
                    enumValue = parameters[0].OriginalPropertyName;
                }
            }
            return enumValue;
        }

        /// <summary>
        /// Get the enumeration field associated with the value
        /// </summary>
        /// <param name="enumType">The enum type</param>
        /// <param name="value">The value to search in the enumeration</param>
        /// <returns>The enumeration value</returns>
        /// <exception cref="TypeMismatchException">Exception raised when the type chosen is not an enumeration</exception>
        public static object GetEnumerationFieldByValue(Type enumType, string value)
        {
            if (enumType.IsEnum)
            {
                try
                {
                    return typeof(EnumHelper).GetMethod("GetEnumerationFieldByValue", 1, new[] { typeof(string) })!.MakeGenericMethod(enumType).Invoke(null, new object[] { value })!;
                }
                catch (Exception exc)
                {
                    throw exc.InnerException!;
                }
            }
            else
            {
                throw new TypeMismatchException(typeof(Enum).FullName!, enumType.FullName!, null!, enumType.FullName + " is not an enumeration");
            }
        }

        /// <summary>
        /// Get the enumeration value by searching for his value and alias
        /// </summary>
        /// <typeparam name="EnumType">The enun type</typeparam>
        /// <param name="value">The value to search</param>
        /// <returns>The enumeration vaalue</returns>
        /// <exception cref="ArgumentException">Exception raised when the value does not belong to the enumeration</exception>
        public static EnumType GetEnumerationFieldByValue<EnumType>(string value) where EnumType : Enum
        {
            EnumHelper helper = new EnumHelper();
            helper.LoadEnumerationValues<EnumType>();

            int parsedNumber;
            string stringToEvaluate = "";

            foreach (ParameterDescriptor descriptor in helper._parameters.Values)
            {
                if (int.TryParse(value.Trim(), out parsedNumber))
                {
                    if ((int) descriptor.Value == parsedNumber)
                    {
                        stringToEvaluate = parsedNumber.ToString();
                    }
                }
                else
                {
                    if (descriptor.Aliases.Count > 0)
                    {
                        if (descriptor.Aliases.IndexOf(value.ToLower().Trim()) > -1)
                        {
                            stringToEvaluate = descriptor.OriginalPropertyName;
                        }
                    }
                    else
                    {
                        if (descriptor.Name.ToLower().Trim() == value.ToLower().Trim())
                        {
                            stringToEvaluate = descriptor.OriginalPropertyName;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(stringToEvaluate))
            {
                /**
                 * The checks and the validations done might be enough to ensure that
                 * the value belongs to the enumeration type, so the Parse method should not
                 * raise an exception 
                 */
                return (EnumType) Enum.Parse(typeof(EnumType), stringToEvaluate);
            }

            /*
             * If something went wrong with the parse of the value, thow an exception.
             * Do not change this exception, as this must contain useful information regarding the object to change and the value provided.
             * Instead, make changes to the exception in CommandManager.SetArguments() that contain the exact argument used by the user.
             */
            throw new ArgumentException("The value " + value + " does not belong to " + typeof(EnumType).FullName, typeof(EnumType).FullName);
        }
    }
}
