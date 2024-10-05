using System.Reflection;
using System.Text.Json;
using InterAppConnector.Attributes;
using InterAppConnector.DataModels;
using InterAppConnector.Enumerations;
using InterAppConnector.Exceptions;
using InterAppConnector.Interfaces;
using static InterAppConnector.CommandOutput;

namespace InterAppConnector
{
    /// <summary>
    /// Manage inter-application communication between different assemblies or modules
    /// </summary>
    public class InterAppCommunication
    {
        /// <summary>
        /// Event raised when an error message is emitted
        /// </summary>
        public event StatusEventHandler ErrorMessageEmitted;

        /// <summary>
        /// Event raised when a warning message is emitted
        /// </summary>
        public event StatusEventHandler WarningMessageEmitted;

        /// <summary>
        /// Event raised when an info message is emitted
        /// </summary>
        public event StatusEventHandler InfoMessageEmitted;

        /// <summary>
        /// Event raised when a success message is emitted
        /// </summary>
        public event StatusEventHandler SuccessMessageEmitted;

        private static CommandExecutionType _commandExecutionType = CommandExecutionType.Interactive;

        /// <summary>
        /// Define in which mode the command is executed
        /// </summary>
        public static CommandExecutionType CommandExecutionType {
            get
            {
                return _commandExecutionType;
            }
        }

        /// <summary>
        /// Define the output format of the commands
        /// </summary>
        public static CommandOutputFormat CommandOutputFormat { get; set; } = CommandOutputFormat.Text;

        private CommandManager _commandManager;

        public InterAppCommunication(CommandManager commandManager)
        {
            _commandManager = commandManager;
        }

        /// <summary>
        /// Built-in method to create a command line interface user interface for interactive applications
        /// </summary>
        /// <param name="arguments">Arguments passed to the CLI</param>
        public void ExecuteAsInteractiveCLI(string[] arguments, CommandOutputFormat commandOutputFormat = CommandOutputFormat.Text)
        {
            CommandExecutionType currentCommandExecutionType = _commandExecutionType;
            CommandOutputFormat currentCommandOutputFormat = CommandOutputFormat;

            _commandExecutionType = CommandExecutionType.Interactive;
            CommandOutputFormat = commandOutputFormat;

            if (!IsErrorEventAlreadyAttached(CommandOutput_ErrorMessageEmitted))
            {
                CommandOutput.ErrorMessageEmitted += CommandOutput_ErrorMessageEmitted;
            }
            
            if (!IsSuccessEventAlreadyAttached(CommandOutput_SuccessMessageEmitted))
            {
                CommandOutput.SuccessMessageEmitted += CommandOutput_SuccessMessageEmitted;
            }
            
            if (!IsInfoEventAlreadyAttached(CommandOutput_InfoMessageEmitted))
            {
                CommandOutput.InfoMessageEmitted += CommandOutput_InfoMessageEmitted;
            }
            
            if (!IsWarningEventAlreadyAttached(CommandOutput_WarningMessageEmitted))
            {
                CommandOutput.WarningMessageEmitted += CommandOutput_WarningMessageEmitted;
            }
            
            string selectedAction = "";
            Argument argumentsParsed = Argument.Parse(arguments, "-");

            List<string> missingParameters = new List<string>();

            if (argumentsParsed.Action.Count > 0)
            {
                try
                {
                    selectedAction = _commandManager.SetArguments(argumentsParsed.Action, argumentsParsed.Arguments.Values.ToList())
                        .GetActionKeyByActions(argumentsParsed.Action);

                    if (!string.IsNullOrEmpty(selectedAction))
                    {
                        foreach (ParameterDescriptor item in _commandManager._arguments[selectedAction].Arguments.Values)
                        {
                            if (item.IsMandatory && !item.IsSetByUser)
                            {
                                missingParameters.Add(item.Name);
                            }
                        }

                        if (missingParameters.Count == 0)
                        {
                            _commandManager.ExecuteCommand(argumentsParsed.Action, _commandManager._parameterObject[selectedAction]);
                        }
                        else
                        {
                            Error("Cannot execute the selected action because some mandatory parameters are missing. Missing parameters: " + string.Join(", ", missingParameters) + Environment.NewLine + CommandUtil.DescribeAction(string.Join(" ", argumentsParsed.Action), _commandManager._commands[selectedAction].GetType().GetCustomAttribute<CommandAttribute>().Description, _commandManager._arguments[selectedAction].Arguments.Values.ToList()));
                        }
                    }
                    else
                    {
                        Error("Cannot execute the selected action because this action does not exist." + Environment.NewLine + CommandUtil.DescribeCommands(_commandManager, false));
                    }
                }
                catch (ArgumentException argumentException)
                {
                    Error(argumentException.Message);
                }
            }
            else
            {
                Console.Write(CommandUtil.DescribeCommands(_commandManager));
            }

            _commandExecutionType = currentCommandExecutionType;
            CommandOutputFormat = currentCommandOutputFormat;
        }

