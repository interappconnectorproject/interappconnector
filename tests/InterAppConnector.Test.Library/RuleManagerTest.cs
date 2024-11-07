using InterAppConnector.Enumerations;
using InterAppConnector.Interfaces;
using InterAppConnector.Rules;
using InterAppConnector.Test.Library.DataModels;
using InterAppConnector.Test.Library.Rules;
using NUnit.Framework;

namespace InterAppConnector.Test.Library
{
    public class RuleManagerTest
    {
        [Test]
        public void IsAttributeSpecializedRule_WithBuiltInAttributeDefinitionRule_ReturnTrue()
        {
            // no assumptions

            // no commands

            Assert.That(RuleManager.IsAttributeSpecializedRule(new AliasRule()), Is.True);
            Assert.That(RuleManager.IsAttributeSpecializedRule(new DescriptionRule()), Is.True);
            Assert.That(RuleManager.IsAttributeSpecializedRule(new ExampleValueRule()), Is.True);
            Assert.That(RuleManager.IsAttributeSpecializedRule(new MandatoryForCommandRule()), Is.True);

        }

        [Test]
        public void IsAttributeSpecializedRule_WithCustomObjectDefinitionRule_ReturnFalse()
        {
            // no assumptions

            // no commands

            Assert.That(RuleManager.IsAttributeSpecializedRule(new ArgumentWithCustomDefinitionRuleRule()), Is.False);
        }

        [Test]
        public void IsObjectSpecializedRule_WithBuiltInAttributeDefinitionRule_ReturnFalse()
        {
            // no assumptions

            // no commands

            Assert.That(RuleManager.IsObjectSpecializedRule(new AliasRule()), Is.False);
            Assert.That(RuleManager.IsObjectSpecializedRule(new DescriptionRule()), Is.False);
            Assert.That(RuleManager.IsObjectSpecializedRule(new ExampleValueRule()), Is.False);
            Assert.That(RuleManager.IsObjectSpecializedRule(new MandatoryForCommandRule()), Is.False);

        }

        [Test]
        public void IsObjectSpecializedRule_WithCustomObjectDefinitionRule_ReturnTrue()
        {
            // no assumptions

            // no commands

            Assert.That(RuleManager.IsObjectSpecializedRule(new ArgumentWithCustomDefinitionRuleRule()), Is.True);
        }

        [Test]
        public void IsAttributeSpecializedRule_WithBuiltInAttributeSettingRule_ReturnTrue()
        {
            // no assumptions

            // no commands

            Assert.That(RuleManager.IsAttributeSpecializedRule(new ValueValidatorRule()), Is.True);
            Assert.That(RuleManager.IsAttributeSpecializedRule(new CustomInputStringRule()), Is.True);

        }

        [Test]
        public void IsAttributeSpecializedRule_WithCustomObjectSettingRule_ReturnFalse()
        {
            // no assumptions

            // no commands

            Assert.That(RuleManager.IsAttributeSpecializedRule(new ArgumentWithCustomDefinitionRuleRule()), Is.False);
        }

        [Test]
        public void IsObjectSpecializedRule_WithBuiltInAttributeSettingRule_ReturnFalse()
        {
            // no assumptions

            // no commands

            Assert.That(RuleManager.IsObjectSpecializedRule(new ValueValidatorRule()), Is.False);
            Assert.That(RuleManager.IsObjectSpecializedRule(new CustomInputStringRule()), Is.False);

        }

        [Test]
        public void IsObjectSpecializedRule_WithCustomObjectSettingRule_ReturnTrue()
        {
            // no assumptions

            // no commands

            Assert.That(RuleManager.IsObjectSpecializedRule(new ArgumentWithCustomDefinitionRuleRule()), Is.True);
        }
    }
}
