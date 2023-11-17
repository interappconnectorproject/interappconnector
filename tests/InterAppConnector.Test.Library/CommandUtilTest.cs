using InterAppConnector.Test.Library.Commands;
using InterAppConnector.Test.Library.DataModels;
using NUnit.Framework;

namespace InterAppConnector.Test.Library
{
    public class CommandUtilTest
    {

        [Test]
        public void DescribeCommands_SimulateConsoleException_ReturnLineHeightEqualToFive()
        {
            CommandManager manager = new CommandManager();
            manager.AddCommand<CommandTest, TestArgumentClass>();

            Action describeAction = () =>
            {
                CommandUtil.DescribeCommands(manager, true, true);
            };

            Assert.That(describeAction, Throws.Nothing);
        }

        [Test]
        public void WriteObject_WithSimpleObject_ReturnObjectString()
        {
            string expectedString = @"
  Age : 5
  Fruit : 3 elements in the list 
   Banana

   Apple

   Watermelon
  Name : Not set

  EmptyList : No elements

  ObjectExample : 
    Number : 1
    Text : test
    Switch : True";
            DataModelToWrite dataModelToWrite = new DataModelToWrite();

            string writtenObject = CommandUtil.WriteObject(dataModelToWrite);

            Assert.That(expectedString, Is.EqualTo(writtenObject));
        }
    }
}