        /// <summary>
        /// Execute the command without user interaction. Use this method if you want to parse the
        /// result of the commnd by your own.
        /// Use the <see cref="Parse{T}(string)"/> method to parse the result of the returned string.
        /// If you need a method that manage the parsing operations for you, use the <seealso cref="ExecuteAsBatch{ReturnedType}(string, dynamic)"/>
        /// method
        /// </summary>
        /// <param name="action">The actions that defined which action is called</param>
        /// <param name="parameters">The parameters to pass to the command</param>
        /// <returns>The raw string returned by the requested command</returns>
        public string ExecuteAsBatch(string action, dynamic parameters, CommandOutputFormat commandOutputFormat = CommandOutputFormat.Json)
        {
            CommandExecutionType currentCommandExecutionType = _commandExecutionType;
            CommandOutputFormat currentCommandOutputFormat = CommandOutputFormat;

            string output = "";
            _commandExecutionType = CommandExecutionType.Batch;
            CommandOutputFormat = commandOutputFormat;

            if (!IsErrorEventAlreadyAttached(CommandOutput_ErrorMessageEmitted))
            {
                CommandOutput.ErrorMessageEmitted += CommandOutput_ErrorMessageEmitted;
            }

            if (!IsSuccessEventAlreadyAttached(CommandOutput_SuccessMessageEmitted))
            {
                CommandOutput.SuccessMessageEmitted += CommandOutput_SuccessMessageEmitted;
            }

            if (!IsInfoEventAlreadyAttached(CommandOutput_InfoMessageEmitted))
            {
                CommandOutput.InfoMessageEmitted += CommandOutput_InfoMessageEmitted;
            }

            if (!IsWarningEventAlreadyAttached(CommandOutput_WarningMessageEmitted))
            {
                CommandOutput.WarningMessageEmitted += CommandOutput_WarningMessageEmitted;
            }

            Argument argumentsParsed = Argument.Parse(parameters, "-");
            argumentsParsed.Action.AddRange(action.Split(" ", StringSplitOptions.RemoveEmptyEntries));
            List<string> missingParameters = new List<string>();

            if (argumentsParsed.Action.Count > 0)
            {
                string selectedAction = _commandManager.SetArguments(argumentsParsed.Action, argumentsParsed.Arguments.Values.ToList())
                    .GetActionKeyByActions(argumentsParsed.Action);

                if (!string.IsNullOrEmpty(selectedAction))
                {
                    foreach (ParameterDescriptor item in _commandManager._arguments[selectedAction].Arguments.Values)
                    {
                        if (item.IsMandatory && !item.IsSetByUser)
                        {
                            missingParameters.Add(item.Name);
                        }
                    }

                    if (missingParameters.Count == 0)
                    {
                        _commandManager.ExecuteCommand(argumentsParsed.Action, _commandManager._parameterObject[selectedAction]);
                        output = _commandManager._result;
                    }
                    else
                    {
                        throw new MissingMandatoryArgumentException(missingParameters, "Cannot execute the selected action because some mandatory parameters are missing. Missing parameters: " + string.Join(", ", missingParameters) + Environment.NewLine + CommandUtil.DescribeAction(string.Join(" ", argumentsParsed.Action), _commandManager._commands[selectedAction].GetType().GetCustomAttribute<CommandAttribute>().Description, _commandManager._arguments[selectedAction].Arguments.Values.ToList()));
                    }
                }
                else
                {
                    output = Error("Cannot execute the selected action because this action does not exist." + Environment.NewLine + CommandUtil.DescribeCommands(_commandManager, false));
                }
            }

            _commandExecutionType = currentCommandExecutionType;
            CommandOutputFormat = currentCommandOutputFormat;

            return output;
        }

