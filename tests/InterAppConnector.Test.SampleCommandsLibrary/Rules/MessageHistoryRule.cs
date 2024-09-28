using InterAppConnector.DataModels;
using InterAppConnector.Enumerations;
using InterAppConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InterAppConnector.Test.SampleCommandsLibrary.Rules
{
    public class MessageHistoryRule : IArgumentDefinitionRule
    {
        public ParameterDescriptor DefineArgumentIfTypeDoesNotExist(object parentObject, PropertyInfo property, ParameterDescriptor descriptor)
        {
            return descriptor;
        }

        public ParameterDescriptor DefineArgumentIfTypeDoesNotExist(object parentObject, FieldInfo property, ParameterDescriptor descriptor)
        {
            return descriptor;
        }

        private void WriteToHistory(CommandExecutionMessageType messageStatus, int exitCode, object message)
        {
            MessageHistory.Messages.Add(CommandUtil.WriteObject(message));
        }

        public ParameterDescriptor DefineArgumentIfTypeExists(object parentObject, FieldInfo field, ParameterDescriptor descriptor)
        {
            return descriptor;
        }

        public ParameterDescriptor DefineArgumentIfTypeExists(object parentObject, PropertyInfo property, ParameterDescriptor descriptor)
        {
            CommandOutput.ErrorMessageEmitting += WriteToHistory;
            CommandOutput.InfoMessageEmitting += WriteToHistory;
            CommandOutput.SuccessMessageEmitting += WriteToHistory;
            CommandOutput.WarningMessageEmitting += WriteToHistory;
            return descriptor;
        }

        public bool IsRuleEnabledInArgumentDefinition(PropertyInfo property)
        {
            return true;
        }

        public bool IsRuleEnabledInArgumentDefinition(FieldInfo field)
        {
            return false;
        }
    }
}
