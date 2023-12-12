﻿using InterAppConnector.Enumerations;
using NUnit.Framework;
using System.Dynamic;
using InterAppConnector.DataModels;
using InterAppConnector.Test.Library.DataModels;
using InterAppConnector.Test.Library.Commands;
using InterAppConnector.Test.Library.Enumerations;
using InterAppConnector.Exceptions;
using InterAppConnector.Test.SampleCommandsLibrary.DataModels;
using InterAppConnector.Test.SampleCommandsLibrary;

namespace InterAppConnector.Test.Library
{
    public class InterAppCommunicationTest
    {
        const string Action = "testbatch";

        [Test]
        public void ExecuteAsBatch_ExecuteBatchTestCommandWithSuccessfulMessage_ReturnSuccessfulMessage()
        {
            CommandManager manager = new CommandManager();
            manager.AddCommand<BatchTestCommand, BatchCommandDataModel>(true);
            BatchCommandDataModel batchCommandDataModel = new BatchCommandDataModel
            {
                ExecutionMessageType = CommandExecutionMessageType.Success
            };
            dynamic parameters = new ExpandoObject();
            parameters.ExecutionMessageType = batchCommandDataModel.ExecutionMessageType;

            InterAppCommunication connector = new InterAppCommunication(manager);
            CommandResult<string> result = connector.ExecuteAsBatch<string>(Action, parameters);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.EqualTo("Operation completed successfully"));
            Assert.That(result.MessageStatus, Is.EqualTo(CommandExecutionMessageType.Success));
        }

