using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver.GridFS;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FilesApi.DataAccessLayer
{
    public class Repository : IRepository
    {
        private readonly IGridFSBucket _gridFSBucket;

        
        public Repository(IOptions<MongoDbSettings> mongodbSetting, string bucketName)
        {
            MongoClient client = new(mongodbSetting.Value.ConnectionString);
            var database = client.GetDatabase(mongodbSetting.Value.DatabaseName);
            _gridFSBucket = new GridFSBucket(database, new GridFSBucketOptions
            {
                BucketName = bucketName
            });
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            var fileId = await _gridFSBucket.UploadFromStreamAsync(fileName, fileStream);
            return fileId.ToString();
        }

        public async Task<Stream> DownloadFileAsync(string id)
        {
            var stream = new MemoryStream();
            var objectId = new ObjectId(id);
            await _gridFSBucket.DownloadToStreamAsync(objectId, stream);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public async Task DeleteFileAsync(string id)
        {
            var objectId = new ObjectId(id);
            await _gridFSBucket.DeleteAsync(objectId);
        }
    }
}
