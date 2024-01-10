using InterAppConnector.DataModels;
using System.Reflection;

namespace InterAppConnector.Interfaces
{
    public interface IArgumentDefinitionRule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentObject"></param>
        /// <param name="field"></param>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public ParameterDescriptor DefineArgumentIfTypeExists(object parentObject, FieldInfo field, ParameterDescriptor descriptor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentObject"></param>
        /// <param name="property"></param>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public ParameterDescriptor DefineArgumentIfTypeExists(object parentObject, PropertyInfo property, ParameterDescriptor descriptor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentObject"></param>
        /// <param name="property"></param>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public ParameterDescriptor DefineArgumentIfTypeDoesNotExist(object parentObject, PropertyInfo property, ParameterDescriptor descriptor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentObject"></param>
        /// <param name="property"></param>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public ParameterDescriptor DefineArgumentIfTypeDoesNotExist(object parentObject, FieldInfo property, ParameterDescriptor descriptor);

        /// <summary>
        /// Define if the rule should be executed or not
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool IsRuleEnabledInArgumentDefinition(PropertyInfo property);

        /// <summary>
        /// Define if the rule should be executed or not
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool IsRuleEnabledInArgumentDefinition(FieldInfo field);
    }
}
