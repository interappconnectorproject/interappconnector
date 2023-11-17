using InterAppConnector.Attributes;
using InterAppConnector.Interfaces;
using InterAppConnector.Test.SampleCommandsLibrary.DataModels;

namespace InterAppConnector.Test.SampleCommandsLibrary
{
    [Command("create", Description = "Create a file in the specified file path. It offers the possibility to write a text in the newly created file")]
    public class CreateFileCommand : ICommand<FileManagerParameter>
    {
        public string Main(FileManagerParameter arguments)
        {
            string message;
            if (!File.Exists(Path.GetFullPath(arguments.FilePath)))
            {
                File.Create(arguments.FilePath);
                message = CommandOutput.Ok("File created successfully");
            }
            else
            {
                message = CommandOutput.Error("Error during write operation. The file already exists");
            }
            return message;
        }
    }
}
