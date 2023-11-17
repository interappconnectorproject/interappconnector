using InterAppConnector.DataModels;
using InterAppConnector.Test.Library.DataModels;
using NUnit.Framework;
using System.Dynamic;

namespace InterAppConnector.Test.Library
{
    [TestFixture]
    public class ArgumentTest
    {
        private static readonly string[][] arguments = {
            new string[] {"new", "document", "-documentName", "Make Love Not War.txt", "-text", "War Destroys Everything. Love heals everyone", "-readOnly", "-forceRewrite", "-location", "a\\location", "-template" }
        };

        private static readonly string[][] onlyActions = {
            new string[] {"new", "document" }
        };

        private static readonly string[][] argumentsWithValueWithHyphens = {
            new string[] {"new", "document", "-documentName", "Make Love Not War.txt", "-text", "War Destroys Everything. Love heals everyone", "-readOnly", "-forceRewrite", "-location", "a\\location", "-negativeNumber", "-12" }
        };

        private static readonly string[][] argumentsWithNumbersAsArguments = {
            new string[] {"new", "document", "-12", "a", "-documentName", "Make Love Not War.txt", "-text", "War Destroys Everything. Love heals everyone", }
        };

        [Test]
        public void Parse_WithDynamicObject_ReturnParsedArguments()
        {
            dynamic dynamicArguments = new ExpandoObject();
            dynamicArguments.Number = 1;
            dynamicArguments.NullValue = null;
            dynamicArguments.Switch = true;
            dynamicArguments.String = "Simple Test String";

            Argument argument = Argument.Parse(dynamicArguments, "-");

            Assert.That(argument.Arguments, Is.Not.Null);
            Assert.That(argument.Arguments.Count, Is.EqualTo(3));
            Assert.That(argument.Arguments["number"].Value, Is.EqualTo("1"));
            Assert.That(argument.Arguments, Does.Not.ContainKey("nullvalue"));
            Assert.That(argument.Arguments["switch"].Value, Is.Not.Null);
            Assert.That(argument.Arguments["string"].Value, Is.EqualTo("Simple Test String"));
        }

        [TestCaseSource(nameof(arguments))]
        public void Parse_CheckActions_ReturnParsedArgument(string[] arguments)
        {
            Argument argument;

            argument = Argument.Parse(arguments, "-");

            Assert.That(argument.Action.Count, Is.EqualTo(2));
            Assert.That(argument.Action[0], Is.EqualTo("new"));
            Assert.That(argument.Action[1], Is.EqualTo("document"));
        }

        [TestCaseSource(nameof(arguments))]
        public void Parse_CheckSwitches_ReturnParsedArgument(string[] arguments)
        {
            Argument argument;

            argument = Argument.Parse(arguments, "-");

            Assert.That(argument.GetParameterValue("readonly"), Is.EqualTo(true));
            Assert.That(argument.GetParameterValue("forceRewrite"), Is.EqualTo(true));
            Assert.That(argument.GetParameterValue("template"), Is.EqualTo(true));
        }

        [TestCaseSource(nameof(arguments))]
        public void Parse_CheckParameters_ReturnParsedArgument(string[] arguments)
        {
            Argument argument;

            argument = Argument.Parse(arguments, "-");

            Assert.That(argument.GetParameterValue("text"), Is.EqualTo("War Destroys Everything. Love heals everyone"));
            Assert.That(argument.GetParameterValue("documentName"), Is.EqualTo("Make Love Not War.txt"));
            Assert.That(argument.GetParameterValue("location"), Is.EqualTo("a\\location"));
        }

        [TestCaseSource(nameof(argumentsWithValueWithHyphens))]
        public void Parse_CheckParametersThatContainsHyphens_ReturnParsedArgument(string[] arguments)
        {
            Argument argument;

            argument = Argument.Parse(arguments, "-");

            Assert.That(argument.GetParameterValue("text"), Is.EqualTo("War Destroys Everything. Love heals everyone"));
            Assert.That(argument.GetParameterValue("documentName"), Is.EqualTo("Make Love Not War.txt"));
            Assert.That(argument.GetParameterValue("location"), Is.EqualTo("a\\location"));
            Assert.That(argument.GetParameterValue("negativenumber"), Is.EqualTo("-12"));
        }

        [TestCaseSource(nameof(argumentsWithNumbersAsArguments))]
        public void Parse_CheckArgumentsThatAreNumbers_IgnoreArgumwnts(string[] arguments)
        {
            Argument argument;

            argument = Argument.Parse(arguments, "-");

            Assert.That(argument.Arguments.Count, Is.EqualTo(2));
            Assert.That(argument.GetParameterValue("text"), Is.EqualTo("War Destroys Everything. Love heals everyone"));
            Assert.That(argument.GetParameterValue("documentName"), Is.EqualTo("Make Love Not War.txt"));
        }

        [TestCaseSource(nameof(onlyActions))]
        public void Parse_OnlyActionParameters_ReturnActionList(string[] arguments)
        {
            Argument argument;

            argument = Argument.Parse(arguments, "-");

            Assert.That(argument.Action.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetArgumentName_ArgumentWithNoAliases_ReturnOriginalPropertyName()
        {
            DataModelExample dataModelExample = new DataModelExample();

            string argumentName = Argument.GetArgumentName<DataModelExample>(nameof(dataModelExample.Number));

            Assert.That(argumentName, Is.EqualTo(nameof(dataModelExample.Number)));
        }

        [Test]
        public void GetArgumentName_ArgumentWithAliases_ReturnAliasName()
        {
            TestArgumentClass testArgumentClass = new TestArgumentClass();

            string argumentName = Argument.GetArgumentName<TestArgumentClass>(nameof(testArgumentClass.FirstNumber));

            Assert.That(argumentName, Is.EqualTo("n1"));
        }

        [Test]
        public void IsParameterDefined_TestWithAlias_ReturnPrincipalParameter()
        {
            ParameterDescriptor parameter = new ParameterDescriptor()
            {
                Name = "principalname",
                Aliases = new List<string>
                {
                    "alias",
                    "anotheralias"
                }
            };
            Argument argument = new Argument();
            argument.AddArgument(parameter);

            bool parameterDefined = argument.IsParameterDefined("anotheralias");

            Assert.That(parameterDefined, Is.True);
        }

        [Test]
        public void HasParameterAValue_WithSampleParameter_ReturnTrue()
        {
            Argument argument = new Argument();
            argument.AddArgument("test1", 2)
                .AddArgument("test2", "addiional parameter", 2);

            bool checkIfParameterHasAValue = argument.HasParameterAValue("test1");

            Assert.That(checkIfParameterHasAValue, Is.True);
        }
    }
}
