using InterAppConnector.Attributes;
using InterAppConnector.DataModels;
using InterAppConnector.Interfaces;
using InterAppConnector.Test.SampleCommandsLibrary.DataModels;
using System.Dynamic;

namespace InterAppConnector.Test.SampleCommandsLibrary
{
    [Command("programinfo")]
    public class ProgramInfoCommand : ICommand<ProgramInfoParameter>
    {
        public string Main(ProgramInfoParameter arguments)
        {
            InfoDataModel infoDataModel = new InfoDataModel()
            {
                Version = InterAppCommunication.CallSingleCommand<VersionCommand, EmptyDataModel>().ExecuteAsBatch<string>("version", new ExpandoObject()).Message,
                MessagesCount = MessageHistory.Messages.Count
            };
            return CommandOutput.Ok(infoDataModel);
        }
    }
}
