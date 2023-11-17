using InterAppConnector.Attributes;
using InterAppConnector.DataModels;
using InterAppConnector.Exceptions;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace InterAppConnector
{
    public class EnumHelper
    {
        internal Dictionary<string, ParameterDescriptor> _parameters = new Dictionary<string, ParameterDescriptor>();

        internal void LoadEnumerationValues<EnumType>() where EnumType : Enum
        {
            foreach (FieldInfo item in typeof(EnumType).GetFields())
            {
                // take only the value defined by the user
                if (!item.IsSpecialName)
                {
                    if (item.GetCustomAttribute<ExcludeItemFromCommandAttribute>() == null)
                    {
                        ParameterDescriptor descriptor = new ParameterDescriptor();
                        descriptor.OriginalPropertyName = item.Name;

                        if (item.GetCustomAttributes<AliasAttribute>().Count() > 0)
                        {
                            foreach (AliasAttribute alias in item.GetCustomAttributes<AliasAttribute>())
                            {
                                if (!string.IsNullOrWhiteSpace(alias.Name))
                                {
                                    double number;
                                    if (!double.TryParse(alias.Name.ToLower().Trim(), out number))
                                    {
                                        if (Regex.IsMatch(alias.Name.ToLower().Trim(), @"^[A-Za-z0-9-]+$", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1)))
                                        {
                                            descriptor.Aliases.Add(alias.Name.ToLower().Trim());
                                        }
                                        else
                                        {
                                            throw new ArgumentException("Invalid string found in " + alias.Name + ". Alias must contain only alphanumeric characters and hyphens (-). Null values, empty string and numbers are also not allowed");
                                        }
                                    }
                                    else
                                    {
                                        throw new ArgumentException("Invalid alias found in " + item.Name + ". The alias cannot be null, an empty string or a number", item.Name);
                                    }
                                }
                                else
                                {
                                    throw new ArgumentException("Invalid alias found in " + item.Name + ". The alias cannot be null, an empty string or a number", item.Name);
                                }
                            }

                            descriptor.Name = descriptor.Aliases[0].ToLower().Trim();
                            //descriptor.Aliases.RemoveAt(0);
                        }
                        else
                        {
                            descriptor.Name = item.Name.ToLower().Trim();
                        }

                        if (item.GetCustomAttribute<DescriptionAttribute>() != null)
                        {
                            descriptor.Description = item.GetCustomAttribute<DescriptionAttribute>().Description;
                        }

                        descriptor.Value = (int)item.GetValue(typeof(EnumType));

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
                    typeof(EnumHelper).GetMethod("LoadEnumerationValues", BindingFlags.Instance | BindingFlags.NonPublic, Type.EmptyTypes).MakeGenericMethod(enumType).Invoke(this, null);
                }
                catch (Exception exc)
                {
                    throw exc.InnerException;
                }
            }
            else
            {
                throw new TypeMismatchException(typeof(Enum).FullName, enumType.GetType().FullName, null, enumType.GetType().FullName + " is not an enumeration");
            }
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
                    return typeof(EnumHelper).GetMethod("GetEnumerationFieldByValue", 1, new[] { typeof(string) }).MakeGenericMethod(enumType).Invoke(null, new object[] { value });
                }
                catch (Exception exc)
                {
                    throw exc.InnerException;
                }
            }
            else
            {
                throw new TypeMismatchException(typeof(Enum).FullName, enumType.GetType().FullName, null, enumType.GetType().FullName + " is not an enumeration");
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
            object enumType;
            string stringToEvaluate = "";

            foreach (ParameterDescriptor descriptor in helper._parameters.Values)
            {
                if (int.TryParse(value.Trim(), out parsedNumber))
                {
                    if ((int) descriptor.Value == parsedNumber)
                    {
                        stringToEvaluate += parsedNumber;
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
                /*if (Enum.TryParse(typeof(EnumType), stringToEvaluate, true, out enumType))
                {
                    if (Enum.IsDefined(typeof(EnumType), enumType))
                    {
                        return (EnumType)enumType;
                    }
                }*/

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
