namespace InterAppConnector
{
    /// <summary>
    /// Contains useful methods that helps with structures
    /// </summary>
    public class StructHelper
    {
        /// <summary>
        /// Check if the type is a struct or not
        /// </summary>
        /// <param name="parameterType">The parameter type</param>
        /// <returns><see langword="true"/> if it is a struct, otherwise <see langword="false"/></returns>
        public static bool IsStruct(Type parameterType)
        {
            bool isStruct = false;
            if (parameterType.IsValueType && !parameterType.IsEnum && !parameterType.IsPrimitive)
            {
                isStruct = true;
            }
            return isStruct;
        }
    }
}
