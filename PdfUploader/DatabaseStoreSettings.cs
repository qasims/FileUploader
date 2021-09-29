namespace PdfUploader
{
    public class DatabaseStoreSettings : IDatabaseStoreSettings
    {
        public string FileCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
