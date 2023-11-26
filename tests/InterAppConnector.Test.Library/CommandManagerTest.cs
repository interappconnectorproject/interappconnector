using InterAppConnector.Exceptions;
using System.Globalization;
using NUnit.Framework;
using InterAppConnector.Test.Library.DataModels;
using InterAppConnector.Test.Library.Commands;
using InterAppConnector.Test.SampleCommandsLibrary;
using InterAppConnector.Test.SampleCommandsLibrary.DataModels;
using InterAppConnector.Test.Library.Enumerations;

namespace InterAppConnector.Test.Library
{
    public class CommandManagerTest
    {
        [Test]
        public void AddCommand_AddDuplicteAction_ReturnDuplicateObjectError()
        {
            CommandManager manager = new CommandManager();

            Action commands = () =>
            {
                manager.AddCommand<ReadFileCommand, BaseParameter>()
                    .AddCommand<CommandTest, TestArgumentClass>();
            };

            Assert.That(commands, Throws.InstanceOf(typeof(DuplicateObjectException)));
        }

        [Test]
        public void AddCommand_AddActionWithDuplicateParameter_ReturnDuplicateObjectError()
        {
            CommandManager manager = new CommandManager();

            Action commands = () =>
            {
                manager.AddCommand<ReadFileCommand, BaseParameter>()
                    .AddCommand<CommandTestWithDuplicateParameter, DuplicateParameter>();
            };

            Assert.That(commands, Throws.InstanceOf(typeof(DuplicateObjectException)));
        }

        [Test]
        public void AddCommand_AddCommandWithMultipleCommands_ReturnMultipleCommandNotAllowedException()
        {
            CommandManager manager = new CommandManager();

            Action commands = () =>
            {
                manager.AddCommand<CommandTestWithMultipleCommands, TestArgumentClass>();
            };

            Assert.That(commands, Throws.InstanceOf(typeof(MultipleCommandNotAllowedException)));
        }

        [Test]
        public void AddCommand_AddCommandWithNullAlias_ReturnArgumentExceptionNullAliasNotAllowed()
        {
            CommandManager manager = new CommandManager();

            Action commands = () =>
            {
                manager.AddCommand<CommandTestWithNullAlias, DataModelWithNullAlias>();
            };

            Assert.That(commands, Throws.InstanceOf(typeof(ArgumentException)));
        }

        [Test]
        public void AddCommand_AddCommandWithEmptyStringAlias_ReturnArgumentExceptionEmptyStringAliasNotAllowed()
        {
            CommandManager manager = new CommandManager();

            Action commands = () =>
            {
                manager.AddCommand<CommandTestWithEmptyAlias, DataModelWithEmptyAlias>();
            };

            Assert.That(commands, Throws.InstanceOf(typeof(ArgumentException)));
        }

        [Test]
        public void AddCommand_AddCommandWithWrongAlias_ReturnArgumentException()
        {
            CommandManager manager = new CommandManager();

            Action commands = () =>
            {
                manager.AddCommand<CommandTestWithWrongAlias, DataModelWithWrongAlias>();
            };

            Assert.That(commands, Throws.InstanceOf(typeof(ArgumentException)));
        }

