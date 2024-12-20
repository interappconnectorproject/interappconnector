﻿using InterAppConnector.Attributes;
using InterAppConnector.DataModels;
using InterAppConnector.Exceptions;
using InterAppConnector.Interfaces;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Data;

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
        
        private static ICommand? _currentCommand;

        private readonly List<Type> definedRules = new List<Type>();

        internal static ICommand? CurrentCommand 
        {
            get
            {
                return _currentCommand;
            }
        }

        public CommandManager()
        {
            definedRules = RuleManager.GetAllRules(new List<Assembly> { typeof(CommandManager).Assembly });
        }

        internal string? GetActionKeyByActions(List<string> actions)
        {
            string? commandName = (from item in _arguments
                                   where item.Value.Action.Intersect(actions).Any()
                                   select item.Key).SingleOrDefault();
            return commandName;
        }

        internal static void SetCommand(ICommand command)
        {
            _currentCommand = command;
        }

        internal CommandManager SetArguments(List<string> actions, List<ParameterDescriptor> argumentsSetByTheUser)
        {
            string? selectedCommand = GetActionKeyByActions(actions);
            if (selectedCommand != default)
            {
                SetCommand(_commands[selectedCommand]);
                object currentParameterObject = _parameterObject[selectedCommand];
                List<IArgumentSettingRule> rulesNotExecuted = new List<IArgumentSettingRule>();
                List<IArgumentSettingRule> rulesToExecute = RuleManager.GetAssemblyRules<IArgumentSettingRule>(typeof(CommandManager).Assembly);
                rulesToExecute = RuleManager.MergeRules(rulesToExecute, RuleManager.GetAssemblyRules<IArgumentSettingRule>(definedRules));
                rulesNotExecuted.AddRange(rulesToExecute);
                foreach (ParameterDescriptor item in argumentsSetByTheUser)
                {
                    ParameterDescriptor? findArgument = (from value in _arguments[selectedCommand].Arguments.Values
                                                         where value.Name.ToLower().Trim() == item.Name.ToLower().Trim()
                                                         || value.Aliases.Any(s => s.ToLower().Trim() == item.Name.ToLower().Trim())
                                                         select value).SingleOrDefault();

                    if (findArgument != default(ParameterDescriptor))
                    {
                        foreach (var rule in from IArgumentSettingRule rule in rulesToExecute
                                             where rule.IsRuleEnabledInArgumentSetting(currentParameterObject.GetType().GetProperty(findArgument.OriginalPropertyName)!)
                                             select rule)
                        {
                            findArgument = ExecuteSettingRules(rule, currentParameterObject, findArgument, item, rulesNotExecuted);
                        }
                    }
                }

                /*
                 * After executing the rules for the parameters set by the user, it is necessary 
                 * to execute the rules for the remaining arguments defined in the command 
                 * Be aware that there is a special rule (the generic one) that should be also executed
                 */
                ExecuteSettingRules(currentParameterObject, rulesNotExecuted);

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

            string className = command.GetType().FullName!;
            if (!_commands.ContainsKey(className))
            {
                _commands.Add(className, command);
                _parameterObject.Add(className, parameterObject);
                SetCommand(command);
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

                List<IArgumentDefinitionRule> rulesNotExecuted = new List<IArgumentDefinitionRule>();
                List<IArgumentDefinitionRule> rulesToExecute = RuleManager.GetAssemblyRules<IArgumentDefinitionRule>(typeof(CommandManager).Assembly);
                rulesToExecute = RuleManager.MergeRules(rulesToExecute, RuleManager.GetAssemblyRules<IArgumentDefinitionRule>(definedRules));
                rulesNotExecuted.AddRange(rulesToExecute);

                foreach (PropertyInfo parameterProperty in parameterObject.GetType().GetProperties())
                {
                    List<Attribute> distinctAttributes = RuleManager.GetDistinctAttributes(parameterProperty);
                    ParameterDescriptor descriptor = new ParameterDescriptor();


                    foreach (var rule in from IArgumentDefinitionRule rule in rulesToExecute
                                         where rule.IsRuleEnabledInArgumentDefinition(parameterProperty)
                                         select rule)
                    {
                        if (RuleManager.IsAttributeSpecializedRule(rule))
                        {
                            Type argumentType = rule.GetType().GetInterface(typeof(IArgumentDefinitionRule<>).FullName!)!.GetGenericArguments()[0];

                            Attribute? attributeExists = (from item in distinctAttributes
                                                          where item.GetType() == argumentType
                                                          select item).FirstOrDefault();

                            if (attributeExists != default(Attribute))
                            {
                                descriptor = rule.DefineArgumentIfTypeExists(parameterObject, parameterProperty, descriptor);
                            }
                            else
                            {
                                descriptor = rule.DefineArgumentIfTypeDoesNotExist(parameterObject, parameterProperty, descriptor);
                            }
                            rulesNotExecuted.Remove(rule);
                        }
                        else if (RuleManager.IsObjectSpecializedRule(rule))
                        {
                            if (rule.GetType().GetInterface(typeof(IArgumentDefinitionRule<>).FullName!)!.GetGenericArguments()[0] == parameterProperty.PropertyType)
                            {
                                descriptor = rule.DefineArgumentIfTypeExists(parameterObject, parameterProperty, descriptor);
                                rulesNotExecuted.Remove(rule);
                            }                            
                        }
                        else
                        {
                            descriptor = rule.DefineArgumentIfTypeExists(parameterObject, parameterProperty, descriptor);
                            rulesNotExecuted.Remove(rule);
                        }
                    }

                    KeyValuePair<string, ParameterDescriptor> findIfAliasAlreadyExists = (from item in argument.Arguments
                                                                                            where item.Value.Aliases.Intersect(descriptor.Aliases).Any()
                                                                                            select new KeyValuePair<string, ParameterDescriptor>(item.Key, item.Value)).FirstOrDefault();

                    if (!findIfAliasAlreadyExists.Equals(Activator.CreateInstance(findIfAliasAlreadyExists.GetType())))
                    {
                        string parametersAlreadyDefined = string.Join(", ", findIfAliasAlreadyExists.Value.Aliases.Intersect(descriptor.Aliases));
                        throw new DuplicateObjectException(parameterProperty.Name, findIfAliasAlreadyExists.Value.Aliases.Intersect(descriptor.Aliases).ToList(), findIfAliasAlreadyExists.Value.OriginalPropertyName, "The property " + parameterProperty.Name + " has some alias that have already defined in " + findIfAliasAlreadyExists.Value.OriginalPropertyName + ". Duplicate aliases: " + parametersAlreadyDefined);
                    }

                    if (includePropertyNameInAliases)
                    {
                        if (descriptor.Aliases.IndexOf(parameterProperty.Name.ToLower().Trim()) < 0)
                        {
                            descriptor.Aliases.Add(parameterProperty.Name.ToLower().Trim());
                        }
                    }

                    if (!string.IsNullOrEmpty(descriptor.Name))
                    {
                        argument.Arguments.Add(descriptor.Name, descriptor);
                    }
                }

                foreach (IArgumentDefinitionRule rule in (from item in rulesNotExecuted
                                                          where item.IsRuleEnabledInArgumentDefinition(((PropertyInfo) null!))
                                                          select item).ToList())
                {
                    rule.DefineArgumentIfTypeDoesNotExist(parameterObject, (PropertyInfo) null!, null!);
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

        private static ParameterDescriptor ExecuteSettingRules(IArgumentSettingRule rule, object currentParameterObject, ParameterDescriptor targetArgument, ParameterDescriptor argumentSetByTheUser, List<IArgumentSettingRule> rulesNotExecuted)
        {
            /*
             * A default rule does not have an interface with an argument type,
             * so you have to consider also this case
             */
            if (RuleManager.IsAttributeSpecializedRule(rule))
            {
                Type argumentType = rule.GetType().GetInterface(typeof(IArgumentSettingRule<>).FullName!)!.GetGenericArguments()[0];

                Attribute? attributeExists = (from distinctAttribute in RuleManager.GetDistinctAttributes(targetArgument.Attributes)
                                              where distinctAttribute.GetType() == argumentType
                                              select distinctAttribute).FirstOrDefault();

                if (attributeExists != default(Attribute))
                {
                    targetArgument = rule.SetArgumentValueIfTypeExists(currentParameterObject, currentParameterObject.GetType().GetProperty(targetArgument.OriginalPropertyName)!, targetArgument, argumentSetByTheUser);
                }
                else
                {
                    targetArgument = rule.SetArgumentValueIfTypeDoesNotExist(currentParameterObject, currentParameterObject.GetType().GetProperty(targetArgument.OriginalPropertyName)!, targetArgument, argumentSetByTheUser);
                }
                rulesNotExecuted.Remove(rule);
            }
            else if (RuleManager.IsObjectSpecializedRule(rule))
            {
                if (rule.GetType().GetInterface(typeof(IArgumentSettingRule<>).FullName!)!.GetGenericArguments()[0] == currentParameterObject.GetType().GetProperty(targetArgument.OriginalPropertyName)!.PropertyType)
                {
                    targetArgument = rule.SetArgumentValueIfTypeExists(currentParameterObject, currentParameterObject.GetType().GetProperty(targetArgument.OriginalPropertyName)!, targetArgument, argumentSetByTheUser);
                    rulesNotExecuted.Remove(rule);
                }
            }
            else
            {
                targetArgument = rule.SetArgumentValueIfTypeExists(currentParameterObject, currentParameterObject.GetType().GetProperty(targetArgument.OriginalPropertyName)!, targetArgument, argumentSetByTheUser);
                rulesNotExecuted.Remove(rule);
            }
            return targetArgument;
        }

        private void ExecuteSettingRules(object currentParameterObject, List<IArgumentSettingRule> rulesNotExecuted)
        {
            foreach (IArgumentSettingRule rule in (from rule in rulesNotExecuted
                                                   where !RuleManager.IsSpecializedRule(rule)
                                                   && rule.IsRuleEnabledInArgumentSetting((PropertyInfo) null!)
                                                   select rule).ToList())
            {
                rule.SetArgumentValueIfTypeDoesNotExist(currentParameterObject, (PropertyInfo)null!, null!, null!);
            }

            foreach (KeyValuePair<PropertyInfo, IArgumentSettingRule> rule in (from rule in rulesNotExecuted
                                                                               from argument in currentParameterObject.GetType().GetProperties()
                                                                               where RuleManager.IsObjectSpecializedRule(rule)
                                                                               && rule.GetType().GetInterface(typeof(IArgumentSettingRule<>).FullName!)!.GetGenericArguments()[0] == argument.PropertyType
                                                                               && rule.IsRuleEnabledInArgumentSetting(argument)
                                                                               select new { argument, rule }).ToDictionary(item => item.argument, item => item.rule))
            {
                ParameterDescriptor currentParameter = (from parameters in _arguments.Values
                                                        from parameter in parameters.Arguments.Values
                                                        where parameter.OriginalPropertyName == currentParameterObject.GetType().GetProperty(rule.Key.Name)!.Name
                                                        select parameter).First();

                rule.Value.SetArgumentValueIfTypeDoesNotExist(currentParameterObject, currentParameterObject.GetType().GetProperty(rule.Key.Name)!, currentParameter, null!);
            }
        }
    }
}
