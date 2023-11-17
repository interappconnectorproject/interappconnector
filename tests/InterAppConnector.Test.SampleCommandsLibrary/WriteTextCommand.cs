using InterAppConnector.Attributes;
using InterAppConnector.Interfaces;
using InterAppConnector.Test.SampleCommandsLibrary.DataModels;

namespace InterAppConnector.Test.SampleCommandsLibrary
{
    [Command("write", Description = "Write a text to the specified file, if exists. It rewrites the content of the file")]
    public class WriteTextCommand : ICommand<FileManagerParameter>
    {
        public string Main(FileManagerParameter arguments)
        {
            string message = "";
            if (File.Exists(Path.GetFullPath(arguments.FilePath)))
            {
                File.WriteAllText(arguments.FilePath, arguments.Text + Environment.NewLine);
                message = CommandOutput.Ok("File rewritten successfully");
            }
            else
            {
                message = CommandOutput.Error("Error during write operation. The file does not exist");
            }
            return message;
        }
    }
}