        [Test]
        public void AddCommand_AddExampleCommands_ShouldConfigureObjectProperly()
        {
            CommandManager command = new CommandManager();

            command.AddCommand<AppendTextCommand, FileManagerParameter>()
                .AddCommand<CreateFileCommand, FileManagerParameter>()
                .AddCommand<WriteTextCommand, FileManagerParameter>()
                .AddCommand<ReadFileCommand, BaseParameter>();

            Assert.That(command._commands.Count, Is.EqualTo(4));
            Assert.That(command._commands[typeof(AppendTextCommand).FullName].GetType(), Is.EqualTo(typeof(AppendTextCommand)));
            Assert.That(command._commands[typeof(CreateFileCommand).FullName].GetType(), Is.EqualTo(typeof(CreateFileCommand)));
            Assert.That(command._commands[typeof(WriteTextCommand).FullName].GetType(), Is.EqualTo(typeof(WriteTextCommand)));
            Assert.That(command._commands[typeof(ReadFileCommand).FullName].GetType(), Is.EqualTo(typeof(ReadFileCommand)));
            Assert.That(command._arguments.Count, Is.EqualTo(4));
            Assert.That(command._arguments[typeof(AppendTextCommand).FullName].Arguments.Count, Is.EqualTo(3));
            Assert.That(command._arguments[typeof(CreateFileCommand).FullName].Arguments.Count, Is.EqualTo(3));
            Assert.That(command._arguments[typeof(WriteTextCommand).FullName].Arguments.Count, Is.EqualTo(3));
            Assert.That(command._arguments[typeof(ReadFileCommand).FullName].Arguments.Count, Is.EqualTo(2));
            Assert.That(command._parameterObject.Count, Is.EqualTo(4));
            Assert.That(command._parameterObject[typeof(AppendTextCommand).FullName].GetType(), Is.EqualTo(typeof(FileManagerParameter)));
            Assert.That(command._parameterObject[typeof(CreateFileCommand).FullName].GetType(), Is.EqualTo(typeof(FileManagerParameter)));
            Assert.That(command._parameterObject[typeof(WriteTextCommand).FullName].GetType(), Is.EqualTo(typeof(FileManagerParameter)));
            Assert.That(command._parameterObject[typeof(ReadFileCommand).FullName].GetType(), Is.EqualTo(typeof(BaseParameter)));
        }

        [Test]
        public void AddCommand_CheckMandatoryParameters_ShouldConfigureObjectProperly()
        {
            CommandManager command = new CommandManager();

            command.AddCommand<MultipleArgumentTypeCommand, MultipleArgumentTypeDataModel>();

            Assert.That(command._arguments[typeof(MultipleArgumentTypeCommand).FullName].Arguments.Count, Is.EqualTo(9));
            Assert.That(command._arguments[typeof(MultipleArgumentTypeCommand).FullName].Arguments["mandatorynumber"].IsMandatory, Is.EqualTo(true));
            Assert.That(command._arguments[typeof(MultipleArgumentTypeCommand).FullName].Arguments["mandatoryfileinfo"].IsMandatory, Is.EqualTo(true));
            Assert.That(command._arguments[typeof(MultipleArgumentTypeCommand).FullName].Arguments["mandatoryguid"].IsMandatory, Is.EqualTo(true));
            Assert.That(command._arguments[typeof(MultipleArgumentTypeCommand).FullName].Arguments["mandatoryswitch"].IsMandatory, Is.EqualTo(true));
        }

        [Test]
        public void AddCommand_CheckOptionalParameters_ShouldConfigureObjectProperly()
        {
            CommandManager command = new CommandManager();

            command.AddCommand<MultipleArgumentTypeCommand, MultipleArgumentTypeDataModel>();

            Assert.That(command._arguments[typeof(MultipleArgumentTypeCommand).FullName].Arguments.Count, Is.EqualTo(9));
            Assert.That(command._arguments[typeof(MultipleArgumentTypeCommand).FullName].Arguments["number"].IsMandatory, Is.EqualTo(false));
            Assert.That(command._arguments[typeof(MultipleArgumentTypeCommand).FullName].Arguments["guid"].IsMandatory, Is.EqualTo(false));
            Assert.That(command._arguments[typeof(MultipleArgumentTypeCommand).FullName].Arguments["switch"].IsMandatory, Is.EqualTo(false));
        }

        [Test]
        public void AddCommand_CheckOptionalClassParameters_ShouldConfigureObjectProperly()
        {
            CommandManager command = new CommandManager();

            command.AddCommand<MultipleArgumentTypeCommand, MultipleArgumentTypeDataModel>();

            Assert.That(command._arguments[typeof(MultipleArgumentTypeCommand).FullName].Arguments["name"].IsMandatory, Is.EqualTo(false));
            Assert.That(command._arguments[typeof(MultipleArgumentTypeCommand).FullName].Arguments["fileinfo"].IsMandatory, Is.EqualTo(false));
        }


