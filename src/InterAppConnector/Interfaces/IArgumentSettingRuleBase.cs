using InterAppConnector.DataModels;
using System.Reflection;

namespace InterAppConnector.Interfaces
{
    public interface IArgumentSettingRule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentObject"></param>
        /// <param name="property"></param>
        /// <param name="argumentDescriptor"></param>
        /// <param name="userValueDescriptor"></param>
        /// <returns></returns>
        public ParameterDescriptor SetArgumentValueIfTypeExists(object parentObject, PropertyInfo property, ParameterDescriptor argumentDescriptor, ParameterDescriptor userValueDescriptor);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentObject"></param>
        /// <param name="property"></param>
        /// <param name="argumentDescriptor"></param>
        /// <param name="userValueDescriptor"></param>
        /// <returns></returns>
        public ParameterDescriptor SetArgumentValueIfTypeExists(object parentObject, FieldInfo property, ParameterDescriptor argumentDescriptor, ParameterDescriptor userValueDescriptor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentObject"></param>
        /// <param name="property"></param>
        /// <param name="argumentDescriptor"></param>
        /// <returns></returns>
        /// <param name="userValueDescriptor"></param>
        public ParameterDescriptor SetArgumentValueIfTypeDoesNotExist(object parentObject, PropertyInfo property, ParameterDescriptor argumentDescriptor, ParameterDescriptor userValueDescriptor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentObject"></param>
        /// <param name="property"></param>
        /// <param name="argumentDescriptor"></param>
        /// <returns></returns>
        /// <param name="userValueDescriptor"></param>
        public ParameterDescriptor SetArgumentValueIfTypeDoesNotExist(object parentObject, FieldInfo property, ParameterDescriptor argumentDescriptor, ParameterDescriptor userValueDescriptor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool IsRuleEnabledInArgumentSetting(PropertyInfo property);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool IsRuleEnabledInArgumentSetting(FieldInfo field);
    }
}
