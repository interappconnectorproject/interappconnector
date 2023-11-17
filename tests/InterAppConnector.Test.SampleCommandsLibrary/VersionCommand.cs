using InterAppConnector.Attributes;
using InterAppConnector.DataModels;
using InterAppConnector.Interfaces;
using System.Reflection;

namespace InterAppConnector.Test.SampleCommandsLibrary
{
    [Command("version", Description = "Contains the version of this library")]
    public class VersionCommand : ICommand<EmptyDataModel>
    {
        public string Main(EmptyDataModel arguments)
        {
            return CommandOutput.Ok(Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }
    }
}
