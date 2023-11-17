using InterAppConnector.Enumerations;
using System.ComponentModel;

namespace InterAppConnector.Test.SampleCommandsLibrary.DataModels
{
    public class BaseParameter
    {
        [Description("Define the file path")]
        public string FilePath { get; set; }

        [Description("Define the output format")]
        public CommandOutputFormat? OutputFormat { get; set; }
    }
}
