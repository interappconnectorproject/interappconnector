﻿using InterAppConnector.Enumerations;
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

        internal static List<Type> GetAllRules(List<Assembly> excludedAssemblies)
        {
            List<Type> rules = new List<Type>();
            List<Assembly> assemblies = new List<Assembly>();
            assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies().Except(excludedAssemblies));

            foreach (Type currentType in (from assembly in assemblies
                                          from type in assembly.DefinedTypes
                                          where (type.GetInterface(typeof(IArgumentDefinitionRule).FullName!) != null
                                          || type.GetInterface(typeof(IArgumentSettingRule).FullName!) != null)
                                          && !type.ContainsGenericParameters && !type.IsInterface
                                          select type).ToList())
            {
                rules.Add(currentType);
            }

            return rules;
        }

        internal static List<RuleType> GetAssemblyRules<RuleType>(List<Type> types)
        {
            List<RuleType> rules = new List<RuleType>();

            foreach (Type currentType in types)
            {
                if (currentType.GetInterface(typeof(RuleType).FullName!) != null
                    && !currentType.ContainsGenericParameters && !currentType.IsInterface)
                {
                    rules.Add((RuleType)Activator.CreateInstance(currentType)!);
                }
            }

            return rules;
        }

        internal static List<RuleType> GetAssemblyRules<RuleType>(Assembly assembly)
        {
            List<RuleType> rules = new List<RuleType>();

            foreach (Type currentType in assembly.DefinedTypes)
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
            Dictionary<string, RuleType> rules = new Dictionary<string, RuleType>();

            foreach (RuleType rule in initialRules.Union(rulesToMerge)) 
            {
                rules.TryAdd(rule!.GetType().FullName!, rule);
            }

            return rules.Values.ToList();
        }

        internal static bool IsSpecializedRule(IArgumentDefinitionRule rule)
        {
            return (from item in rule.GetType().GetInterfaces() 
                    where item.FullName!.StartsWith(typeof(IArgumentDefinitionRule<>).FullName!) 
                    select item).Any();
        }

        internal static bool IsSpecializedRule(IArgumentSettingRule rule)
        {
            return (from item in rule.GetType().GetInterfaces()
                    where item.FullName!.StartsWith(typeof(IArgumentSettingRule<>).FullName!)
                    select item).Any();
        }

        internal static bool IsAttributeSpecializedRule(IArgumentDefinitionRule rule)
        {
            return IsSpecializedRule(rule) && (from item in rule.GetType().GetInterfaces()
                                               where item.GenericTypeArguments.Length > 0
                                               where item.GenericTypeArguments[0].IsSubclassOf(typeof(Attribute)) 
                                               select item).Any();
        }

        internal static bool IsAttributeSpecializedRule(IArgumentSettingRule rule)
        {
            return IsSpecializedRule(rule) && (from item in rule.GetType().GetInterfaces()
                                               where item.GenericTypeArguments.Length > 0
                                               where item.GenericTypeArguments[0].IsSubclassOf(typeof(Attribute))
                                               select item).Any();
        }

        internal static bool IsObjectSpecializedRule(IArgumentDefinitionRule rule)
        {
            return IsSpecializedRule(rule) && (from item in rule.GetType().GetInterfaces()
                                               where item.GenericTypeArguments.Length > 0
                                               where !item.GenericTypeArguments[0].IsSubclassOf(typeof(Attribute)) 
                                               select item).Any();
        }

        internal static bool IsObjectSpecializedRule(IArgumentSettingRule rule)
        {
            return IsSpecializedRule(rule) && (from item in rule.GetType().GetInterfaces()
                                               where item.GenericTypeArguments.Length > 0
                                               where !item.GenericTypeArguments[0].IsSubclassOf(typeof(Attribute))
                                               select item).Any();
        }
    }
}
