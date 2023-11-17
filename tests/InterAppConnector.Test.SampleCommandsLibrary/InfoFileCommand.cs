using InterAppConnector.Attributes;
using InterAppConnector.Interfaces;
using InterAppConnector.Test.SampleCommandsLibrary.DataModels;

namespace InterAppConnector.Test.SampleCommandsLibrary
{
    [Command("info", Description = "Gets some info regarding the file")]
    public class InfoFileCommand : ICommand<BaseParameter>
    {
        public string Main(BaseParameter arguments)
        {
            string message = "";
            CommandOutput.Info("Checking if file " + arguments.FilePath + " exists");
            Thread.Sleep(10000);
            if (File.Exists(arguments.FilePath))
            {
                CommandOutput.Info("The file " + arguments.FilePath + " exists. Reading properties from file");
                Thread.Sleep(10000);
                FileInfo file = new FileInfo(arguments.FilePath);
                FileInfoItem item = new FileInfoItem
                {
                    CreationDate = file.CreationTime,
                    FullPath = file.FullName,
                    LastEditedDate = file.LastWriteTime
                };
                message = CommandOutput.Ok(item);
            }
            else
            {
                message = CommandOutput.Error("Error finding file in " + arguments.FilePath + ". The file does not exists");
            }
            return message;
        }
    }
}
