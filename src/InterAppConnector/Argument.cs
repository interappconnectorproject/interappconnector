using System.Dynamic;
using System.Reflection;
using InterAppConnector.Attributes;
using InterAppConnector.DataModels;

namespace InterAppConnector
{
    /// <summary>
    /// Manages the command arguments
    /// </summary>
    public class Argument
    {
        protected Dictionary<string, ParameterDescriptor> _parameters = new Dictionary<string, ParameterDescriptor>();
        protected List<string> _action = new();

        /// <summary>
        /// The list of the actions selected by the user
        /// </summary>
        public List<string> Action
        {
            get
            {
                return _action;
            }
        }

        internal Dictionary<string, ParameterDescriptor> Arguments
        {
            get
            {
                return _parameters;
            }
        }

        public Argument AddArgument(string name, object value = null)
        {
            AddArgument(new ParameterDescriptor
            {
                Name = name,
                Value = value
            });
            return this;
        }

        public Argument AddArgument(string name, string description, object value = null)
        {
            AddArgument(new ParameterDescriptor
            {
                Name = name,
                Description = description,
                Value = value
            });
            return this;
        }

        public Argument AddArgument(ParameterDescriptor argumentDescriptor)
        {
            if (!_parameters.ContainsKey(argumentDescriptor.Name.ToLower()))
            {
                _parameters.Add(argumentDescriptor.Name.ToLower(), argumentDescriptor);
            }
            return this;
        }

        public object GetParameterValue(string parameterName)
        {
            object returnedObject = null;
            if (_parameters.ContainsKey(parameterName.ToLower().Trim()))
            {
                returnedObject = _parameters[parameterName.ToLower().Trim()].Value;
            }
            return returnedObject;
        }

        /// <summary>
        /// Check if a parameter is defined. This method does not check the value of the parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <returns>true if the parameter exists, otherwise false</returns>
        public bool IsParameterDefined(string name)
        {
            bool parameterExists = false;
            if (!string.IsNullOrEmpty(GetPrincipalParameter(name)))
            {
                parameterExists = true;
            }
            return parameterExists;
        }

        public bool HasParameterAValue(string name)
        {
            bool hasValue = false;
            if (IsParameterDefined(name))
            {
                hasValue = true;
            }
            return hasValue;
        }

        /// <summary>
        /// Get the first entry of the custom attribute with the type in <typeparamref name="ParameterType"/>, if exists,
        /// otherwise return <see langword="null"/>
        /// </summary>
        /// <typeparam name="ParameterType">The type of the attribute to find</typeparam>
        /// <param name="parameterName">The name of the parameter</param>
        /// <returns>The first entry of the attribute with type <typeparamref name="ParameterType"/>, otherwise <see langword="null"/></returns>
        public ParameterType GetCustomAttribute<ParameterType>(string parameterName) where ParameterType : Attribute
        {
            ParameterType parameter = null;

            if (IsParameterDefined(parameterName))
            {
                foreach (object item in _parameters[parameterName].Attributes)
                {
                    if (item.GetType() == typeof(ParameterType) && parameter == null)
                    {
                        parameter = (ParameterType)item;
                    }
                }
            }

            return parameter;
        }

        /// <summary>
        /// Get the first alias assigned to the property or the property name if no aliases were found.<br/>
        /// It returns <see langword="null"/> if the property does not belong to the argument defined in <typeparamref name="ArgumentType"/><br/>
        /// This method is useful in a scenario where code obfuscation is necessary
        /// </summary>
        /// <typeparam name="ArgumentType">The argument type</typeparam>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>a string with the first alias or the property name if the property exists, otherwise <see langword="null"/></returns>
        public static string? GetArgumentName<ArgumentType>(string propertyName)
        {
            string? argumentName = null;
            PropertyInfo? propertyFound = typeof(ArgumentType).GetProperty(propertyName);
            if (propertyFound != null)
            {
                if (propertyFound.GetCustomAttributes<AliasAttribute>().Count() > 0)
                {
                    argumentName = propertyFound.GetCustomAttributes<AliasAttribute>().First().Name;
                }
                else
                {
                    argumentName = propertyFound.Name;
                }
            }

            return argumentName;
        }

