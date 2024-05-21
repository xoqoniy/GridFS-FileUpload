namespace FilesApi.DataAccessLayer
{
    public class MongoDbSettings
    {
        public string ConnectionString {  get; set; }
        public string DatabaseName { get; set; }

        //Collection Name - (Like SQL - Table name)
        public string CollectionName { get; set; }
    }
}
