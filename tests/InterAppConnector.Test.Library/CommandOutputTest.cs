using InterAppConnector.DataModels;
using InterAppConnector.Enumerations;
using InterAppConnector.Exceptions;
using InterAppConnector.Test.Library.DataModels;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Dynamic;

namespace InterAppConnector.Test.Library
{
    public class CommandOutputTest
    {
        private delegate void TestDelegate();

        private event TestDelegate TestEvent;

        [Test]
        public void IsEventAlreadyAttached_CompareWithTwoSameEvent_ShouldReturnTrue()
        {
            TestEvent += CommandOutputTest_TestEvent;
            TestEvent += CommandOutputTest_TestEvent2;

            TestEvent.Invoke();

            Assert.That(CommandOutput.IsEventAlreadyAttached(TestEvent, CommandOutputTest_TestEvent2), Is.True);
        }

        [Test]
        public void IsEventAlreadyAttached_CompareWithDifferentEventsWithTheSameName_ShouldReturnTrue()
        {
            TestEvent += CommandOutputTest_TestEvent;
            TestEvent += CommandOutputTest_TestEvent2;
            TestEvent += CommandOutputTest_TestEvent;
            TestEvent += CommandOutputTest_TestEvent2;

            TestEvent.Invoke();

            Assert.That(CommandOutput.IsEventAlreadyAttached(TestEvent, CommandOutputTest_TestEvent2), Is.True);
        }

        [Test]
        public void IsEventAlreadyAttached_CompareWitANullEvent_ShouldReturnFalse()
        {
            TestEvent += CommandOutputTest_TestEvent;
            TestEvent += CommandOutputTest_TestEvent2;

            TestEvent.Invoke();

            Assert.That(CommandOutput.IsEventAlreadyAttached(TestEvent, null), Is.False);
        }

        [Test]
        public void IsEventAlreadyAttached_CompareWitANullTarget_ShouldReturnFalse()
        {
            TestEvent += CommandOutputTest_TestEvent;
            TestEvent += CommandOutputTest_TestEvent2;

            TestEvent.Invoke();

            Assert.That(CommandOutput.IsEventAlreadyAttached(null, CommandOutputTest_TestEvent2), Is.False);
        }

        [Test]
        public void IsEventAlreadyAttached_CompareAllNull_ShouldReturnFalse()
        {
            TestEvent += CommandOutputTest_TestEvent;
            TestEvent += CommandOutputTest_TestEvent2;

            TestEvent.Invoke();

            Assert.That(CommandOutput.IsEventAlreadyAttached(null, null), Is.False);
        }

        private void CommandOutputTest_TestEvent()
        {
            Assert.That(CommandOutput.IsEventAlreadyAttached(TestEvent, CommandOutputTest_TestEvent1), Is.False);
        }

        private void CommandOutputTest_TestEvent1()
        {
            throw new NotImplementedException();
        }

        private void CommandOutputTest_TestEvent2()
        {
            Assert.That(CommandOutput.IsEventAlreadyAttached(TestEvent, CommandOutputTest_TestEvent1), Is.False);
        }

        [Test]
        public void Parse_ParseSuccessMessage_ReturnSuccessMessage()
        {
            string successMessage = CommandOutput.Ok("Test passed");

            CommandResult<string> parsedMessage = CommandOutput.Parse<string>(successMessage);

            Assert.That(parsedMessage.MessageStatus, Is.EqualTo(CommandExecutionMessageType.Success));
            Assert.That(parsedMessage.MessageType, Is.EqualTo("System.String"));
            Assert.That(parsedMessage.Message, Is.EqualTo("Test passed"));
            Assert.That(Environment.ExitCode, Is.EqualTo((int)CommandExecutionMessageType.Success - 1));
        }