        [Test]
        public void GetActionKeyByActions_DefineActionListCommand_ReturnClassNameAssociatedWithThatCommand()
        {
            List<string> action = new List<string>
            {
                "read"
            };
            CommandManager command = new CommandManager();
            command.AddCommand<AppendTextCommand, FileManagerParameter>()
                .AddCommand<CreateFileCommand, FileManagerParameter>()
                .AddCommand<WriteTextCommand, FileManagerParameter>()
                .AddCommand<ReadFileCommand, BaseParameter>();

            string actionKey = command.GetActionKeyByActions(action);

            Assert.That(actionKey, Is.EqualTo(typeof(ReadFileCommand).FullName));
        }

        [Test]
        public void SetArgument_ParameterWithSingleAlias_ReturnArgumentSet()
        {
            Argument arguments = Argument.Parse(new[] { "setargument", "-plate", "abcd1234" }, "-");
            CommandManager manager = new CommandManager();
            manager.AddCommand<SetArgumentCommand, SetArgumentMethodDataModel>();

            manager.SetArguments(new List<string>(new[] { "setargument" }), arguments.Arguments.Values.ToList());

            Assert.That(manager._arguments[typeof(SetArgumentCommand).FullName].Arguments["plate"], Is.Not.Null);
            Assert.That(((LicensePlate) manager._arguments[typeof(SetArgumentCommand).FullName].Arguments["plate"].Value).Plate, Is.EqualTo("abcd1234"));
        }

        [TestCase("abcd123a")]
        [TestCase("ab")]
        public void SetArgument_ParameterWithWrongCustomString_ReturnArgumentException(string value)
        {
            Argument arguments = Argument.Parse(new[] { "setargument", "-plate", value }, "-");
            CommandManager manager = new CommandManager();
            manager.AddCommand<SetArgumentCommand, SetArgumentMethodDataModel>();

            Action wrongAction = () =>
            {
                manager.SetArguments(new List<string>(new[] { "setargument" }), arguments.Arguments.Values.ToList());
            };

            Assert.That(wrongAction, Throws.ArgumentException);
        }

        [Test]
        public void SetArgument_ArgumentWithDuplicateInputStringFormat_ReturnArgumentException()
        {
            Argument arguments = Argument.Parse(new[] { "setargument", "-duplicatecustomstringformatclass", "test" }, "-");
            CommandManager manager = new CommandManager();
            manager.AddCommand<SetArgumentCommand, SetArgumentMethodDataModel>();

            Action wrongAction = () =>
            {
                manager.SetArguments(new List<string>(new[] { "setargument" }), arguments.Arguments.Values.ToList());
            };

            Assert.That(wrongAction, Throws.InstanceOf<DuplicateObjectException>());
        }

        [Test]
        public void SetArgument_ParameterWithExceptionInConstructor_ReturnArgumentException()
        {
            Argument arguments = Argument.Parse(new[] { "setargument", "-guid", "xxx" }, "-");
            CommandManager manager = new CommandManager();
            manager.AddCommand<SetArgumentCommand, SetArgumentMethodDataModel>();

            Action wrongAction = () =>
            {
                manager.SetArguments(new List<string>(new[] { "setargument" }), arguments.Arguments.Values.ToList());
            };

            Assert.That(wrongAction, Throws.ArgumentException);
        }

        [Test]
        public void SetArgument_ParameterWithExceptionInCustomConstructor_ReturnArgumentException()
        {
            Argument arguments = Argument.Parse(new[] { "setargument", "-custom", "," }, "-");
            CommandManager manager = new CommandManager();
            manager.AddCommand<SetArgumentCommand, SetArgumentMethodDataModel>();

            Action wrongAction = () =>
            {
                manager.SetArguments(new List<string>(new[] { "setargument" }), arguments.Arguments.Values.ToList());
            };

            Assert.That(wrongAction, Throws.ArgumentException
                                        .And.Message.Contain("No elements was added to the list. Please specify at least one string"));
        }

