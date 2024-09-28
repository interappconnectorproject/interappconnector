using InterAppConnector.Attributes;
using InterAppConnector.DataModels;
using InterAppConnector.Interfaces;
using InterAppConnector.Test.SampleCommandsLibrary.DataModels;
using System.Dynamic;
using System.Reflection;

namespace InterAppConnector.Test.SampleCommandsLibrary
{
    [Command("programinfo")]
    public class ProgramInfoCommand : ICommand<ProgramInfoParameter>
    {
        public string Main(ProgramInfoParameter arguments)
        {
            CommandOutput.Info("Executing command");
            InfoDataModel infoDataModel = new InfoDataModel()
            {
                //Version = InterAppCommunication.CallSingleCommand<VersionCommand, EmptyDataModel>().ExecuteAsBatch<string>("version", new ExpandoObject()).Message,
                Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                MessagesCount = MessageHistory.Messages.Count
            };
            return CommandOutput.Ok(infoDataModel);
        }
    }
}
