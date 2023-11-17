namespace InterAppConnector.App.CLI.SampleApplication
{
    public class DataModelToWrite
    {
        public int Age { get; set; } = 5;

        public List<string> Fruit { get; set; } = new List<string>()
        {
            "Banana",
            "Apple",
            "Watermelon"
        };
    }
}
