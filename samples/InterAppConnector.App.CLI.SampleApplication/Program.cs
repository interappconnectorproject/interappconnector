using InterAppConnector.Test.SampleCommandsLibrary.DataModels;
using InterAppConnector.Test.SampleCommandsLibrary;
using InterAppConnector.DataModels;
using System.Dynamic;

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