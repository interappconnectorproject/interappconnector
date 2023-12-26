using InterAppConnector.Interfaces;
using System.Reflection;

namespace InterAppConnector
{
    public static class RuleManager
    {
        internal static List<Attribute> GetDistinctAttributes(PropertyInfo property)
        {
            List<Attribute> attributes = new List<Attribute>();

            foreach (Attribute attribute in property.GetCustomAttributes())
            {
                Attribute? findAttribute = (from item in attributes
                                            where item.GetType().FullName == attribute.GetType().FullName
                                            select item).FirstOrDefault();

                if (findAttribute == default(Attribute))
                {
                    attributes.Add(attribute);
                }
            }

            return attributes;
        }

        internal static List<Attribute> GetDistinctAttributes(FieldInfo property)
        {
            List<Attribute> attributes = new List<Attribute>();

            foreach (Attribute attribute in property.GetCustomAttributes())
            {
                Attribute? findAttribute = (from item in attributes
                                            where item.GetType().FullName == attribute.GetType().FullName
                                            select item).FirstOrDefault();

                if (findAttribute == default(Attribute))
                {
                    attributes.Add(attribute);
                }
            }

            return attributes;
        }

        internal static List<Attribute> GetDistinctAttributes(List<object> rawAttributeList)
        {
            List<Attribute> attributes = new List<Attribute>();

            foreach (Attribute attribute in rawAttributeList)
            {
                Attribute? findAttribute = (from item in attributes
                                            where item.GetType() == attribute.GetType()
                                            select item).FirstOrDefault();

                if (findAttribute == default(Attribute))
                {
                    attributes.Add(attribute);
                }
            }

            return attributes;
        }

        internal static List<RuleType> GetAssemblyRules<RuleType>(Type assemblyType)
        {
            List<RuleType> rules = new List<RuleType>();

            foreach (Type currentType in assemblyType.Assembly.ExportedTypes)
            {
                if (currentType.GetInterface(typeof(RuleType).FullName!) != null
                    && !currentType.ContainsGenericParameters && !currentType.IsInterface)
                {
                    rules.Add((RuleType)Activator.CreateInstance(currentType)!);
                }
            }

            return rules;
        }

        internal static List<RuleType> MergeRules<RuleType>(List<RuleType> initialRules, List<RuleType> rulesToMerge)
        {
            List<RuleType> resultingRules = initialRules;

            List<RuleType> additionalRules = (from item in rulesToMerge
                                           where !resultingRules.Contains(item)
                                           select item).ToList();

            return resultingRules.Union(additionalRules).ToList();
        }

        internal static bool IsSpecializedRule(IArgumentDefinitionRule rule)
        {
            return (from item in rule.GetType().GetInterfaces() where item.FullName!.StartsWith(typeof(IArgumentDefinitionRule<>).FullName!) select item).Any();
        }

        internal static bool IsSpecializedRule(IArgumentSettingRule rule)
        {
            return (from item in rule.GetType().GetInterfaces() where item.FullName!.StartsWith(typeof(IArgumentSettingRule<>).FullName!) select item).Any();
        }
    }
}
