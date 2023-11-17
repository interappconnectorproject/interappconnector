namespace InterAppConnector.Test.Library.DataModels
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
        public string Name { get; set; } = null;
        public List<string> EmptyList { get; set; } = new List<string>();
        public DataModelExample ObjectExample { get; set; } = new DataModelExample
        {
            Number = 1,
            Switch = true,
            Text = "test",
        };
    }
}
