using InterAppConnector.DataModels;
using InterAppConnector.Interfaces;
using InterAppConnector.Test.Library.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InterAppConnector.Test.Library.Rules
{
    public class CustomObjectDefinitionRule : IArgumentDefinitionRule<ArgumentWithCustomRule>
    {
        public ParameterDescriptor DefineArgumentIfTypeDoesNotExist(object parentObject, PropertyInfo property, ParameterDescriptor descriptor)
        {
            throw new NotImplementedException();
        }

        public ParameterDescriptor DefineArgumentIfTypeDoesNotExist(object parentObject, FieldInfo property, ParameterDescriptor descriptor)
        {
            throw new NotImplementedException();
        }

        public ParameterDescriptor DefineArgumentIfTypeExists(object parentObject, FieldInfo field, ParameterDescriptor descriptor)
        {
            throw new NotImplementedException();
        }

        public ParameterDescriptor DefineArgumentIfTypeExists(object parentObject, PropertyInfo property, ParameterDescriptor descriptor)
        {
            ArgumentWithCustomRule rule = new ArgumentWithCustomRule
            {
                Count = 1
            };

            if (property.GetValue(parentObject) == null)
            {
                property.SetValue(parentObject, rule);
                descriptor.Value = rule;
            }

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
