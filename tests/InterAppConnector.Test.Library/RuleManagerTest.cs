using InterAppConnector.Interfaces;
using InterAppConnector.Rules;
using InterAppConnector.Test.Library.Rules;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterAppConnector.Test.Library
{
    public class RuleManagerTest
    {
        [Test]
        public void IsAttributeSpecializedRule_WithBuiltInAttributeRule_ReturnTrue()
        {
            // no assumptions

            // no commands

            Assert.That(RuleManager.IsAttributeSpecializedRule(new AliasRule()), Is.True);
            Assert.That(RuleManager.IsAttributeSpecializedRule(new DescriptionRule()), Is.True);
            Assert.That(RuleManager.IsAttributeSpecializedRule(new ExampleValueRule()), Is.True);
            Assert.That(RuleManager.IsAttributeSpecializedRule(new MandatoryForCommandRule()), Is.True);

        }

        [Test]
        public void IsAttributeSpecializedRule_WithCustomObjectRule_ReturnFalse()
        {
            // no assumptions

            // no commands

            Assert.That(RuleManager.IsAttributeSpecializedRule(new CustomObjectDefinitionRule()), Is.False);
        }

        [TestCase]
        public void IsObjectSpecializedRule_WithBuiltInAttributeRule_ReturnFalse()
        {
            // no assumptions

            // no commands

            Assert.That(RuleManager.IsObjectSpecializedRule(new AliasRule()), Is.False);
            Assert.That(RuleManager.IsObjectSpecializedRule(new DescriptionRule()), Is.False);
            Assert.That(RuleManager.IsObjectSpecializedRule(new ExampleValueRule()), Is.False);
            Assert.That(RuleManager.IsObjectSpecializedRule(new MandatoryForCommandRule()), Is.False);

        }

        [Test]
        public void IsObjectSpecializedRule_WithCustomObjectRule_ReturnTrue()
        {
            // no assumptions

            // no commands

            Assert.That(RuleManager.IsObjectSpecializedRule(new CustomObjectDefinitionRule()), Is.True);
        }
    }
}
