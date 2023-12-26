using InterAppConnector.Attributes;
using InterAppConnector.DataModels;
using InterAppConnector.Interfaces;
using System.Reflection;
using System.Text.RegularExpressions;

namespace InterAppConnector.Rules
{
    public class AliasRule : IArgumentDefinitionRule<AliasAttribute>
    {
        public ParameterDescriptor DefineArgumentIfTypeDoesNotExist(object parentObject, PropertyInfo property, ParameterDescriptor descriptor)
        {
            descriptor.Name = property.Name.ToLower().Trim();
            return descriptor;
        }

        public ParameterDescriptor DefineArgumentIfTypeDoesNotExist(object parentObject, FieldInfo property, ParameterDescriptor descriptor)
        {
            descriptor.Name = property.Name.ToLower().Trim();
            return descriptor;
        }

        public ParameterDescriptor DefineArgumentIfTypeExists(object parentObject, PropertyInfo property, ParameterDescriptor descriptor)
        {
            foreach (string name in property.GetCustomAttributes<AliasAttribute>().Select(x => x.Name))
            {
                if (Regex.IsMatch(name, @"^[A-Za-z0-9-]+$", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1)))
                {
                    if (!descriptor.Aliases.Contains(name.ToLower().Trim()))
                    {
                        descriptor.Aliases.Add(name.ToLower().Trim());
                    }
                }
                else
                {
                    throw new ArgumentException("Invalid string found in " + property.Name + ". Alias must contain only alphanumeric characters and hyphens (-). Null values or empty string are also not allowed");
                }
            }

            descriptor.Name = property.GetCustomAttributes<AliasAttribute>().First().Name.ToLower().Trim();
            return descriptor;
        }

        public ParameterDescriptor DefineArgumentIfTypeExists(object parentObject, FieldInfo field, ParameterDescriptor descriptor)
        {
            foreach (string name in field.GetCustomAttributes<AliasAttribute>().Select(x => x.Name))
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    double number;
                    if (!double.TryParse(name.ToLower().Trim(), out number)
                        && Regex.IsMatch(name.ToLower().Trim(), @"^[A-Za-z0-9-]+$", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1)))
                    {
                        if (!descriptor.Aliases.Contains(name.ToLower().Trim()))
                        {
                            descriptor.Aliases.Add(name.ToLower().Trim());
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Invalid alias found in " + field.Name + ". The alias must contain alphanumerical characters and hypens. It cannot be null, an empty string or a number", field.Name);
                    }
                }
                else
                {
                    throw new ArgumentException("Invalid alias found in " + field.Name + ". The alias cannot be null, an empty string or a number", field.Name);
                }
            }

            descriptor.Name = descriptor.Aliases[0].ToLower().Trim();
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