        [TestCase("-anotherlicenseplate")]
        [TestCase("-anotherlicenseplate2")]
        public void SetArgument_ParameterWithExceptionInConstructorCustomString_ReturnArgumentException(string value)
        {
            Argument arguments = Argument.Parse(new[] { "setargument", value, "$" }, "-");
            CommandManager manager = new CommandManager();
            manager.AddCommand<SetArgumentCommand, SetArgumentMethodDataModel>();

            Action wrongAction = () =>
            {
                manager.SetArguments(new List<string>(new[] { "setargument" }), arguments.Arguments.Values.ToList());
            };

            Assert.That(wrongAction, Throws.ArgumentException);
        }

        [Test]
        public void SetArgument_ParameterWithMultipleAliases_ReturnArgumentSet()
        {
            Argument arguments = Argument.Parse(new[] { "setargument", "-onechar", "a", "-output", "json" }, "-");
            CommandManager manager = new CommandManager();
            manager.AddCommand<SetArgumentCommand, SetArgumentMethodDataModel>();

            manager.SetArguments(new List<string>(new[] { "setargument" }), arguments.Arguments.Values.ToList());

            Assert.That(manager._arguments[typeof(SetArgumentCommand).FullName].Arguments["char"], Is.Not.Null);
            Assert.That(manager._arguments[typeof(SetArgumentCommand).FullName].Arguments["char"].Value, Is.EqualTo('a'));
        }

        [Test]
        public void SetArgument_ParameterWithDefaultConstructor_ReturnArgumentSet()
        {
            Argument arguments = Argument.Parse(new[] { "setargument", "-guid", "e6aad9ac-2205-44ba-bb28-0ad0a1c75a0f", "-output", "json" }, "-");
            CommandManager manager = new CommandManager();
            manager.AddCommand<SetArgumentCommand, SetArgumentMethodDataModel>();

            manager.SetArguments(new List<string>(new[] { "setargument" }), arguments.Arguments.Values.ToList());

            Assert.That(manager._arguments[typeof(SetArgumentCommand).FullName].Arguments["guid"], Is.Not.Null);
            Assert.That(((Guid)manager._arguments[typeof(SetArgumentCommand).FullName].Arguments["guid"].Value).ToString(), Is.EqualTo("e6aad9ac-2205-44ba-bb28-0ad0a1c75a0f"));
        }

        [Test]
        public void SetArgument_ParameterWithExplicitCustomString_ReturnArgumentSet()
        {
            Argument arguments = Argument.Parse(new[] { "setargument", "-otherguid", "e6aad9ac-2205-44ba-bb28-0ad0a1c75a0f", "-output", "json" }, "-");
            CommandManager manager = new CommandManager();
            manager.AddCommand<SetArgumentCommand, SetArgumentMethodDataModel>();

            manager.SetArguments(new List<string>(new[] { "setargument" }), arguments.Arguments.Values.ToList());

            Assert.That(manager._arguments[typeof(SetArgumentCommand).FullName].Arguments["otherguid"], Is.Not.Null);
            Assert.That(((Guid)manager._arguments[typeof(SetArgumentCommand).FullName].Arguments["otherguid"].Value).ToString(), Is.EqualTo("e6aad9ac-2205-44ba-bb28-0ad0a1c75a0f"));
        }

        [Test]
        public void SetArgument_ParameterWithWrongCustomClass_ReturnArgumentSet()
        {
            Argument arguments = Argument.Parse(new[] { "setargument", "-wrong", "e6aad9ac-2205-44ba-bb28-0ad0a1c75a0f", "-output", "json" }, "-");
            CommandManager manager = new CommandManager();
            manager.AddCommand<SetArgumentCommand, SetArgumentMethodDataModel>();

            Action wrongAction = () =>
            {
                manager.SetArguments(new List<string>(new[] { "setargument" }), arguments.Arguments.Values.ToList());
            };

            Assert.That(wrongAction, Throws.InstanceOf<MethodNotFoundException>()
                .And.Property("TargetClassName").EqualTo(typeof(WrongInputStringFormatTest).Name));
        }

