using InterAppConnector.Attributes;
using InterAppConnector.Interfaces;
using InterAppConnector.Test.SampleCommandsLibrary.DataModels;

namespace InterAppConnector.Test.SampleCommandsLibrary
{
    [Command("append", Description = "Append a text to the specified file, if exists. It does not rewrite the content of the file")]
    public class AppendTextCommand : ICommand<FileManagerParameter>
    {
        public string Main(FileManagerParameter arguments)
        {
            string message = "";
            if (File.Exists(Path.GetFullPath(arguments.FilePath)))
            {
                File.AppendAllText(arguments.FilePath, arguments.Text + Environment.NewLine);
                message = CommandOutput.Ok("Text added successfully in the file");
            }
            else
            {
                message = CommandOutput.Error("Error during write operation. The file does not exist");
            }
            return message;
        }
    }
}
