using System.Reflection;

namespace InterAppConnector
{
    public static class PropertyUtil
    {
        public static bool IsPropertyNullable(PropertyInfo property)
        {
            bool isNullable = false;
            NullabilityInfoContext nullabilityInfoContext = new NullabilityInfoContext();
            NullabilityInfo info = nullabilityInfoContext.Create(property);

            if (info.WriteState == NullabilityState.Nullable || info.ReadState == NullabilityState.Nullable)
            {
                isNullable = true;
            }

            return isNullable;
        }
    }
}