        [Test]
        public void SetArgument_ParameterWithWrongParameterNumberInInputStringFormat_ReturnTypeMismatchException()
        {
            Argument arguments = Argument.Parse(new[] { "setargument", "-wrongparameternumberparameter", "e6aad9ac-2205-44ba-bb28-0ad0a1c75a0f", "-output", "json" }, "-");
            CommandManager manager = new CommandManager();
            manager.AddCommand<SetArgumentCommand, SetArgumentMethodDataModel>();

            Action wrongAction = () =>
            {
                manager.SetArguments(new List<string>(new[] { "setargument" }), arguments.Arguments.Values.ToList());
            };

            Assert.That(wrongAction, Throws.InstanceOf<TypeMismatchException>());
        }

        [Test]
        public void SetArgument_ParameterWithWrongParameterInMethodCustomClass_ReturnArgumentSet()
        {
            Argument arguments = Argument.Parse(new[] { "setargument", "-wrongparameter", "parametertest", "-output", "json" }, "-");
            CommandManager manager = new CommandManager();
            manager.AddCommand<SetArgumentCommand, SetArgumentMethodDataModel>();

            Action wrongAction = () =>
            {
                manager.SetArguments(new List<string>(new[] { "setargument" }), arguments.Arguments.Values.ToList());
            };

            Assert.That(wrongAction, Throws.InstanceOf<TypeMismatchException>());
        }

        [Test]
        public void SetArgument_ParameterWithCustomConstructor_ReturnArgumentSet()
        {
            Argument arguments = Argument.Parse(new[] { "setargument", "-custom", "apple,banana", "-output", "json" }, "-");
            CommandManager manager = new CommandManager();
            manager.AddCommand<SetArgumentCommand, SetArgumentMethodDataModel>();

            manager.SetArguments(new List<string>(new[] { "setargument" }), arguments.Arguments.Values.ToList());

            Assert.That(manager._arguments[typeof(SetArgumentCommand).FullName].Arguments["custom"], Is.Not.Null);
            Assert.That(((CustomStringFormatClass)manager._arguments[typeof(SetArgumentCommand).FullName].Arguments["custom"].Value).List[0], Is.EqualTo("apple"));
            Assert.That(((CustomStringFormatClass)manager._arguments[typeof(SetArgumentCommand).FullName].Arguments["custom"].Value).List[1], Is.EqualTo("banana"));
        }

        [Test]
        public void SetArgument_ParameterWithLastArgumentNotSet_ReturnArgumentException()
        {
            Argument arguments = Argument.Parse(new[] { "setargument", "-output", "json", "-char" }, "-");
            CommandManager manager = new CommandManager();
            manager.AddCommand<SetArgumentCommand, SetArgumentMethodDataModel>();

            Action wrongAction = () =>
            {
                manager.SetArguments(new List<string>(new[] { "setargument" }), arguments.Arguments.Values.ToList());
            };

            Assert.That(wrongAction, Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        public void ExecuteCommand_DefineActionListCommand_ReturnActionExecutedSuccessfully()
        {
            List<string> action = new List<string>
            {
                "testvehicle"
            };

            Vehicle parameter = new Vehicle()
            {
                Type = VehicleType.Car,
                LicensePlate = LicensePlate.ParseExact("ab123", "llnnn", CultureInfo.InvariantCulture)
            };

            CommandManager command = new CommandManager();
            command.AddCommand<VehicleTestCommand, Vehicle>();

            bool executeCommand = command.ExecuteCommand(action, parameter);

            Assert.That(executeCommand, Is.EqualTo(true));
        }
    }
}
