[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=interappconnectorproject_interappconnector&metric=coverage)](https://sonarcloud.io/summary/new_code?id=interappconnectorproject_interappconnector)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=interappconnectorproject_interappconnector&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=interappconnectorproject_interappconnector)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=interappconnectorproject_interappconnector&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=interappconnectorproject_interappconnector)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=interappconnectorproject_interappconnector&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=interappconnectorproject_interappconnector)

[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=interappconnectorproject_interappconnector&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=interappconnectorproject_interappconnector)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=interappconnectorproject_interappconnector&metric=bugs)](https://sonarcloud.io/summary/new_code?id=interappconnectorproject_interappconnector)

# InterAppConnector

InterAppConnector is an integration library that helps to create modules that can be easily integrated in different applications and project types by using a common message format.

You can create one or more modules (called commands) and call them in the same mode without worrying about the project type

# Features

- Easy integration and out-of-box experience with console applications
- Integration with web applications, automated and non-interactive procedures (such as batch, jobs, automated tasks, ...) and other project types
- Argument definition in classes. You can define mandatory and optional arguments that are checked before the command starts
- Support for obfuscated code (in preview)
- Extensible (coming soon)

# Quick example

1. Create a class for parameters and name it `SampleArgument`. Add a string property called `Name`

	```csharp
	public class SampleArgument
	{
		public string Name {get; set;}
	}
	```

2. Create a class for command and name it `SampleCommand`
	1. Add a `Command` attribute and implement `ICommand<SampleArgument>` interface 
	2. In `Main` method write your business logic code

	```csharp
	[Command("hello", Description = "A simple hello command")]
	public class SampleCommand : ICommand<SampleArgument>
	{
		public string Main(SampleArgument command)
		{
			return CommandOutput.Ok("Hello, " + command.Name);
		}
	}
	```

3. Depending on your project type:
	- In a console application, add in your `Program.cs` file in `Main(string[] args)` this code

		```csharp
		CommandManager command = new CommandManager();
		command.AddCommand<SampleCommand, SampleArgument>();

		InterAppCommunication connector = new InterAppCommunication(command);
		connector.ExecuteAsInteractiveCLI(args);
		``` 

	-  In other applications:
		- Parameters must be defined in a `dynamic` object where the name of the properties are the name of the arguments
		- Replace `ExecuteAsInteractiveCLI(string[])` method of the example above with `ExecuteAsBatch(string, dynamic)` method

		```csharp
		dynamic arguments = new ExpandoObject();
		arguments.Name = "John";
		
		CommandManager command = new CommandManager();
		command.AddCommand<SampleCommand, SampleArgument>();

		InterAppCommunication connector = new InterAppCommunication(command);
		CommandResult<SampleArgument> result = connector.ExecuteAsBatch<SampleArgument>("hello", arguments);
		```

4. Build the project if you have created a console application, otherwise go to step 5

5. In order to get the result
	- If you are creating a console application, open the Command Prompt in Windows or Bash in Linux and type the command below (it is assumed that the project name is called SampleProgram)

		```batch
		SampleProgram.exe hello -name John
		```

		You will see a green message with the date and time in current datetime format, the message status followed by the success code and the message `Hello, John`. A typical execution of the program generate the following output:
		
		`[11/10/2023 23:50:34] SUCCESS (0): Hello, John`

	- In other applications, use the object returned by the `ExecuteAsBatch<ArgumentType>` method.
		
# Documentation

For advanced scenarios and in order to learn how to use the library efficiently, see the documentation provided with the library. Below you will find informations about how to find this documentation

## General documentation

The general documentation is available in the [Github Wiki](https://github.com/interappconnectorproject/interappconnector/wiki) section.