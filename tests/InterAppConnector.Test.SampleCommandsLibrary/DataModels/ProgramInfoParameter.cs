using InterAppConnector.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterAppConnector.Test.SampleCommandsLibrary.DataModels
{
    public class ProgramInfoParameter
    {
        [ExcludeItemFromCommand]
        public MessageHistory History { get; set; } = new MessageHistory();
    }
}
