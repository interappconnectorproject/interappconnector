namespace InterAppConnector.Test.Library
{
    public class PublicationList
    {
        public List<string> Items { get; set; } = new List<string>();

        public PublicationList(string listItems)
        {
            string[] publications = listItems.Split(',');
            foreach (string publication in publications)
            {
                Items.Add(publication.Trim());
            }
        }
    }
}
