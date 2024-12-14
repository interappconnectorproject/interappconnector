using InterAppConnector.DataModels;
using InterAppConnector.Test.SampleCommandsLibrary;
using InterAppConnector.Test.SampleCommandsLibrary.DataModels;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace InterAppConnector.App.CLI.SampleApplication
{
    public class Program
    {
        static void Main(string[] args)
        {
            // args = "programinfo".Split(' ');
            //args = "als -test fos".Split(' ');
            //args = "".Split(' ');
            CommandManager command = new CommandManager();
            command.AddCommand<AppendTextCommand, FileManagerParameter>()
                .AddCommand<CreateFileCommand, FileManagerParameter>()
                .AddCommand<WriteTextCommand, FileManagerParameter>()
                .AddCommand<ReadFileCommand, BaseParameter>()
                .AddCommand<InfoFileCommand, BaseParameter>()
                .AddCommand<VersionCommand, EmptyDataModel>()
                .AddCommand<ProgramInfoCommand, ProgramInfoParameter>();


            InterAppCommunication connector = new InterAppCommunication(command);
            connector.ExecuteAsInteractiveCLI(args);
        }


    }
}