        /// <summary>
        /// Returns the <see cref="CommandResult{ResultType}"/> object already ready to be used in your application
        /// without parsing it. This method handles the necessary parsing operations for you.
        /// If you need the raw string and you want to use your own parsing method, use the <seealso cref="ExecuteAsBatch(string, dynamic)"/> method
        /// </summary>
        /// <typeparam name="ReturnedType"></typeparam>
        /// <param name="action">The actions that defined which action is called</param>
        /// <param name="parameters">The parameters to pass to the command</param>
        /// <returns> The result of the command in an object</returns>
        public CommandResult<ReturnedType> ExecuteAsBatch<ReturnedType>(string action, dynamic parameters)
        {
            string returnedString = ExecuteAsBatch(action, parameters);

            return Parse<ReturnedType>(returnedString);
        }

        /// <summary>
        /// Call a single command. This method is useful when you want to integrate a command in another command.<br />
        /// This method does not execute the command. In order to execute it, you have to use one of the execute methods available in <see cref="InterAppCommunication"/> class
        /// (for instance <see cref="ExecuteAsBatch(string, dynamic)"/>, <see cref="ExecuteAsBatch{ReturnedType}(string, dynamic)"/> or <see cref="ExecuteAsInteractiveCLI(string[])"/>)
        /// </summary>
        /// <typeparam name="CommandType">The command type</typeparam>
        /// <typeparam name="ArgumentType">The argument type to be passed to the command</typeparam>
        /// <returns>an <see cref="InterAppCommunication"/> object </returns>
        public static InterAppCommunication CallSingleCommand<CommandType, ArgumentType>() where CommandType : ICommand<ArgumentType>, new() where ArgumentType : new()
        {
            CommandManager command = new CommandManager();
            command.AddCommand<CommandType, ArgumentType>();

            InterAppCommunication connector = new InterAppCommunication(command);
            return connector;
        }

        private void CommandOutput_WarningMessageEmitted(CommandExecutionMessageType messageStatus, int exitCode, object message)
        {
            if (WarningMessageEmitted != null)
            {
                WarningMessageEmitted(messageStatus, exitCode, message);
            }
            else
            {
                if (CommandExecutionType == CommandExecutionType.Interactive)
                {
                    if (CommandOutputFormat == CommandOutputFormat.Text)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("[" + DateTime.Now + "]" + " WARNING (" + exitCode + ") : " + CommandUtil.WriteObject(ExtractMessageObject(message)));
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.WriteLine(JsonSerializer.Serialize(message));
                    }
                }
            }
        }

        private void CommandOutput_InfoMessageEmitted(CommandExecutionMessageType messageStatus, int exitCode, object message)
        {
            if (InfoMessageEmitted != null)
            {
                InfoMessageEmitted(messageStatus, exitCode, message);
            }
            else
            {
                if (CommandExecutionType == CommandExecutionType.Interactive)
                {
                    if (CommandOutputFormat == CommandOutputFormat.Text)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("[" + DateTime.Now + "]" + " INFO (" + exitCode + ") : " + CommandUtil.WriteObject(ExtractMessageObject(message)));
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.WriteLine(JsonSerializer.Serialize(message));
                    }
                }
            }
        }

        private void CommandOutput_SuccessMessageEmitted(CommandExecutionMessageType messageStatus, int exitCode, object message)
        {
            if (SuccessMessageEmitted != null)
            {
                SuccessMessageEmitted(messageStatus, exitCode, message);
            }
            else
            {
                if (CommandExecutionType == CommandExecutionType.Interactive)
                {
                    if (CommandOutputFormat == CommandOutputFormat.Text)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[" + DateTime.Now + "]" + " SUCCESS (" + exitCode + ") : " + CommandUtil.WriteObject(ExtractMessageObject(message)));
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.WriteLine(JsonSerializer.Serialize(message));
                    }
                }
            }
        }

        private void CommandOutput_ErrorMessageEmitted(CommandExecutionMessageType messageStatus, int exitCode, object message)
        {
            if (ErrorMessageEmitted != null)
            {
                ErrorMessageEmitted(messageStatus, exitCode, message);
            }
            else
            {
                if (CommandExecutionType == CommandExecutionType.Interactive)
                {
                    if (CommandOutputFormat == CommandOutputFormat.Text)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[" + DateTime.Now + "]" + " ERROR (" + exitCode + ") : " + CommandUtil.WriteObject(ExtractMessageObject(message)));
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.WriteLine(JsonSerializer.Serialize(message));
                    }
                }
            }
        }
    }
}
