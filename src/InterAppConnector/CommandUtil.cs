using InterAppConnector.Attributes;
using InterAppConnector.DataModels;
using InterAppConnector.Enumerations;
using InterAppConnector.Interfaces;
using System.Collections;
using System.Reflection;
using System.Text;

namespace InterAppConnector
{
    /// <summary>
    /// Contain methods useful for commands
    /// </summary>
    public class CommandUtil
    {
        /// <summary>
        /// The maximum object depth. The default value is 64, but you can define any positive number.
        /// Don't set a huge number to this property, as it could raise a <see cref="StackOverflowException"/> exception
        /// </summary>
        public static uint MaximumObjectDepth { get; set; } = 64;

        internal static string DescribeCommands(CommandManager commandManager, bool writeHeader = true, bool simulateConsoleException = false)
        {
            StringBuilder description = new StringBuilder();
            int lineLength;
            int divisor = 1;

            if (simulateConsoleException)
            {
                divisor = 0;
            }

            try
            {
                lineLength = Console.WindowWidth / divisor;
            }
            catch 
            {
                /*
                 * This line is useful for VS Test Explorer in order to pass the test without errors.
                 * But there is another problem: how to test this branch in order to have the code covered by the tests?
                 * A possibile solution is to "simulate" an error, but it requires another parameter in the method
                 * that it is useless in real applications. Is there a cleaner way to test this code without the additional
                 * parameter?
                 */
                lineLength = 5;
            }

            if (writeHeader)
            {
                description.AppendLine(DrawLine(lineLength));
                description.AppendLine();
                description.Append(Assembly.GetEntryAssembly().GetName().Name);
                description.Append(" ");
                description.AppendLine(Assembly.GetEntryAssembly().GetName().Version.ToString());
                description.AppendLine();
                description.AppendLine(DrawLine(lineLength));
            }
            description.AppendLine();
            description.AppendLine("Available actions:");
            description.AppendLine();

            foreach (KeyValuePair<string, Argument> item in commandManager._arguments)
            {
                string action;
                string actionDescription = "No description provided";

                if (commandManager._commands[item.Key].GetType().GetCustomAttribute<CommandAttribute>() != null)
                {
                    action = commandManager._commands[item.Key].GetType().GetCustomAttribute<CommandAttribute>().Name;
                    actionDescription = commandManager._commands[item.Key].GetType().GetCustomAttribute<CommandAttribute>().Description;
                }
                else
                {
                    action = commandManager._commands[item.Key].GetType().Name.ToLower().Trim();
                }

                description.AppendLine(DescribeAction(action, actionDescription, item.Value.Arguments.Values.ToList()));
            }

            return description.ToString();
        }

        private static string DrawLine(int lenght)
        {
            string line = "";

            for (int i = 0; i < lenght - 1; i++)
            {
                line += "-";
            }

            return line;
        }