        private string GetPrincipalParameter(string parameterName)
        {
            string principalParameter = null;

            if (_parameters.ContainsKey(parameterName.ToLower().Trim()))
            {
                principalParameter = parameterName.ToLower().Trim();
            }
            else
            {
                ParameterDescriptor argument = (from item in _parameters.Values
                                                where item.Aliases.IndexOf(parameterName.ToLower().Trim()) > -1
                                                select item).FirstOrDefault();

                if (argument != default(ParameterDescriptor))
                {
                    principalParameter = argument.Name.ToLower().Trim();
                }

            }
            return principalParameter;
        }

        protected Argument ParseInternal(string[] arguments, string argumentPrefixes)
        {
            Queue<string> argumentList = new Queue<string>(arguments);
            ParameterDescriptor descriptor = null;
            while (argumentList.Count > 0)
            {
                string argument = argumentList.Dequeue();

                /**
                 * An argument must not be a number.
                 * However, there may be cases where you want to pass a negative number in an argument
                 * In this case, the string is considered a value of the latest argument declared at that time
                 */

                if (IsArgumentWithPrefix(argument, argumentPrefixes))
                {
                    double number;
                    
                    if (double.TryParse(argument, out number))
                    {
                        if (descriptor != null && descriptor.Value == null)
                        {
                            descriptor.Value = number.ToString();
                            AddArgument(descriptor);
                            descriptor = null;
                        }
                    }
                    else
                    {
                        if (descriptor != null)
                        {
                            if (descriptor.Value == null)
                            {
                                descriptor.Value = true;
                            }
                            AddArgument(descriptor);
                            descriptor = null;
                        }
                        descriptor = new ParameterDescriptor();
                        descriptor.Name = argument.Substring(argumentPrefixes.Length);
                    }
                }
                else
                {
                    if (descriptor == null)
                    {
                        Action.Add(argument);
                    }
                    else
                    {
                        descriptor.Value = argument;
                    }
                }
            }

            if (descriptor != null)
            {
                if (descriptor.Value == null)
                {
                    descriptor.Value = true;
                }
                AddArgument(descriptor);
            }

            return this;
        }

        /// <summary>
        /// Parse the arguments. This command should be used when arguments are defined as an array of string, for example when the command is called
        /// via a command line interface
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="argumentPrefixes"></param>
        /// <returns></returns>
        public static Argument Parse(string[] arguments, string argumentPrefixes)
        {
            Argument result = new Argument();
            return result.ParseInternal(arguments, argumentPrefixes);
        }

        public static Argument Parse(dynamic arguments, string argumentPrefix)
        {
            Argument result = new Argument();
            List<string> argumentList = new List<string>();
            ExpandoObject args = arguments;
            foreach (KeyValuePair<string, object?> item in args)
            {
                if (item.Value is not null)
                {
                    argumentList.Add(argumentPrefix + item.Key);
                    if (item.Value is not bool)
                    {
                        argumentList.Add(item.Value.ToString());
                    }
                }
            }
            return result.ParseInternal(argumentList.ToArray(), argumentPrefix);
        }

        protected static bool IsArgumentWithPrefix(string argument, string argumentPrefixes)
        {
            bool isArgumentWithPrefix = false;
            string[] argumentPrefixList = argumentPrefixes.Split(",");
            foreach (string argumentPrefix in argumentPrefixList)
            {
                if (argument.ToLower().StartsWith(argumentPrefix.ToLower()))
                {
                    isArgumentWithPrefix = true;
                }
            }
            return isArgumentWithPrefix;
        }
    }
}
