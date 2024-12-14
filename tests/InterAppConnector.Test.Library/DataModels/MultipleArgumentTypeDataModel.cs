namespace InterAppConnector.Test.Library.DataModels
{

    public class MultipleArgumentTypeDataModel
    {
        public int MandatoryNumber { get; set; }
        public string Name { get; set; }
        public int? Number { get; set; }
        public Guid? Guid { get; set; }
        public Guid MandatoryGuid { get; set; }
        public FileInfo? FileInfo { get; set; }
        public FileInfo MandatoryFileInfo { get; set; } = new FileInfo(@".\");
        public bool MandatorySwitch { get; set; }
        public bool? Switch { get; set; }

    }
}