        internal static string DescribeAction(string action, string actionDescription, List<ParameterDescriptor> arguments)
        {
            StringBuilder exampleParameters = new StringBuilder();
            StringBuilder description = new StringBuilder();
            description.AppendLine("- " + action);
            description.AppendLine("  " + actionDescription);
            exampleParameters.Append(" " + action);
            foreach (ParameterDescriptor descriptor in arguments)
            {
                description.Append("\t-" + descriptor.Name);
                if (descriptor.IsMandatory)
                {
                    description.Append(" (required) : ");
                    exampleParameters.Append(" -" + descriptor.Name + " ");
                }
                else
                {
                    description.Append(" (optional) : ");
                }

                if (!string.IsNullOrEmpty(descriptor.Description))
                {
                    description.AppendLine(descriptor.Description);
                }
                else
                {
                    description.AppendLine("No description provided");
                }
                

                // for enumerations, describe the numbers and values that a parameter may have
                if (descriptor.ParameterType.IsEnum)
                {
                    description.AppendLine("\tAccepted values for this parameter: ");
                    EnumHelper helper = new EnumHelper();
                    helper.LoadEnumerationValues(descriptor.ParameterType);
                    string exampleEnumValue = "";
                    foreach (ParameterDescriptor possibleValue in helper._parameters.Values)
                    {
                        description.Append("\t\t" + possibleValue.Name + " ");
                        description.Append("(");
                        description.Append(possibleValue.Value);
                        description.Append(") : ");
                        if (string.IsNullOrEmpty(possibleValue.Description))
                        {
                            description.Append("No description provided");
                        }
                        else
                        {
                            description.Append(possibleValue.Description);
                        }

                        description.AppendLine();

                        if (possibleValue.Aliases.Count > 1)
                        {
                            for (int i = 1; i < possibleValue.Aliases.Count; i++)
                            {
                                description.AppendLine("\t\t" + possibleValue.Aliases[i] + " : Same as " + possibleValue.Name);
                            }
                        }

                        if (string.IsNullOrEmpty(exampleEnumValue))
                        {
                            exampleEnumValue = possibleValue.Name;
                        }
                    }

                    if (descriptor.IsMandatory)
                    {
                        exampleParameters.Append("\"" + exampleEnumValue + "\"");
                    }
                }
                else
                {
                    /**
                     * For other types, we have three cases:
                     * - no validators are defined
                     * - no examples are defined
                     * - it is defined a validator and an example
                     * A validator has a precedence to everything. If a validator is not defined, the library checks
                     * if there is an example defined in [ExampleValue] attribute. If there is not this attribute
                     * write a generic string. Remember that booleans haven't any values
                     */
                    if (descriptor.IsMandatory && descriptor.ParameterType != typeof(bool))
                    {
                        if (descriptor.Attributes.Exists(item => item.GetType() == typeof(ValueValidatorAttribute)))
                        {
                            ValueValidatorAttribute attribute = (ValueValidatorAttribute) descriptor.Attributes.Find(item => item.GetType() == typeof(ValueValidatorAttribute))!;
                            IValueValidator? validator = Activator.CreateInstance(attribute.ValueValidatorType) as IValueValidator;
                            if (validator != null)
                            {
                                exampleParameters.Append('\"');
                                exampleParameters.Append(validator.GetSampleValidValue());
                                exampleParameters.Append('\"');
                            }
                            else
                            {
                                exampleParameters.Append("\"##INVALIDVALIDATOR##\"");
                            }
                        }
                        else if (descriptor.Attributes.Exists(item => item.GetType() == typeof(ExampleValueAttribute)))
                        {
                            ExampleValueAttribute attribute = (ExampleValueAttribute)descriptor.Attributes.Find(item => item.GetType() == typeof(ExampleValueAttribute))!;
                            exampleParameters.Append("\"" + attribute.ExampleValue + "\"");
                        }
                        else
                        {
                            exampleParameters.Append("\"<value>\"");
                        }
                    }
                }

                /*
                 * Be aware that the first alias is taken as the argument name, so it is not necessary
                 * to insert it in the list of the list of the other aliases for the same argument as
                 * it is already inserted as the main argument
                 */
                if (descriptor.Aliases.Count > 1)
                {
                    for (int i = 1; i < descriptor.Aliases.Count; i++)
                    {
                        description.AppendLine("\t-" + descriptor.Aliases[i] + " : Same as " + descriptor.Name);
                    }
                }

            }

            if (InterAppCommunication.CommandExecutionType == CommandExecutionType.Interactive)
            {
                description.AppendLine();
                description.AppendLine("  Command example:");
                description.AppendLine("  " + Path.GetFileName(Environment.ProcessPath) +  exampleParameters.ToString());
            }

            return description.ToString();
        }

        /// <summary>
        /// Convert the object <paramref name="objectToWrite"/> to his string representation
        /// </summary>
        /// <param name="objectToWrite">The object to stringify</param>
        /// <param name="numberOfSpaces"> The number of spaces to leave when writing the object</param>
        /// <param name="depth">The current depth</param>
        /// <returns>String representation of the object <paramref name="objectToWrite"/></returns>
        public static string WriteObject(object objectToWrite, int numberOfSpaces = 0, int depth = 0)
        {
            StringBuilder text = new StringBuilder();
            if (depth < MaximumObjectDepth)
            {
                string prefix = "";
                int spaces = numberOfSpaces;

                for (int i = 0; i < numberOfSpaces; i++)
                {
                    prefix += " ";
                }

                if (objectToWrite != null)
                {
                    if (objectToWrite.GetType().IsPrimitive || objectToWrite is string || objectToWrite is DateTime)
                    {
                        text.Append( objectToWrite.ToString());
                    }
                    else
                    {
                        if (objectToWrite is ICollection)
                        {
                            ICollection collection = (ICollection)objectToWrite;
                            if (collection.Count > 0)
                            {
                                text.AppendLine(collection.Count + " elements in the list ");

                                foreach (object value in collection)
                                {
                                    text.AppendLine(prefix + " " + WriteObject(value, spaces + 2, depth + 1) + Environment.NewLine);
                                }
                            }
                            else
                            {
                                text.AppendLine("No elements");
                            }
                        }
                        else if (objectToWrite is Enum)
                        {
                            text.Append((int)objectToWrite);
                        }
                        else
                        {
                            // text += prefix + "This object contains " + objectToWrite.GetType().GetProperties().Length + " properties" + Environment.NewLine;
                            text.AppendLine();
                            foreach (PropertyInfo item in objectToWrite.GetType().GetProperties())
                            {
                                string textObject = WriteObject(item.GetValue(objectToWrite), spaces + 2, depth + 1);

                                /*if (!string.IsNullOrEmpty(prefix) && textObject.StartsWith(prefix))
                                {
                                    textObject = textObject.Substring(prefix.Length + 2);
                                }*/

                                text.AppendLine(prefix + "  " + item.Name + " : " + textObject);
                            }
                        }
                    }
                }
                else
                {
                    text.AppendLine("Not set");
                }

                if (text.ToString().EndsWith(Environment.NewLine + Environment.NewLine))
                {
                    return text.ToString(0, text.Length - Environment.NewLine.Length * 2);
                }
            }

            return text.ToString();
        }
    }
}