        [Test]
        public void ExecuteAsBatch_ExecuteBatchTestCommandWithWarningMessage_ReturnWarningMessage()
        {
            CommandManager manager = new CommandManager();
            manager.AddCommand<BatchTestCommand, BatchCommandDataModel>(true);
            BatchCommandDataModel batchCommandDataModel = new BatchCommandDataModel
            {
                ExecutionMessageType = CommandExecutionMessageType.Warning
            };
            dynamic parameters = new ExpandoObject();
            parameters.ExecutionMessageType = batchCommandDataModel.ExecutionMessageType;

            InterAppCommunication connector = new InterAppCommunication(manager);
            CommandResult<string> result = connector.ExecuteAsBatch<string>(Action, parameters);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.EqualTo("Operation completed with a warning"));
            Assert.That(result.MessageStatus, Is.EqualTo(CommandExecutionMessageType.Warning));
        }

        [Test]
        public void ExecuteAsBatch_ExecuteBatchTestCommandWithInfoMessage_ReturnInfoMessage()
        {
            CommandManager manager = new CommandManager();
            manager.AddCommand<BatchTestCommand, BatchCommandDataModel>(true);
            BatchCommandDataModel batchCommandDataModel = new BatchCommandDataModel
            {
                ExecutionMessageType = CommandExecutionMessageType.Info
            };
            dynamic parameters = new ExpandoObject();
            parameters.ExecutionMessageType = batchCommandDataModel.ExecutionMessageType;

            InterAppCommunication connector = new InterAppCommunication(manager);
            CommandResult<string> result = connector.ExecuteAsBatch<string>(Action, parameters);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.EqualTo("An informational message"));
            Assert.That(result.MessageStatus, Is.EqualTo(CommandExecutionMessageType.Info));
        }

        [Test]
        public void ExecuteAsBatch_ExecuteBatchTestCommandWithFailureMessage_ReturnFailureMessage()
        {
            CommandManager manager = new CommandManager();
            manager.AddCommand<BatchTestCommand, BatchCommandDataModel>(true);
            BatchCommandDataModel batchCommandDataModel = new BatchCommandDataModel
            {
                ExecutionMessageType = CommandExecutionMessageType.Failed
            };
            dynamic parameters = new ExpandoObject();
            parameters.ExecutionMessageType = batchCommandDataModel.ExecutionMessageType;

            InterAppCommunication connector = new InterAppCommunication(manager);
            CommandResult<string> result = connector.ExecuteAsBatch<string>(Action, parameters);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.EqualTo("Operation failed"));
            Assert.That(result.MessageStatus, Is.EqualTo(CommandExecutionMessageType.Failed));
        }

        /// llllnnnn
        [Test]
        [Description("This test case is useful in order to test the use of the parameters when their name is not known, " +
            "for instance in the case of a code obfuscation. In fact, during code obfuscation the name of" +
            "the properties may be replaced with other names")]
        public void ExecuteAsBatch_WithEnumAndAliasCorrectParametersAndFieldName_ReturnSuccessfulObject()
        {
            CommandManager command = new CommandManager();
            command.AddCommand<VehicleTestCommand, Vehicle>();
            Vehicle vehicle = new Vehicle();
            dynamic parameters = new ExpandoObject();
            ((IDictionary<string, object>)parameters)[nameof(vehicle.Type)] = "motorcycle";
            ((IDictionary<string, object>)parameters)[nameof(vehicle.LicensePlate)] = "abcd1234";

            InterAppCommunication connector = new InterAppCommunication(command);
            CommandResult<Vehicle> commandExecution = connector.ExecuteAsBatch<Vehicle>("testvehicle", parameters);

            Assert.That(commandExecution.MessageStatus, Is.EqualTo(CommandExecutionMessageType.Success));
            Assert.That(commandExecution.Message.Type, Is.EqualTo(VehicleType.Motorbike));
            Assert.That(commandExecution.Message.LicensePlate.Plate, Is.EqualTo("abcd1234"));
        }

        [Test]
        public void ExecuteAsBatch_WithEnumAndAliasAndCorrectParameters_ReturnSuccessfulObject()
        {
            CommandManager command = new CommandManager();
            command.AddCommand<VehicleTestCommand, Vehicle>();
            dynamic dynamic = new ExpandoObject();
            dynamic.type = "motorcycle";
            dynamic.licenseplate = "abcd1234";

            InterAppCommunication connector = new InterAppCommunication(command);
            CommandResult<Vehicle> commandExecution = connector.ExecuteAsBatch<Vehicle>("testvehicle", dynamic);

            Assert.That(commandExecution.MessageStatus, Is.EqualTo(CommandExecutionMessageType.Success));
            Assert.That(commandExecution.Message.Type, Is.EqualTo(VehicleType.Motorbike));
            Assert.That(commandExecution.Message.LicensePlate.Plate, Is.EqualTo("abcd1234"));
        }

        /// llllnnnn
        [Test]
        public void ExecuteAsBatch_WithEnumAndAliasAndWrongParameters_ReturnFailureObject()
        {
            CommandManager command = new CommandManager();
            command.AddCommand<VehicleTestCommand, Vehicle>();
            dynamic dynamic = new ExpandoObject();
            dynamic.type = "motorcycle";

            Action connectorAction = () =>
            {
                InterAppCommunication connector = new InterAppCommunication(command);
                CommandResult<string> commandExecution = connector.ExecuteAsBatch<string>("testvehicle", dynamic);
            };

            Assert.That(connectorAction, Throws.InstanceOf<MissingMandatoryArgumentException>()
                .And.Property("MissingParameters").Count.EqualTo(1));
        }

        [Test]
        public void ExecuteAsBatch_WithMissingValueTypeArguments_ReturnMissingArgumentExceptions()
        {
            CommandManager command = new CommandManager();
            command.AddCommand<ValueTypeCommand, ValueTypeDataModel>();
            dynamic dynamic = new ExpandoObject();

            Action connectorAction = () =>
            {
                InterAppCommunication connector = new InterAppCommunication(command);
                CommandResult<int> commandExecution = connector.ExecuteAsBatch<int>("valuetype", dynamic);
            };

            Assert.That(connectorAction, Throws.InstanceOf<MissingMandatoryArgumentException>()
                .And.Property("MissingParameters").Count.EqualTo(1));
        }

        [Test]
        public void ExecuteAsBatch_WithMissingOptionalValueTypeArguments_ReturnValue()
        {
            CommandManager command = new CommandManager();
            command.AddCommand<ValueTypeCommand, ValueTypeDataModel>();
            dynamic dynamic = new ExpandoObject();
            dynamic.MandatoryValue = 5;
            int returnedValue = 0;

            Action connectorAction = () =>
            {
                InterAppCommunication connector = new InterAppCommunication(command);
                CommandResult<int> commandExecution = connector.ExecuteAsBatch<int>("valuetype", dynamic);
                returnedValue = commandExecution.Message;
            };

            Assert.That(connectorAction, Throws.Nothing);
            Assert.That(returnedValue, Is.EqualTo(5));
        }

        [Test]
        public void ExecuteAsBatch_WithMissingValidatedOptionalValueTypeArguments_ReturnNoErrors()
        {
            CommandManager command = new CommandManager();
            command.AddCommand<ValidatedValueTypeCommand, ValidatedValueTypeDataModel>();
            dynamic dynamic = new ExpandoObject();
            dynamic.Age = 28;
            int returnedValue = 0;

            Action connectorAction = () =>
            {
                InterAppCommunication connector = new InterAppCommunication(command);
                CommandResult<int> commandExecution = connector.ExecuteAsBatch<int>("validatedvaluetype", dynamic);
                returnedValue = commandExecution.Message;
            };

            Assert.That(connectorAction, Throws.Nothing);
            Assert.That(returnedValue, Is.EqualTo(28));
        }

        [Test]
        public void ExecuteAsBatch_WithWrongOptionalValueTypeArguments_ReturnArgumetnExceptionError()
        {
            CommandManager command = new CommandManager();
            command.AddCommand<ValidatedValueTypeCommand, ValidatedValueTypeDataModel>();
            dynamic dynamic = new ExpandoObject();
            dynamic.Age = 28;
            dynamic.OptionalDate = "12081800";
            int returnedValue = 0;

            Action connectorAction = () =>
            {
                InterAppCommunication connector = new InterAppCommunication(command);
                CommandResult<int> commandExecution = connector.ExecuteAsBatch<int>("validatedvaluetype", dynamic);
                returnedValue = commandExecution.Message;
            };

            Assert.That(connectorAction, Throws.ArgumentException);
        }

        [Test]
        public void ExecuteAsBatch_WithAllValueTypeArgumentsSet_ReturnValue()
        {
            CommandManager command = new CommandManager();
            command.AddCommand<ValueTypeCommand, ValueTypeDataModel>();
            dynamic dynamic = new ExpandoObject();
            dynamic.MandatoryValue = 5;
            dynamic.OptionalValue = 15;
            int returnedValue = 0;

            Action connectorAction = () =>
            {
                InterAppCommunication connector = new InterAppCommunication(command);
                CommandResult<int> commandExecution = connector.ExecuteAsBatch<int>("valuetype", dynamic);
                returnedValue = commandExecution.Message;
            };

            Assert.That(connectorAction, Throws.Nothing);
            Assert.That(returnedValue, Is.EqualTo(20));
        }

        [Test]
        public void ExecuteAsBatch_WithWrongAction_ShouldNotReturnAnException()
        {
            CommandManager command = new CommandManager();
            command.AddCommand<VehicleTestCommand, Vehicle>();
            dynamic dynamic = new ExpandoObject();
            dynamic.type = "motorcycle";

            Action connectorAction = () => 
            {
                InterAppCommunication connector = new InterAppCommunication(command);
                CommandResult<string> commandExecution = connector.ExecuteAsBatch<string>("inexistentaction", dynamic);
            };

            Assert.That(connectorAction, Throws.Nothing);
        }

        [Test]
        public void ExecuteAsInteractiveCLI_WithEnumAndAliasAndCorrectParameters_ReturnSuccessfulStatusCode()
        {
            CommandManager command = new CommandManager();
            command.AddCommand<VehicleTestCommand, Vehicle>();
            string[] arguments = { "testvehicle", "-type", "motorcycle", "-licenseplate", "abcd1234" };

            Action connectorAction = () =>
            {
                InterAppCommunication connector = new InterAppCommunication(command);
                connector.ExecuteAsInteractiveCLI(arguments);
            };

            Assert.That(connectorAction, Throws.Nothing);
            Assert.That(Environment.ExitCode, Is.EqualTo(0));
        }

        [Test]
        public void ExecuteAsInteractiveCLI_WithWrongEnum_ReturnFailureStatusCode()
        {
            CommandManager command = new CommandManager();
            command.AddCommand<VehicleTestCommand, Vehicle>();
            string[] arguments = { "testvehicle", "-type", "caravan", "-licenseplate", "abcd1234" };

            Action connectorAction = () =>
            {
                InterAppCommunication connector = new InterAppCommunication(command);
                connector.ExecuteAsInteractiveCLI(arguments);
            };

            Assert.That(connectorAction, Throws.Nothing);
            Assert.That(Environment.ExitCode, Is.EqualTo(3));
        }

        [Test]
        public void ExecuteAsInteractiveCLI_WithMissingValueTypeArguments_ReturnMissingArgumentMessage()
        {
            CommandManager command = new CommandManager();
            command.AddCommand<ValueTypeCommand, ValueTypeDataModel>();
            string[] arguments = "valuetype".Split(" ");

            Action connectorAction = () =>
            {
                InterAppCommunication connector = new InterAppCommunication(command);
                connector.ExecuteAsInteractiveCLI(arguments);
            };

            Assert.That(connectorAction, Throws.Nothing);
            Assert.That(Environment.ExitCode, Is.EqualTo(3));
        }

        [Test]
        public void ExecuteAsInteractiveCLI_WithMissingOptionalValueTypeArguments_ReturnValue()
        {
            CommandManager command = new CommandManager();
            command.AddCommand<ValueTypeCommand, ValueTypeDataModel>();
            string[] arguments = "valuetype -mandatoryvalue 5".Split(" ");

            Action connectorAction = () =>
            {
                InterAppCommunication connector = new InterAppCommunication(command);
                connector.ExecuteAsInteractiveCLI(arguments);
            };

            Assert.That(connectorAction, Throws.Nothing);
            Assert.That(Environment.ExitCode, Is.EqualTo(0));
        }

        [Test]
        public void ExecuteAsInteractiveCLI_WithMissingOptionalValueTypeArguments_ReturnError()
        {
            CommandManager command = new CommandManager();
            command.AddCommand<ValueTypeCommand, ValueTypeDataModel>();
            string[] arguments = "validatedvaluetype -age 30 -OptionalDate 12051800".Split(" ");

            Action connectorAction = () =>
            {
                InterAppCommunication connector = new InterAppCommunication(command);
                connector.ExecuteAsInteractiveCLI(arguments);
            };

            Assert.That(connectorAction, Throws.Nothing);
            Assert.That(Environment.ExitCode, Is.EqualTo(3));
        }

        [Test]
        public void ExecuteAsInteractiveCLI_WithMissingValidatedOptionalValueTypeArguments_ReturnValue()
        {
            CommandManager command = new CommandManager();
            command.AddCommand<ValidatedValueTypeCommand, ValidatedValueTypeDataModel>();
            string[] arguments = "validatedvaluetype -age 30".Split(" ");

            Action connectorAction = () =>
            {
                InterAppCommunication connector = new InterAppCommunication(command);
                connector.ExecuteAsInteractiveCLI(arguments);
            };

            Assert.That(connectorAction, Throws.Nothing);
            Assert.That(Environment.ExitCode, Is.EqualTo(0));
        }

        [Test]
        public void ExecuteAsInteractiveCLI_WithAllValueTypeArgumentsSet_ReturnValue()
        {
            CommandManager command = new CommandManager();
            command.AddCommand<ValueTypeCommand, ValueTypeDataModel>();
            string[] arguments = "valuetype -mandatoryvalue 5 -optionalvalue 15".Split(" ");

            Action connectorAction = () =>
            {
                InterAppCommunication connector = new InterAppCommunication(command);
                connector.ExecuteAsInteractiveCLI(arguments);
            };

            Assert.That(connectorAction, Throws.Nothing);
            Assert.That(Environment.ExitCode, Is.EqualTo(0));
        }

        [Test]
        public void ExecuteAsInteractiveCLI_WithInexistentAction_ReturnFailureStatusCode()
        {
            CommandManager command = new CommandManager();
            command.AddCommand<VehicleTestCommand, Vehicle>()
                .AddCommand<AppendTextCommand, FileManagerParameter>()
                .AddCommand<CreateFileCommand, FileManagerParameter>()
                .AddCommand<WriteTextCommand, FileManagerParameter>()
                .AddCommand<InfoFileCommand, BaseParameter>()
                .AddCommand<VersionCommand, EmptyDataModel>()
                .AddCommand<CommandTest, TestArgumentClass>();
            string[] arguments = { "inexistentaction", "-type", "motorcycle" };

            Action connectorAction = () =>
            {
                InterAppCommunication connector = new InterAppCommunication(command);
                connector.ExecuteAsInteractiveCLI(arguments);
            };

            Assert.That(connectorAction, Throws.Nothing);
            Assert.That(Environment.ExitCode, Is.Not.EqualTo(0));
        }

        [Test]
        public void ExecuteAsInteractiveCLI_WithNoAction_ReturnNoErrors()
        {
            CommandManager command = new CommandManager();
            command.AddCommand<VehicleTestCommand, Vehicle>()
                .AddCommand<AppendTextCommand, FileManagerParameter>()
                .AddCommand<CreateFileCommand, FileManagerParameter>()
                .AddCommand<WriteTextCommand, FileManagerParameter>()
                .AddCommand<ReadFileCommand, BaseParameter>()
                .AddCommand<InfoFileCommand, BaseParameter>()
                .AddCommand<VersionCommand, EmptyDataModel>()
                .AddCommand<MinimalCommand, EmptyDataModel>();
            string[] arguments = { "-type", "motorcycle" };

            Action connectorAction = () =>
            {
                InterAppCommunication connector = new InterAppCommunication(command);
                connector.ExecuteAsInteractiveCLI(arguments);
            };

            Assert.That(connectorAction, Throws.Nothing);
        }

        [Test]
        public void CallSingleCommand_WithTestCommandConfiguredProperly_ReturnSuccessMessage()
        {
            dynamic dynamic = new ExpandoObject();
            dynamic.type = "motorcycle";
            dynamic.licenseplate = "abcd1234";

            InterAppCommunication connector = InterAppCommunication.CallSingleCommand<VehicleTestCommand, Vehicle>();
            CommandResult<Vehicle> commandExecution = connector.ExecuteAsBatch<Vehicle>("testvehicle", dynamic);

            Assert.That(commandExecution.MessageStatus, Is.EqualTo(CommandExecutionMessageType.Success));
            Assert.That(commandExecution.Message.Type, Is.EqualTo(VehicleType.Motorbike));
            Assert.That(commandExecution.Message.LicensePlate.Plate, Is.EqualTo("abcd1234"));
        }

        [Test]
        public void CallSingleCommand_WithWrongAction_ShouldNotReturnAnException()
        {
            dynamic dynamic = new ExpandoObject();
            dynamic.type = "motorcycle";
            dynamic.licenseplate = "abcd1234";

            Action connectorAction = () =>
            {
                InterAppCommunication connector = InterAppCommunication.CallSingleCommand<VehicleTestCommand, Vehicle>();
                CommandResult<Vehicle> commandExecution = connector.ExecuteAsBatch<Vehicle>("inexistentaction", dynamic);
            };

            Assert.That(connectorAction, Throws.InstanceOf<TypeMismatchException>());
        }

        /// llllnnnn
        [Test]
        public void CallSingleCommand_WithEnumAndAliasAndWrongParameters_ReturnFailureObject()
        {
            dynamic dynamic = new ExpandoObject();
            dynamic.type = "motorcycle";

            Action connectorAction = () =>
            {
                InterAppCommunication connector = InterAppCommunication.CallSingleCommand<VehicleTestCommand, Vehicle>();
                CommandResult<string> commandExecution = connector.ExecuteAsBatch<string>("testvehicle", dynamic);
            };

            Assert.That(connectorAction, Throws.InstanceOf<MissingMandatoryArgumentException>()
                .And.Property("MissingParameters").Count.EqualTo(1));
        }

        [Test]
        public void CallSingleCommand_TestSuccessMessageInBatchMode_ReturnSuccessMessage()
        {
            dynamic arguments = new ExpandoObject();

            InterAppCommunication connector = InterAppCommunication.CallSingleCommand<SuccessMessageTestCommand, EmptyDataModel>();
            connector.InfoMessageEmitted += Connector_InfoMessageEmitted;
            connector.WarningMessageEmitted += Connector_WarningMessageEmitted;
            connector.ErrorMessageEmitted += Connector_ErrorMessageEmitted;
            connector.SuccessMessageEmitted += Connector_SuccessMessageEmitted;
            CommandResult<string> commandExecution = connector.ExecuteAsBatch<string>("successmessage", arguments);

            Assert.That(commandExecution.Message, Is.EqualTo("Operation completed successfully"));
            Assert.That(commandExecution.MessageStatus, Is.EqualTo(CommandExecutionMessageType.Success));
        }

        [Test]
        public void CallSingleCommand_TestWarningMessageInBatchMode_ReturnWarningMessage()
        {
            dynamic arguments = new ExpandoObject();

            InterAppCommunication connector = InterAppCommunication.CallSingleCommand<WarningMessageTestCommand, EmptyDataModel>();
            connector.InfoMessageEmitted += Connector_InfoMessageEmitted;
            connector.WarningMessageEmitted += Connector_WarningMessageEmitted;
            connector.ErrorMessageEmitted += Connector_ErrorMessageEmitted;
            connector.SuccessMessageEmitted += Connector_SuccessMessageEmitted;
            CommandResult<string> commandExecution = connector.ExecuteAsBatch<string>("warningmessage", arguments);

            Assert.That(commandExecution.Message, Is.EqualTo("This command is created in order to test the warning message, so you can ignore this message if you want"));
            Assert.That(commandExecution.MessageStatus, Is.EqualTo(CommandExecutionMessageType.Warning));
        }

        [Test]
        public void CallSingleCommand_TestErrorMessageInBatchMode_ReturnErrorMessage()
        {
            dynamic arguments = new ExpandoObject();

            InterAppCommunication connector = InterAppCommunication.CallSingleCommand<ErrorMessageTestCommand, EmptyDataModel>();
            connector.InfoMessageEmitted += Connector_InfoMessageEmitted;
            connector.WarningMessageEmitted += Connector_WarningMessageEmitted;
            connector.ErrorMessageEmitted += Connector_ErrorMessageEmitted;
            connector.SuccessMessageEmitted += Connector_SuccessMessageEmitted;
            CommandResult<string> commandExecution = connector.ExecuteAsBatch<string>("errormessage", arguments);

            Assert.That(commandExecution.Message, Is.EqualTo("This command is created in order to test the error message, so you can ignore this message if you want"));
            Assert.That(commandExecution.MessageStatus, Is.EqualTo(CommandExecutionMessageType.Failed));
        }

        [TestCase(CommandOutputFormat.Text)]
        [TestCase(CommandOutputFormat.Json)]
        public void CallSingleCommand_TestSuccessMessageInInteractiveMode_ReturnSuccessMessage(CommandOutputFormat format)
        {
            // no assumptions

            InterAppCommunication connector = InterAppCommunication.CallSingleCommand<SuccessMessageTestCommand, EmptyDataModel>();
            connector.ExecuteAsInteractiveCLI(new[] { "successmessage" }, format);

            Assert.That(Environment.ExitCode, Is.EqualTo(0));
        }

        [TestCase(CommandOutputFormat.Text)]
        [TestCase(CommandOutputFormat.Json)]
        public void CallSingleCommand_TestWarningMessageInInteractiveMode_ReturnWarningMessage(CommandOutputFormat format)
        {
            // no assumptions

            InterAppCommunication connector = InterAppCommunication.CallSingleCommand<WarningMessageTestCommand, EmptyDataModel>();
            connector.ExecuteAsInteractiveCLI(new[] { "warningmessage" }, format);

            Assert.That(Environment.ExitCode, Is.EqualTo((int) CommandExecutionMessageType.Warning - 1));
        }

        [TestCase(CommandOutputFormat.Text)]
        [TestCase(CommandOutputFormat.Json)]
        public void CallSingleCommand_TestErrorMessageInInteractiveMode_ReturnErrorMessage(CommandOutputFormat format)
        {
            // no assumptions

            InterAppCommunication connector = InterAppCommunication.CallSingleCommand<ErrorMessageTestCommand, EmptyDataModel>();
            connector.ExecuteAsInteractiveCLI(new[] { "errormessage" }, format);

            Assert.That(Environment.ExitCode, Is.EqualTo((int)CommandExecutionMessageType.Failed - 1));
        }

        private void Connector_SuccessMessageEmitted(CommandExecutionMessageType messageStatus, int exitCode, object message)
        {
            Assert.That(messageStatus, Is.EqualTo(CommandExecutionMessageType.Success));
        }

        private void Connector_ErrorMessageEmitted(CommandExecutionMessageType messageStatus, int exitCode, object message)
        {
            Assert.That(messageStatus, Is.EqualTo(CommandExecutionMessageType.Failed));
        }

        private void Connector_WarningMessageEmitted(CommandExecutionMessageType messageStatus, int exitCode, object message)
        {
            Assert.That(messageStatus, Is.EqualTo(CommandExecutionMessageType.Warning));
        }

        private void Connector_InfoMessageEmitted(CommandExecutionMessageType messageStatus, int exitCode, object message)
        {
            Assert.That(messageStatus, Is.EqualTo(CommandExecutionMessageType.Info));
        }
    }
}
