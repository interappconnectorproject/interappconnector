using InterAppConnector.Attributes;
using System.ComponentModel;

namespace InterAppConnector.Test.SampleCommandsLibrary.DataModels
{
    public class FileManagerParameter : BaseParameter
    {
        [MandatoryForCommand(typeof(WriteTextCommand))]
        [MandatoryForCommand(typeof(AppendTextCommand))]
        [Description("The text to insert into the file")]
        public string Text { get; set; }
    }
}
