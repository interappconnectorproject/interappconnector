using InterAppConnector.Attributes;
using InterAppConnector.Interfaces;
using InterAppConnector.Test.SampleCommandsLibrary.DataModels;

namespace InterAppConnector.Test.SampleCommandsLibrary
{
    [Command("read", Description = "Read the content of the file")]
    public class ReadFileCommand : ICommand<BaseParameter>
    {
        public string Main(BaseParameter arguments)
        {
            string message = "";
            if (File.Exists(Path.GetFullPath(arguments.FilePath)))
            {
                try
                {
                    message = File.ReadAllText(arguments.FilePath);
                }
                catch (Exception exc)
                {
                    message = "There was a problem reading the file. Error is " + exc.Message;
                }
            }
            else
            {
                message = CommandOutput.Error("It is not possible to read the file. The file does not exist");
            }
            return message;
        }
    }
}
