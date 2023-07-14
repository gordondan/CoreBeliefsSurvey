namespace CoreBeliefsSurvey.Server
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public string StorageAccountKey { get; set; }
        public string StorageAccountName { get; set; }
        public string TableName { get;set; }
        public string BlobName { get; set; }
    }
}
