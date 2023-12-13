using InterAppConnector.Attributes;
using InterAppConnector.DataModels;
using InterAppConnector.Exceptions;
using InterAppConnector.Interfaces;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("InterAppConnector.Test.Library")]
namespace InterAppConnector
{
    /// <summary>
    /// Manage the commands
    /// </summary>
    public class CommandManager
    {
        internal Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();
        internal Dictionary<string, Argument> _arguments = new Dictionary<string, Argument>();
        internal Dictionary<string, object> _parameterObject = new Dictionary<string, object>();
        internal string _result = "";

        internal string? GetActionKeyByActions(List<string> actions)
        {
            string? commandName = (from item in _arguments
                                   where item.Value.Action.Intersect(actions).Any()
                                   select item.Key).SingleOrDefault();
            return commandName;
        }

        internal CommandManager SetArguments(List<string> actions, List<ParameterDescriptor> argument)
        {
            string? selectedCommand = GetActionKeyByActions(actions);

            if (selectedCommand != default)
            {
                foreach (ParameterDescriptor item in argument)
                {
                    ParameterDescriptor? findArgument = (from value in _arguments[selectedCommand].Arguments.Values
                                                         where value.Name.ToLower().Trim() == item.Name.ToLower().Trim()
                                                         || value.Aliases.Any(s => s.ToLower().Trim() == item.Name.ToLower().Trim())
                                                         select value).SingleOrDefault();

                    if (findArgument != default(ParameterDescriptor))
                    {
                        ParameterDescriptor modifiedValues = _arguments[selectedCommand].Arguments[findArgument.Name];
                        Type? parameterType = Nullable.GetUnderlyingType(modifiedValues.ParameterType);
                        if (parameterType == null)
                        {
                            parameterType = modifiedValues.ParameterType;
                        }

                        if (parameterType.IsEnum)
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
                                _arguments[selectedCommand].Arguments[findArgument.Name].Value = EnumHelper.GetEnumerationFieldByValue(parameterType, item.Value.ToString());
                                _parameterObject[selectedCommand].GetType().GetProperty(findArgument.OriginalPropertyName)!.SetValue(_parameterObject[selectedCommand], EnumHelper.GetEnumerationFieldByValue(parameterType, item.Value.ToString()));
                                _arguments[selectedCommand].Arguments[findArgument.Name].IsSetByUser = true;
                            }
                            catch (ArgumentException exc)
                            {
                                /*
                                 * If something went wrong with the parse of the value, throw an exception.
                                 * You may need to change this exception with a more specialized exception,
                                 * but the InnerException property should contain the exception raised by
                                 * EnumHelper.GetFieldName()
                                 */
                                throw new ArgumentException("The value provided in parameter " + item.Name + " is not acceptable. Please provide a new value", item.Name, exc);
                            }
                        }
                        else if ((parameterType.IsClass || StructHelper.IsStruct(parameterType)) && parameterType != typeof(string))
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

                            if (_arguments[selectedCommand].GetCustomAttribute<CustomInputStringAttribute>(findArgument.Name) == null)
                            {
                                List<MethodInfo> methodsWithCustomInputStringAttribute = (from method in parameterType.GetMethods()
                                                                                          where method.GetCustomAttribute<CustomInputStringAttribute>() != null
                                                                                          select method).ToList();
                                if (methodsWithCustomInputStringAttribute.Count < 2)
                                {
                                    if (methodsWithCustomInputStringAttribute.Count == 0)
                                    {
                                        // Check if there is a constructor that accepts a string as parameter
                                        ConstructorInfo? constructor = parameterType.GetConstructor(new[] { typeof(string) });
                                        if (constructor != null)
                                        {
                                            try
                                            {
                                                _arguments[selectedCommand].Arguments[findArgument.Name].Value = constructor.Invoke(new[] { item.Value });
                                                _parameterObject[selectedCommand].GetType().GetProperty(findArgument.OriginalPropertyName)!.SetValue(_parameterObject[selectedCommand], constructor.Invoke(new[] { item.Value }));
                                                _arguments[selectedCommand].Arguments[findArgument.Name].IsSetByUser = true;
                                            }
                                            catch (Exception exc)
                                            {
                                                throw new ArgumentException("The value provided to argument " + item.Name + " is not acceptable. Reason: " + exc.GetBaseException().Message, item.Name, exc.InnerException);
                                            }
                                        }
                                        else
                                        {
                                            throw new MethodNotFoundException(parameterType.Name, "Cannot find a public constructor that accepts a string as parameter in class " + parameterType.Name);
                                        }
                                    }
                                    else
                                    {
                                        if (methodsWithCustomInputStringAttribute[0].GetParameters().Length == 1)
                                        {
                                            if (methodsWithCustomInputStringAttribute[0].GetParameters()[0].ParameterType == typeof(string))
                                            {
                                                try
                                                {
                                                    _arguments[selectedCommand].Arguments[findArgument.Name].Value = methodsWithCustomInputStringAttribute[0].Invoke(null, new[] { item.Value })!;
                                                    _parameterObject[selectedCommand].GetType().GetProperty(findArgument.OriginalPropertyName)!.SetValue(_parameterObject[selectedCommand], methodsWithCustomInputStringAttribute[0].Invoke(null, new[] { item.Value }));
                                                    _arguments[selectedCommand].Arguments[findArgument.Name].IsSetByUser = true;
                                                }
                                                catch (Exception exc)
                                                {
                                                    throw new ArgumentException("The value provided to argument " + item.Name + " is not acceptable. Reason: " + exc.GetBaseException().Message, item.Name, exc.InnerException);
                                                }
                                            }
                                            else
                                            {
                                                throw new TypeMismatchException(typeof(string).FullName!, methodsWithCustomInputStringAttribute[0].GetParameters()[0].ParameterType.FullName!, "", "Error in argument definition. It is expected a string, but the actual type is " + methodsWithCustomInputStringAttribute[0].GetParameters()[0].ParameterType.FullName);
                                            }
                                        }
                                        else
                                        {
                                            throw new TypeMismatchException(typeof(string).FullName!, "More than 1 parameter", "", "Error in argument definition. It is expected one parameter of type string, but the method has multiple parameters.");
                                        }
                                    }
                                }
                                else
                                {
                                    throw new DuplicateObjectException(methodsWithCustomInputStringAttribute[1].Name, new List<string> { "[CustomInputString] attribute" }, methodsWithCustomInputStringAttribute[0].Name, "Error in class " + parameterType.Name + ". It is expected that only one method may have the [CustomInputString] attribute, but there are " + methodsWithCustomInputStringAttribute.Count + " methods with this attribute");
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(_arguments[selectedCommand].GetCustomAttribute<CustomInputStringAttribute>(findArgument.Name).StringFormat))
                                {
                                    try
                                    {
                                        // Do not use the ParseExact method. Use the constructor with the parameter as value
                                        _arguments[selectedCommand].Arguments[findArgument.Name].Value = parameterType.GetConstructor(new[] { typeof(string) })!.Invoke(new[] { item.Value });
                                        _parameterObject[selectedCommand].GetType().GetProperty(findArgument.OriginalPropertyName)!.SetValue(_parameterObject[selectedCommand], parameterType.GetConstructor(new[] { typeof(string) })!.Invoke(new[] { item.Value }));
                                        _arguments[selectedCommand].Arguments[findArgument.Name].IsSetByUser = true;
                                    }
                                    catch (Exception exc)
                                    {
                                        throw new ArgumentException("The value provided to argument " + item.Name + " is not acceptable. Reason: " + exc.GetBaseException().Message, item.Name, exc.InnerException);
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        // Use ParseExact method
                                        _arguments[selectedCommand].Arguments[findArgument.Name].Value = parameterType.GetMethod("ParseExact", new[] {typeof(string), typeof(string), typeof(IFormatProvider)})!.Invoke(null, new[] { item.Value, _arguments[selectedCommand].GetCustomAttribute<CustomInputStringAttribute>(findArgument.Name).StringFormat, CultureInfo.InvariantCulture })!;
                                        _parameterObject[selectedCommand].GetType().GetProperty(findArgument.OriginalPropertyName)!.SetValue(_parameterObject[selectedCommand], parameterType.GetMethod("ParseExact", new[] { typeof(string), typeof(string), typeof(IFormatProvider) })!.Invoke(null, new[] { item.Value, _parameterObject[selectedCommand].GetType().GetProperty(findArgument.OriginalPropertyName)!.GetCustomAttribute<CustomInputStringAttribute>()!.StringFormat, CultureInfo.InvariantCulture }));
                                        _arguments[selectedCommand].Arguments[findArgument.Name].IsSetByUser = true;
                                    }
                                    catch (Exception exc)
                                    {
                                        throw new ArgumentException("The value provided to argument " + item.Name + " is not acceptable. Reason: " + exc.GetBaseException().Message, item.Name, exc.InnerException);
                                    }
                                }
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
                                if (parameterType != typeof(bool) && item.Value.GetType() != typeof(bool))
                                {
                                    _arguments[selectedCommand].Arguments[findArgument.Name].Value = Convert.ChangeType(item.Value, parameterType);
                                    _parameterObject[selectedCommand].GetType().GetProperty(findArgument.OriginalPropertyName)!.SetValue(_parameterObject[selectedCommand], Convert.ChangeType(item.Value, parameterType));
                                    _arguments[selectedCommand].Arguments[findArgument.Name].IsSetByUser = true;
                                }
                                else
                                {
                                    throw new ArgumentException();
                                }
                            }
                            catch (Exception exc)
                            {
                                throw new ArgumentException("The value provided to argument " + item.Name + " is not acceptable. Please provide a valid value", item.Name, exc);
                            }
                        }

                        if (_arguments[selectedCommand].GetCustomAttribute<ValueValidatorAttribute>(findArgument.Name) != null)
                        {
                            ValueValidatorAttribute validator = _arguments[selectedCommand].GetCustomAttribute<ValueValidatorAttribute>(findArgument.Name);

                            if (validator.ValueValidatorType.GetInterface(typeof(IValueValidator).Name) != null)
                            {
                                IValueValidator valueValidator = (IValueValidator) Activator.CreateInstance(validator.ValueValidatorType)!;
                                bool customValidationErrorMessageMissing = false;

                                try
                                {
                                    if (!valueValidator.ValidateValue(_arguments[selectedCommand].Arguments[findArgument.Name].Value))
                                    {
                                        customValidationErrorMessageMissing = true;
                                    }
                                }
                                catch(Exception exc) 
                                {
                                    throw new ArgumentException("The value provided to argument " + item.Name + " is not acceptable. Reason: " + exc.GetBaseException().Message, item.Name, exc.InnerException);
                                }

                                if (customValidationErrorMessageMissing)
                                {
                                    throw new ArgumentException("The value provided to argument " + item.Name + " is not valid according to the validation procedure");
                                }
                            }
                            else
                            {
                                throw new TypeMismatchException(typeof(IValueValidator).Name, "", "", validator.ValueValidatorType.Name + " doesn't have an interface of type " + typeof(IValueValidator).Name);
                            }
                        }
                    }
                }
            }
            return this;
        }

        /// <summary>
        /// Add the action to the list of available actions
        /// </summary>
        /// <typeparam name="CommandType">The command class</typeparam>
        /// <typeparam name="ParameterType">The type of the argument accepted by the command</typeparam>
        /// <param name="includePropertyNameInAliases"><see langword="true"/> if you want to add the property name as alias</param>
        /// <returns>This object</returns>
        /// <exception cref="MultipleCommandNotAllowedException">Exception raised when there are multiple ICommand in a command</exception>
        /// <exception cref="DuplicateObjectException">Exception raised when you want to add a command that is already implemented</exception>
        /// <exception cref="ArgumentException">Exception raised when an alias contains invalid characters</exception>
        public CommandManager AddCommand<CommandType, ParameterType>(bool includePropertyNameInAliases = false) where CommandType : ICommand<ParameterType>, new() where ParameterType : new()
        {
            CommandType command = new CommandType();
            ParameterType parameterObject = new ParameterType();
            int commandsImplementedNumber = 0;
            foreach (Type item in command.GetType().GetInterfaces())
            {
                if (item.GetInterface(typeof(ICommand).FullName!) != null)
                {
                    commandsImplementedNumber++;
                }
            }

            if (commandsImplementedNumber > 1)
            {
                throw new MultipleCommandNotAllowedException(command.GetType().FullName!, command.GetType().FullName + " has multiple ICommand class implemented. An action may have only one ICommand implementation");
            }

            string className = command.GetType().FullName;
            if (!_commands.ContainsKey(className))
            {
                _commands.Add(className, command);
                _parameterObject.Add(className, parameterObject);

                Argument argument = new Argument();

                if (command.GetType().GetCustomAttribute<CommandAttribute>() != null)
                {
                    argument.Action.AddRange(command.GetType().GetCustomAttribute<CommandAttribute>()!.Name.ToLower().Trim().Split(" "));
                }
                else
                {
                    argument.Action.AddRange(command.GetType().Name.ToLower().Trim().Split(" "));
                }

                KeyValuePair<string, Argument> findIfCommandAlreadyExists = (from item in _arguments
                                                                             where string.Join(" ", item.Value.Action) == string.Join(" ", argument.Action)
                                                                             select new KeyValuePair<string, Argument>(item.Key, item.Value)).FirstOrDefault();

                if (!findIfCommandAlreadyExists.Equals(Activator.CreateInstance(findIfCommandAlreadyExists.GetType())))
                {
                    throw new DuplicateObjectException(className, argument.Action, findIfCommandAlreadyExists.Key, "The commnd " + string.Join(" ", argument.Action) + " in " + className + " is already defined in " + findIfCommandAlreadyExists.Key);
                }

                foreach (PropertyInfo parameterProperty in parameterObject.GetType().GetProperties())
                {
                    if (parameterProperty.GetCustomAttribute<ExcludeItemFromCommandAttribute>() == null)
                    {
                        ParameterDescriptor descriptor = new ParameterDescriptor();
                        descriptor.OriginalPropertyName = parameterProperty.Name;

                        Type? parameterType = Nullable.GetUnderlyingType(parameterProperty.PropertyType);

                        if (parameterType == null)
                        {
                            descriptor.ParameterType = parameterProperty.PropertyType;
                        }
                        else
                        {
                            descriptor.ParameterType = parameterType;
                        }

                        List<object> attributes = new List<object>(parameterProperty.GetCustomAttributes());

                        if (parameterProperty.GetCustomAttributes<MandatoryForCommandAttribute>().Count() > 0)
                        {
                            foreach (MandatoryForCommandAttribute propertyAttribute in parameterProperty.GetCustomAttributes<MandatoryForCommandAttribute>())
                            {
                                if (propertyAttribute.Command == command.GetType())
                                {
                                    descriptor.IsMandatory = true;
                                    attributes.Remove(propertyAttribute);
                                }
                            }
                        }
                        else
                        {
                            if (parameterProperty.PropertyType.IsValueType)
                            {
                                if (Nullable.GetUnderlyingType(parameterProperty.PropertyType) == null)
                                {
                                    descriptor.IsMandatory = true;
                                }
                            }
                            else
                            {
                                if (parameterProperty.GetValue(parameterObject) != null)
                                {
                                    parameterProperty.SetValue(parameterObject, null);
                                    descriptor.IsMandatory = true;                                  
                                }
                            }
                        }

                        if (parameterProperty.GetCustomAttributes<AliasAttribute>().Count() > 0)
                        {
                            foreach (AliasAttribute propertyAttribute in parameterProperty.GetCustomAttributes<AliasAttribute>())
                            {
                                if (Regex.IsMatch(propertyAttribute.Name, @"^[A-Za-z0-9-]+$", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1)))
                                {
                                    if (!descriptor.Aliases.Contains(propertyAttribute.Name.ToLower().Trim()))
                                    {
                                        descriptor.Aliases.Add(propertyAttribute.Name.ToLower().Trim());
                                        attributes.Remove(propertyAttribute);
                                    }
                                }
                                else
                                {
                                    throw new ArgumentException("Invalid string found in " + parameterProperty.Name + ". Alias must contain only alphanumeric characters and hyphens (-). Null values or empty string are also not allowed");
                                }
                            }

                            descriptor.Name = parameterProperty.GetCustomAttributes<AliasAttribute>().First().Name.ToLower().Trim();
                        }
                        else
                        {
                            descriptor.Name = parameterProperty.Name.ToLower().Trim();
                        }

                        KeyValuePair<string, ParameterDescriptor> findIfAliasAlreadyExists = (from item in argument.Arguments
                                                                                              where item.Value.Aliases.Intersect(descriptor.Aliases).Any()
                                                                                              select new KeyValuePair<string, ParameterDescriptor>(item.Key, item.Value)).FirstOrDefault();

                        if (!findIfAliasAlreadyExists.Equals(Activator.CreateInstance(findIfAliasAlreadyExists.GetType())))
                        {
                            string parametersAlreadyDefined = string.Join(", ", findIfAliasAlreadyExists.Value.Aliases.Intersect(descriptor.Aliases));
                            throw new DuplicateObjectException(parameterProperty.Name, findIfAliasAlreadyExists.Value.Aliases.Intersect(descriptor.Aliases).ToList(), findIfAliasAlreadyExists.Value.OriginalPropertyName, "The property " + parameterProperty.Name + " has some alias that have already defined in " + findIfAliasAlreadyExists.Value.OriginalPropertyName + ". Duplicate aliases: " + parametersAlreadyDefined);
                        }

                        if (parameterProperty.GetCustomAttribute<DescriptionAttribute>() != null)
                        {
                            descriptor.Description = parameterProperty.GetCustomAttribute<DescriptionAttribute>()!.Description;
                            attributes.Remove(parameterProperty.GetCustomAttribute<DescriptionAttribute>()!);
                        }

                        descriptor.Value = parameterProperty.GetValue(parameterObject)!;
                        descriptor.Attributes = attributes;

                        if (includePropertyNameInAliases)
                        {
                            if (descriptor.Aliases.IndexOf(parameterProperty.Name.ToLower().Trim()) < 0)
                            {
                                descriptor.Aliases.Add(parameterProperty.Name.ToLower().Trim());
                            }
                        }

                        if (parameterProperty.GetCustomAttribute<ExampleValueAttribute>() != null 
                            && string.IsNullOrEmpty(parameterProperty.GetCustomAttribute<ExampleValueAttribute>()!.ExampleValue))
                        {
                            throw new ArgumentException("The example value cannot be null or empty", parameterProperty.Name);
                        }

                        argument.Arguments.Add(descriptor.Name, descriptor);
                    }
                }
                _arguments.Add(className, argument);
            }
            return this;
        }

        internal bool ExecuteCommand(List<string> action, object parameters)
        {
            bool commandExecuted = false;
            if (action.Count > 0)
            {
                string? selectedCommand = (from item in _arguments
                                           where string.Join(" ", item.Value.Action) == string.Join(" ", action)
                                           select item.Key).FirstOrDefault();

                if (selectedCommand != default)
                {
                    Type genericType = _commands[selectedCommand].GetType().GetInterfaces()[0].GetGenericArguments()[0];
                    object commandConstructor = _commands[selectedCommand].GetType().GetConstructor(Type.EmptyTypes)!.Invoke(Array.Empty<object>());
                    _result = (string)typeof(ICommand<>).MakeGenericType(genericType).GetMethod("Main")!.Invoke(commandConstructor, new object[] { parameters })!;
                    commandExecuted = true;
                }
            }
            return commandExecuted;
        }
    }
}