        [Test]
        public void Parse_ParseSuccessMessageWithWrongReturnType_ReturnFailedMessageTypeMismatch()
        {
            string successMessage = CommandOutput.Ok(false);

            Action parseMessage = () => CommandOutput.Parse<int>(successMessage);

            Assert.That(parseMessage, Throws.InstanceOf(typeof(TypeMismatchException))
                .And.Property("ExpectedType").EqualTo(typeof(int).FullName)
                .And.Property("DeclaredType").EqualTo(typeof(bool).FullName)
                .And.Property("OriginalMessage").EqualTo("{\"MessageStatus\":1,\"MessageType\":\"System.Boolean\",\"Message\":false}"));
        }

        [Test]
        public void Parse_ParseSuccessMessageWithWrongString_ReturnMalformedMessageException()
        {
            string successMessage = "Test";

            Action parseMessage = () => CommandOutput.Parse<int>(successMessage);

            Assert.That(parseMessage, Throws.InstanceOf(typeof(MalformedMessageException))
                .And.Property("OriginalMessage").EqualTo("Test"));
        }

        [Test]
        public void ExtractMessageObject_WithNullArgument_ReturnArgumentException()
        {
            // no arrangements

            Action parseMessage = () => CommandOutput.ExtractMessageObject(null);

            Assert.That(parseMessage, Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        public void ExtractMessageObject_WithDifferentObjectType_ReturnArgumentException()
        {
            PublicationList list = new PublicationList("");

            Action parseMessage = () => CommandOutput.ExtractMessageObject(list);

            Assert.That(parseMessage, Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        public void ExtractMessageObject_WithCorrectObjectType_ReturnObject()
        {
            string message = CommandOutput.Ok("Test");
            CommandResult<string> messageObject = CommandOutput.Parse<string>(message);

            object parsedMessage = CommandOutput.ExtractMessageObject(messageObject);

            Assert.That(parsedMessage, Is.TypeOf(typeof(string)));
            Assert.That(parsedMessage, Is.EqualTo("Test"));
        }

        [Test]
        public void ExtractMessageObject_WithWrongMessageObject_ReturnObject()
        {
            List<EmptyDataModel> argument = new List<EmptyDataModel>();

            Action parseMessage = () => CommandOutput.ExtractMessageObject(argument);

            Assert.That(parseMessage, Throws.InstanceOf<TypeMismatchException>());
        }

        [Test]
        public void Parse_ParseSuccessMessageWithCustomObject_ReturnSuccessMessageWithCustomObjectMessage()
        {
            DataModelExample example = new DataModelExample();
            example.Number = 5;
            example.Text = "Example";
            example.Switch = true;
            string successMessage = CommandOutput.Ok(example);

            CommandResult<DataModelExample> parsedMessage = CommandOutput.Parse<DataModelExample>(successMessage);

            Assert.That(parsedMessage.MessageStatus, Is.EqualTo(CommandExecutionMessageType.Success));
            Assert.That(parsedMessage.MessageType, Is.EqualTo("InterAppConnector.Test.Library.DataModels.DataModelExample"));
            Assert.That(parsedMessage.Message.Number, Is.EqualTo(5));
            Assert.That(parsedMessage.Message.Text, Is.EqualTo("Example"));
            Assert.That(parsedMessage.Message.Switch, Is.EqualTo(true));
            Assert.That(Environment.ExitCode, Is.EqualTo((int)CommandExecutionMessageType.Success - 1));
        }

        [Test]
        public void Parse_ParseInfoMessage_ReturnInfoMessage()
        {
            string successMessage = CommandOutput.Info("Test info");

            CommandResult<string> parsedMessage = CommandOutput.Parse<string>(successMessage);

            Assert.That(parsedMessage.MessageStatus, Is.EqualTo(CommandExecutionMessageType.Info));
            Assert.That(parsedMessage.MessageType, Is.EqualTo("System.String"));
            Assert.That(parsedMessage.Message, Is.EqualTo("Test info"));
            Assert.That(Environment.ExitCode, Is.EqualTo((int)CommandExecutionMessageType.Info - 1));
        }

        [Test]
        public void Parse_ParseWarningMessage_ReturnWarningMessage()
        {
            string successMessage = CommandOutput.Warning("Test warning");

            CommandResult<string> parsedMessage = CommandOutput.Parse<string>(successMessage);

            Assert.That(parsedMessage.MessageStatus, Is.EqualTo(CommandExecutionMessageType.Warning));
            Assert.That(parsedMessage.MessageType, Is.EqualTo("System.String"));
            Assert.That(parsedMessage.Message, Is.EqualTo("Test warning"));
            Assert.That(Environment.ExitCode, Is.EqualTo((int)CommandExecutionMessageType.Warning - 1));
        }

        [Test]
        [TestCase(50)]
        public void Parse_ParseWarningMessageWithCustomStatusCode_ReturnWarningMessage(int customStatusCode)
        {
            string successMessage = CommandOutput.Warning("Test warning", customStatusCode);

            CommandResult<string> parsedMessage = CommandOutput.Parse<string>(successMessage);

            Assert.That(parsedMessage.MessageStatus, Is.EqualTo(CommandExecutionMessageType.Warning));
            Assert.That(parsedMessage.MessageType, Is.EqualTo("System.String"));
            Assert.That(parsedMessage.Message, Is.EqualTo("Test warning"));
            Assert.That(Environment.ExitCode, Is.EqualTo(customStatusCode));
        }

        [Test]
        public void Parse_ParseFailureMessage_ReturnFailureMessage()
        {
            string successMessage = CommandOutput.Error("Test failure");

            CommandResult<string> parsedMessage = CommandOutput.Parse<string>(successMessage);

            Assert.That(parsedMessage.MessageStatus, Is.EqualTo(CommandExecutionMessageType.Failed));
            Assert.That(parsedMessage.MessageType, Is.EqualTo("System.String"));
            Assert.That(parsedMessage.Message, Is.EqualTo("Test failure"));
            Assert.That(Environment.ExitCode, Is.EqualTo((int)CommandExecutionMessageType.Failed - 1));
        }

        [Test]
        public void Parse_ParseWrongMessageWithNoMessageType_ReturnMalformedMessage()
        {
            dynamic message = new ExpandoObject();
            message.Message = 12;
            message.MessageStatus = CommandExecutionMessageType.Success;

            Action parsedMessage = () => { CommandOutput.Parse<LicensePlate>(JsonConvert.SerializeObject(message)); };

            Assert.That(parsedMessage, Throws.InstanceOf<MalformedMessageException>());
        }

        [Test]
        public void Parse_ParseWrongMessageWithWrongMessageType_ReturnMalformedMessage()
        {
            dynamic message = new ExpandoObject();
            message.Message = 12;
            message.MessageType = typeof(LicensePlate).FullName;
            message.MessageStatus = CommandExecutionMessageType.Success;

            Action parsedMessage = () => { CommandOutput.Parse<LicensePlate>(JsonConvert.SerializeObject(message)); };

            Assert.That(parsedMessage, Throws.InstanceOf<MalformedMessageException>());
        }

        [Test]
        [TestCase(-2)]
        public void Parse_ParseFailureMessageWithCustomStatusCode_ReturnFailureMessage(int customStatusCode)
        {
            string successMessage = CommandOutput.Error("Test failure", customStatusCode);

            CommandResult<string> parsedMessage = CommandOutput.Parse<string>(successMessage);

            Assert.That(parsedMessage.MessageStatus, Is.EqualTo(CommandExecutionMessageType.Failed));
            Assert.That(parsedMessage.MessageType, Is.EqualTo("System.String"));
            Assert.That(parsedMessage.Message, Is.EqualTo("Test failure"));
            Assert.That(Environment.ExitCode, Is.EqualTo(customStatusCode));
        }
    }
}
