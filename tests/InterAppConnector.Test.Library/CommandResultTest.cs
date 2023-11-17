using InterAppConnector.DataModels;
using InterAppConnector.Test.Library.DataModels;
using NUnit.Framework;

namespace InterAppConnector.Test.Library
{
    public class CommandResultTest
    {
        [Test]
        public void CommandResult_DefineStringAsResult_ReturnSystemStringType()
        {
            CommandResult<string> result = new CommandResult<string>();

            // no actions

            Assert.That(result.MessageType, Is.EqualTo("System.String"));
        }

        [Test]
        public void CommandResult_DefineCustomObjectAsResult_ReturnCustomObjectType()
        {
            CommandResult<DataModelExample> result = new CommandResult<DataModelExample>();

            // no actions

            Assert.That(result.MessageType, Is.EqualTo("InterAppConnector.Test.Library.DataModels.DataModelExample"));
        }
    }
}